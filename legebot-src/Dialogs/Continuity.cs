using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Dialogs
{
    /// <summary>
    /// utility methods and constants to help with saving and restoring
    /// state information across queries but within a conversation.
    /// </summary>
    public class Continuity
    {
        /// <summary>
        /// number of bills to show at one time between "more" queries.
        /// </summary>
        public const int NumBillsToShowInGroup = 5;

        /// <summary>
        /// the default state is Texas (of course)
        /// </summary>
        public const string DefaultState = "Texas";

        /// <summary>
        /// database key to indentify the current bill index number if we are
        /// using "more" to see more bills
        /// </summary>
        public const string KeyBillNumber = "BillNumber";
        /// <summary>
        /// database key used to store the current u.s. state name
        /// </summary>
        public const string KeyState = "State";
        /// <summary>
        /// the key of the current set of bills to cache to display across
        /// 'more' queries.
        /// </summary>
        public const string KeyBillResults = "BillResults";

        /// <summary>
        /// the key for the current legislature definition
        /// </summary>
        public const string KeyLegeSet = "LegeSet";

        /// <summary>
        /// the key for the legislator definition
        /// </summary>
        public const string KeyLegislator = "Legislator";

        /// <summary>
        /// save the current legistlator name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetSavedLegislator(IDialogContext context, string nom)
        {
            try
            {
                if (nom != null && nom != "")
                {
                    context.ConversationData.SetValue(KeyLegislator, nom);
                }
                else
                {                   
                    context.ConversationData.SetValue(KeyLegislator, "");
                }
                return true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// get the current legislator name
        /// </summary>
        /// <param name="context"></param>
        /// <returns>legislator name or blank</returns>
        public static string GetSavedLegislator(IDialogContext context)
        {
            string Legislator = "";
            try
            {
                bool Ok = context.ConversationData.TryGetValue<string>(KeyLegislator, out Legislator);
                if (!Ok || Legislator == null || Legislator  == "")
                {
                    return "";
                }
                else
                {
                    return Legislator;
                }
            }
            catch (System.Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// save the current u.s. state name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetSavedState(IDialogContext context, string state)
        {
            try
            {
                if (state != null && state != "")
                {
                    context.ConversationData.SetValue(KeyState, state);
                }
                else
                {
                    state = DefaultState;
                    context.ConversationData.SetValue(KeyState, state);
                }
                return true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }
        /// <summary>
        /// save the current set of bills that were found.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool SetSavedBills(IDialogContext context, Newtonsoft.Json.Linq.JArray arr)
        {
            try
            {
                if (arr != null)
                {
                    context.ConversationData.SetValue(KeyBillResults, arr);
                }
                else
                {
                    context.ConversationData.SetValue(KeyState, arr);
                }
                return true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }
        /// <summary>
        /// save the current bill index position
        /// </summary>
        /// <param name="context"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool SetSavedBillNumber(IDialogContext context, int num)
        {
            try
            {
                context.ConversationData.SetValue(KeyBillNumber, num);
                return true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// get the current u.s. state name
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSavedState(IDialogContext context)
        {
            string State = DefaultState;
            try
            {
                bool OkState = context.ConversationData.TryGetValue<string>(KeyState, out State);
                if (!OkState || State == null || State == "")
                {
                    State = DefaultState;
                }
            }
            catch (System.Exception)
            {
            }
            return State;
        }

        public static OpenStateClientLib.LegeSet GetLegeSet(IDialogContext context)
        {
            try
            {
                OpenStateClientLib.LegeSet Legeset;
                bool Ok = context.ConversationData.TryGetValue<OpenStateClientLib.LegeSet>(KeyLegeSet, out Legeset);
                if (!Ok || Legeset == null)
                {
                    return null;
                }
                return Legeset;
            }
            catch (System.Exception)
            {
            }
            return null;
        }
   
        /// <summary>
        /// save a LegeSet
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lset"></param>
        /// <returns></returns>
        public static bool SetLegeSet(IDialogContext context, OpenStateClientLib.LegeSet lset)
        {
            try
            {
                context.ConversationData.SetValue(KeyLegeSet, lset);
                return true;
            }
            catch (System.Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// get the current bill index number
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int GetSavedBillNumber(IDialogContext context)
        {
            int BillNumber;
            try
            {

                bool OkBillNumber = context.ConversationData.TryGetValue<int>(KeyBillNumber, out BillNumber);
                if (!OkBillNumber)
                {
                    return -1;
                }
                return BillNumber;
            }
            catch (System.Exception)
            {
            }
            return -1;
        }

        /// <summary>
        /// get the current set of cached bills
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JArray GetSavedBills(IDialogContext context)
        {
            Newtonsoft.Json.Linq.JArray arr = null;
            try
            {
                bool OkBills = context.ConversationData.TryGetValue<Newtonsoft.Json.Linq.JArray>(KeyBillResults, out arr);
                if (!OkBills || arr == null)
                {
                    return null;
                }
            }
            catch (System.Exception)
            {
            }
            return arr;
        }

        /// <summary>
        /// removed the saved bills from the database
        /// </summary>
        /// <param name="context"></param>
        internal static void RemoveSavedBills(IDialogContext context)
        {
            context.ConversationData.RemoveValue(Continuity.KeyBillResults);
        }
    }
}