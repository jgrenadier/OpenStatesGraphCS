using System;
using System.Configuration;
using System.Threading.Tasks;
using LuisBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        private async Task PostIt(IDialogContext context, string msg)
        {
            //
            // fix string so that the carriage return shows up in Slack.
            string Msg2 = msg.Replace("\n", "\n\n");
            await context.PostAsync(Msg2);
        }

        private async Task ShowHelp(IDialogContext context, LuisResult result)
        {
            string Msg = "";
            Msg += "Example dialog that you may type into this bot include:\n";
            Msg += "Set State Texas\n";
            Msg += "Find bills about \"tax\" since 1/28/2017\n";
            Msg += "Find \"school\" bills since 1/28/2017\n";
            Msg += "Describe bills between 1/28/2017 to 12/28/2017\n";
            Msg += "Find bills since 2017\n";
            Msg += "Describe bills since last year\n";
            Msg += "Find \"tax\" bills\n";
            Msg += "Legislators\n";
            Msg += "Legislator \"Foghorn\"\n";
            Msg += "Legislator \"all\"\n";
            Msg += "Legislator \"none\"\n";
            // await context.PostAsync(Msg);
            await PostIt(context, Msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            // await this.ShowLuisResult(context, result);
            await ShowHelp(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            // await this.ShowLuisResult(context, result);
            await ShowHelp(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            // await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            // await this.ShowLuisResult(context, result);
            await ShowHelp(context, result);
        }

        private async void ExtraGuiActionBill(Newtonsoft.Json.Linq.JToken bill)
        {
        }

        [LuisIntent("Find")]
        public async Task FindIntent(IDialogContext context, LuisResult result)
        {
            try
            {
                string CurState;          
                CurState = Continuity.GetSavedState(context);
                string SearchTerm = BillDialogUtil.GetSearchTerm(result);

                string CurVoter;
                CurVoter = Continuity.GetSavedLegislator(context);

                DateTime MinTime;
                DateTime MaxTime;
                bool Ok = BillDialogUtil.GetEntityDateRange(result, out MinTime, out MaxTime);
                // if (Ok)
                {
                    // await context.PostAsync($"SearchTerm = {SearchTerm} MinTime = {MinTime}, MaxTime = {MaxTime}");
                    // // context.Wait(MessageReceived);

                    OpenStateClientLib.OpenStateClient cli = new OpenStateClientLib.OpenStateClient();
                    
                    Newtonsoft.Json.Linq.JArray arr = await cli.GetBillsFilteredAsync(CurState, MinTime, MaxTime, SearchTerm, null, ExtraGuiActionBill);
                    int CurrentBill = 0;

                    bool WillBeMore;
                    int MaxToShow = Continuity.NumBillsToShowInGroup;
                    if (MaxToShow >= arr.Count)
                    {
                        MaxToShow = arr.Count;
                        WillBeMore = false;
                    }
                    else
                    {
                        WillBeMore = true;
                    }

                    bool MoreToDisplay = false;
                    for (int ii = 0; ii < MaxToShow; ii++)
                    {
                        CurrentBill = ii;
                        MoreToDisplay = (ii >= (arr.Count - 1));
                        await BillDialogUtil.DisplayOneBill(context, arr[ii], CurState, CurVoter);
                    }
                    if (MoreToDisplay)
                    {                      
                        await context.PostAsync("Type 'More' to see more bills.\n");
                    }
                    // context.ConversationData.SetValue(KeyBillNumber, CurrentBill + 1);
                    Continuity.SetSavedBillNumber(context, CurrentBill + 1);
                    if (WillBeMore)
                    {
                        // context.ConversationData.SetValue(KeyBillResults, arr);
                        Continuity.SetSavedBills(context, arr);
                        if (SearchTerm != null && SearchTerm != "")
                        {
                            await context.PostAsync($"Type 'More' to see more '{SearchTerm}' bills.\n");
                        }
                        else
                        {
                            await context.PostAsync("Type 'More' to see more bills.\n");
                        }
                    }
                    else
                    {                        
                        Continuity.RemoveSavedBills(context);
                    }

                }
                context.Wait(MessageReceived);
            }
            catch(System.Exception)
            {

            }
        }

  
        /// <summary>
        /// If the number of bills to display is large, just show some of them
        /// and allow the user to see more of them by typing "More".
        /// Note: This kind of dialog sequence could be applied to other displays
        /// than just bills and the pattern of this code could be generalized.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("More")]
        public async Task MoreIntent(IDialogContext context, LuisResult result)
        {           
            int BillNumber = Continuity.GetSavedBillNumber(context);
            if (BillNumber < 0)
            {
                // none left
                await context.PostAsync("Not anymore bills to show.");
                context.Wait(MessageReceived);
                return;
            }
            string CurVoter = Continuity.GetSavedLegislator(context);

            Newtonsoft.Json.Linq.JArray arr = Continuity.GetSavedBills(context);
            string State = Continuity.GetSavedState(context);
        
            if (arr == null)
            {
                await context.PostAsync("No bills saved to show.");
                context.Wait(MessageReceived);
                return;
            }
            int NumLeft = arr.Count;
            if (BillNumber >= NumLeft)
            {
                // none left
                await context.PostAsync("No more bills to show.");
                context.Wait(MessageReceived);
                return;
            }
            int NumToShow = NumLeft;
            if (NumToShow > Continuity.NumBillsToShowInGroup)
            {
                NumToShow = Continuity.NumBillsToShowInGroup;
            }
 

            for (int i = BillNumber; i < BillNumber + NumToShow; i++)
            {                
                if (arr != null && i < arr.Count) 
                {
                    await BillDialogUtil.DisplayOneBill(context, arr[i], State, CurVoter); 
                }
            }

            BillNumber += NumToShow; 
            if (BillNumber < arr.Count)
            {
                Continuity.SetSavedBillNumber(context, BillNumber);
                await context.PostAsync("Type 'More' to see more bills.\n");
            }
            else
            {
                Continuity.SetSavedBills(context, null);
                Continuity.SetSavedBillNumber(context, -1);
            }
  
        }



        [LuisIntent("State")]
        public async Task StateIntent(IDialogContext context, LuisResult result)
        {
            string state = BillDialogUtil.GetEntityState(result);

            if (state != null && state != "" && state != "nothing")
            {
                
                Continuity.SetSavedState(context, state);
            }
            else
            {
                state = Continuity.GetSavedState(context);
            }
            await context.PostAsync("All hail the great state of " + state + ".");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Legislator")]
        public async Task LegislatorIntent(IDialogContext context, LuisResult result)
        {

            // await context.PostAsync("result.Query = " + result.Query);
            // context.Wait(MessageReceived);

            string State = Continuity.GetSavedState(context);
            if (State == null || State == "")
            {
                await context.PostAsync("No State selected" + ".");
                context.Wait(MessageReceived);
                return;
            }
            string SearchTerm = BillDialogUtil.GetSearchTerm(result);


            OpenStateClientLib.OpenStateClient cli = new OpenStateClientLib.OpenStateClient();
            
            OpenStateClientLib.LegeSet lset = await cli.GetStatePeopleAsync(State);
            if (lset == null)
            {
                await context.PostAsync("Cannot get legislator list" + ".");
                context.Wait(MessageReceived);
                return;
            }
            string Summary = lset.SummarizeLeges();

            if (SearchTerm == null || SearchTerm == "")
            {
                string OldLegislator = Continuity.GetSavedLegislator(context);
                if (OldLegislator != null && OldLegislator != "")
                {
                    Summary += "Selected Legislator is " + OldLegislator + "!\n";
                }
                // await context.PostAsync(Summary);
                await PostIt(context, Summary);
                context.Wait(MessageReceived);
                return;
            }

            if (SearchTerm != null || SearchTerm != "")
            {              
                Continuity.SetSavedLegislator(context, SearchTerm);
                string Msg;
                if (SearchTerm.ToLower() == "all")
                {
                    string Msg2 = $"We will show the votes made by all legislators\nwhen you search for bills.";
                    Msg = Msg2;
                    await context.PostAsync(Msg);
                    context.Wait(MessageReceived);
                    return;
                }
                else if (SearchTerm.ToLower() == "none")
                {
                    string Msg2 = $"We will show no votes made by legislators\nwhen you search for bills.";
                    Msg = Msg2;
                    await context.PostAsync(Msg);
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    int NumFound = lset.FindLegislator(SearchTerm, out Msg);
                    if (NumFound == 1)
                    {
                        string Msg2 = $"We will show the votes made by this legislator\nwhen you search for bills.";
                        Msg += ("\n" + Msg2);
                        await context.PostAsync(Msg);
                        context.Wait(MessageReceived);
                        return;
                    }
                    if (NumFound >= 0)
                    {
                        string Msg2 = $"We will show the votes made by these legislators\nwhen you search for bills.";
                        Msg += ("\n" + Msg2);
                        await context.PostAsync(Msg);
                        context.Wait(MessageReceived);
                        return;
                    }
                    else
                    {
                        await context.PostAsync(Msg);
                        context.Wait(MessageReceived);
                        return;
                    }
                }
            }
 
        }


        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }

   
    }
}