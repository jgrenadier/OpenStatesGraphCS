using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using OpenStateClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LuisBot.Dialogs
{
    /// <summary>
    /// utulities to handle our application specific LUIS data 
    /// </summary>
    public class BillDialogUtil
    {
        /// <summary>
        /// utility to interpret a LUIS date range entity
        /// </summary>
        /// <param name="result"></param>
        /// <param name="mintime"></param>
        /// <param name="maxtime"></param>
        /// <returns></returns>
        public static bool GetEntityDateRange(LuisResult result, out DateTime mintime, out DateTime maxtime)
        {
            bool IsOk = false;
            mintime = DateTime.MinValue;
            maxtime = DateTime.MaxValue;
            try
            {
                if (result != null)
                {
                    if (result.Entities != null && result.Entities.Count > 0)
                    {
                        EntityRecommendation reco = result.Entities[0];
                        if (reco != null && reco.Resolution != null)
                        {
                            System.Collections.Generic.IDictionary<string, object> dict = reco.Resolution;
                            object oval2 = null;
                            bool OkValues = dict.TryGetValue("values", out oval2);
                            if (OkValues)
                            {
                                System.Collections.Generic.List<object> list2 = (System.Collections.Generic.List<object>)oval2;
                                int n = list2.Count;
                                for (int i = 0; i < n; i++)
                                {

                                    object o2 = list2[i];
                                    if (o2 != null)
                                    {
                                        System.Collections.Generic.Dictionary<string, object> dict2 = (System.Collections.Generic.Dictionary<string, object>)o2;
                                        object Timex = null;
                                        string sTimex = null;
                                        object Mod = null;
                                        string sMod = null;
                                        object Typ = null;
                                        string sTyp = null;
                                        object Start = null;
                                        string sStart = null;
                                        DateTime StartFound = System.DateTime.MinValue;
                                        object End = null;
                                        string sEnd = null;
                                        DateTime EndFound = System.DateTime.MinValue;
                                        bool OkDate = false;
                                        bool OkTimex = dict2.TryGetValue("timex", out Timex);
                                        if (OkTimex)
                                        {
                                            sTimex = Timex.ToString();
                                        }
                                        bool OkMod = dict2.TryGetValue("Mod", out Mod);
                                        if (OkMod)
                                        {
                                            sMod = Mod.ToString();
                                        }
                                        bool OkTyp = dict2.TryGetValue("type", out Typ);
                                        if (OkTyp)
                                        {
                                            sTyp = Typ.ToString();
                                        }
                                        bool OkStart = dict2.TryGetValue("start", out Start);
                                        if (OkStart)
                                        {
                                            sStart = Start.ToString();
                                            OkDate = System.DateTime.TryParse(Start.ToString(), out StartFound);
                                        }
                                        else
                                        {
                                            OkStart = dict2.TryGetValue("value", out Start);
                                            if (OkStart)
                                            {
                                                sStart = Start.ToString();
                                                OkDate = System.DateTime.TryParse(Start.ToString(), out StartFound);
                                            }
                                        }
                                        if (sTyp != null && (sTyp == "daterange" || sTyp == "date"))
                                        {
                                            if (sStart != null && OkDate)
                                            {
                                                mintime = StartFound;
                                                IsOk = true;
                                            }
                                        }
                                        bool OkEnd = dict2.TryGetValue("end", out End);
                                        if (OkEnd)
                                        {
                                            sEnd = End.ToString();
                                            OkDate = System.DateTime.TryParse(End.ToString(), out EndFound);
                                        }
                                        if (sTyp != null && sTyp == "daterange")
                                        {
                                            if (sEnd != null && OkDate)
                                            {
                                                maxtime = EndFound;
                                                IsOk = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return IsOk;
        }


        /// <summary>
        /// utility to interpret the LUIS state entity that I defined to find
        /// any one of the fify states.
        /// Note: This should be extended to include Puerto Rico etc.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetEntityState(LuisResult result) 
        {
            // DateTime mintime;
            // DateTime maxtime;
            // bool IsOk; 
            try
            {
                if (result != null) 
                {
                    if (result.Entities != null && result.Entities.Count > 0)
                    {
                        EntityRecommendation reco = result.Entities[0];
                        if (reco != null && reco.Resolution != null) 
                        {
                            string Msg = "";
                            System.Collections.Generic.IDictionary<string, object> dict = reco.Resolution;
                            foreach (var s in dict.Keys)
                            {
                                Msg += (s + "!");
                                string Val = dict[s].ToString();
                                Msg += (Val);
                            }

                            object oval2 = null;
                            bool OkValues = dict.TryGetValue("values", out oval2);
                            if (OkValues)
                            {
                                System.Collections.Generic.List<object> list2 = (System.Collections.Generic.List<object>)oval2;
                                int n = list2.Count;
                                Msg = "";
                                if (n > 0)
                                {
                                    Msg = list2[0].ToString();
                                }
                                return Msg;
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return "nothing";
        }

        /// <summary>
        /// utility method to tell some random things about entities for
        /// diagnostic purposes. Not currently used.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetEntitySummary(LuisResult result)
        {
            string Message = "";
            try
            {
                int n = result.Entities.Count;
                for (int i = 0; i < n; i++)
                {
                    string sEntity = result.Entities[i].Entity;
                    string sRole = result.Entities[i].Role;
                    string sType = result.Entities[i].Type;
                    string sLine = $"Entity = {sEntity}, Role = {sRole}, Type = {sType} \n";
                    Message += sLine;
                }
            }
            catch (System.Exception)
            {

            }
            return Message;
        }

        /// <summary>
        /// convert a character array to a string
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static string CharArrayToString(char[] arr)
        {
            string All = "";
            try
            {
                int n = arr.Length;
                for (int i = 0; i < n; i++)
                {
                    All += arr[i];
                }
            }
            catch (System.Exception)
            {

            }
            return All;
        }

        /// <summary>
        /// provide a query string with entities removed.
        /// This is not currently used.  
        /// The purpose was to helo isolate the subject text that
        /// the user is interested in searching for.  
        /// For now, the user should put double quotes around
        /// the subject text for searching.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string StripEntities(LuisResult result)
        {
            char[] Remaining = result.Query.ToCharArray();
            try
            {
                int n = result.Entities.Count;
                for (int i = 0; i < n; i++)
                {
                    EntityRecommendation er = result.Entities[i];
                    if (er.StartIndex != null)
                    {
                        for (int j = (int)er.StartIndex; j <= (int)er.EndIndex; j++)
                        {
                            Remaining[j] = ' ';
                        }
                    }

                }
            }
            catch (System.Exception)
            {

            }
            string s = CharArrayToString(Remaining);
            return s;
        }

        /// <summary>
        /// extract the search term from the query string.
        /// For example, the query 'find "tax" bills from 1/1/2017' , the
        /// search term is "tax".
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetSearchTerm(LuisResult result)
        {
            string Message = null;
            try
            {
                string s = result.Query;
                char[] Quotes = "\"'".ToCharArray();
                int Start = s.IndexOfAny(Quotes);
                int End = s.LastIndexOfAny(Quotes);
                int Len = (End - Start) - 1;
                if (Start >= 0 && End >= 0 && Start != End && Len > 0)
                {
                    string s2 = s.Substring(Start + 1, Len);
                    return s2;
                }
                //string s3 = BillDialogUtil.StripEntities(result);
                //int ii = s3.ToLower().IndexOf("about");
                //if (ii >= 0)
                //{
                //    string s4 = s3.Substring(ii + 5);
                //    int iNextBlank = s4.IndexOf(" ");
                //    s4 = s4.Substring(0, iNextBlank);
                //    return s4;
                //}
                //return s3;
            }
            catch (System.Exception)
            {

            }
            return Message;
        }

        /// <summary>
        /// display text that represents a bill to the bot
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bill"></param>
        /// <param name="curstate"></param>
        /// <param name="curvoter"></param>
        /// <returns></returns>
        public static async Task DisplayOneBill(IDialogContext context, Newtonsoft.Json.Linq.JToken bill, string curstate, string curvoter)
        {
  
            string Msg = OpenStateClient.DisplayOneBill(bill, curstate, curvoter);
            await context.PostAsync(Msg);
            // RecentContext.Wait(MessageReceived);

        }


    }
}