using GraphQL.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStateClientLib
{

    /// <summary>
    /// class that is used by client software to interrogate
    /// the Open States ( https://openstates.org/ ) state legislative data
    /// using GraphQL API ( http://docs.openstates.org/en/latest/api/v2/index.html )
    /// This library uses the GraphQL.Client nuget package and was testing with v1.0.3.  
    /// </summary>
    public class OpenStateClient : IDisposable
    {
        public OpenStateClient()
        {
            Init();
        }

        #region Properties

        /// <summary>
        /// there is basically one endpoint for accessing all of the open states
        /// graphql api.  For now an api key is not necessary.
        /// </summary>
        public const string EndPointS = @"http://alpha.openstates.org/graphql";

        // Note: The documentation says that you can use an api key while for now
        // it is not necessary.  I havent had much luck getting the api to work with a key
        // so perhaps I was doing it wrong as in...
        //  ucon.addRequestProperty("x-api-key", apiKey);

        private GraphQL.Client.GraphQLClient GraphQLClient  = null;

        #endregion Properties

        /// <summary>
        /// initialization used in the constructor
        /// </summary>
        private void Init()
        {
            string Full = EndPointS;
           
            Uri endPoint = new Uri(Full);
           
            var options = new GraphQL.Client.GraphQLClientOptions();
            options.EndPoint = endPoint;          
            this.GraphQLClient = new GraphQL.Client.GraphQLClient(options);
           
            
        }
        /// <summary>
        /// dispose of any resources to avoid memory leaks
        /// </summary>
        public void Dispose() =>
            this.GraphQLClient.Dispose();

        /// <summary>
        /// quote mark that we can use in our query strings and replace them
        /// with double quotes be using.
        /// </summary>
        private const string QuoteMarkUsed = "'";
        /// <summary>
        /// GraphQL query strings often include double quote marks that
        /// are a bit awkward to use in C# string initialization.  So, I allow
        /// the query strings to use things like single quotes instead and substitute the double
        /// quote mark.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static string FixQueryQuotes(string q)
        {
            if (q == null)
            {
                return "";
            }
            string s = q.Replace(QuoteMarkUsed, "\"");
            return s;
        }


        /// <summary>
        /// list up to 100 bills for a given jurisdiction (like a state) / Session. 
        /// The classification of "bills" eliminates the less important "resolution"s.
        /// $state example is Texas
        /// $session example is 851
        /// Note: graphql has a feature that should allow us to use 'query variables' to set
        /// parameters without using string interpolation.  However, I didnt have success with
        /// query variables so i am using '$' as a way of identifying strings that I want to replace
        /// using string interpolation.
        /// </summary>
        private const string QsBills = @"
query BillsByStateSession {
    search_1: bills(first: 100, after:""$cursor"", jurisdiction: ""$state"", session:""$session"", classification:""bill"") {
        edges {
            node {
                id
                identifier
                title
                classification
                updatedAt
                createdAt
                subject
                legislativeSession {
                    identifier
                    jurisdiction
                    {
                        name
                    }
                }
                documents {
                    date
                    note
                    links {
                        url
                    }
                }
                sources {
                    url
                    note
                }                
                votes {
                  edges {
                    node {
                      id
                      votes {
                        option
                        voterName
                        note
                      }
                    }
                  }
                }
            }
            cursor
        }
    }
}";

        /// <summary>
        /// Get a set of n bills starting at a cursor position
        /// </summary>
        /// <param name="state"></param>
        /// <param name="session"></param>
        /// <param name="cursor">usually null (so as to start at the first occurance)</param>
        /// <returns>if null or blank, start at the beginnning</returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetBillsAsync(string state, string session, string cursor)
        {
            var graphQLRequest = new GraphQLRequest();

            string q = QsBills;
            //
            // use simple string interpolation to set the state and session
            // parameters.
            //
            // note: It should be possible to use query variables but I havent been able
            // to get that working in OpenStates yet.
            q = q.Replace("$state", state);
            q = q.Replace("$session", session);
            //
            // use the cursor only if not reading the first bills available
            //
            if (cursor == null || cursor == "")
            {
                q = q.Replace(@"after:""$cursor"",", "");
            }
            else
            {
                q = q.Replace("$cursor", cursor);
            }
            //
            // start reading the bills
            //
            graphQLRequest.Query = q;
            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);
            return response;
        }

   

        /// <summary>
        /// get the number of edges
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private static int GetCount(Newtonsoft.Json.Linq.JArray arr)
        {
            if (arr == null)
                return 0;
            return arr.Count;
        }

   

        /// <summary>
        /// find the last cursor in a set of edges.
        /// If we are reading something that has more elements than can be read in
        /// one fell swoop, we will get the cursor of the last element so that we can
        /// ask for more elements with the "after" clause in the next query.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetNextCursor(Newtonsoft.Json.Linq.JArray results, int count)
        {
            if (count <= 0)
            {
                return null;
            }
            // string Cursor = results[count - 1].ToString();
            string Cursor = results[count - 1]["cursor"].ToString();
            return Cursor;
        }


   

        /// <summary>
        /// append the results to a summary result.
        /// This code assumes that the summary results can be safely appended to.
        /// It works fine but since I dont know the the internals of the code that
        /// generated GraphQLResponse instances, I may have to change this implementation
        /// later.
        /// </summary>
        /// <param name="resultsall"></param>
        /// <param name="results"></param>
        /// <param name="count"></param>
        private static void AppendResults(ref Newtonsoft.Json.Linq.JArray resultsall, Newtonsoft.Json.Linq.JArray results, int count)
        {
            if (count <= 0)
            {
                return;
            }
            if (resultsall == null)
            {
                resultsall = results;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    resultsall.Add(results[i]);
                }
            }
        }

        /// <summary>
        /// check if a cursor string is reasonable
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        private static bool IsCursorOK(string cursor)
        {
            if (cursor == null || cursor == "")
            {
                return false;
            }
            return true;
        }
  

        /// <summary>
        /// delegate definintion for any extra processing that may be added for each
        /// partitioned query.  Typical extra processsing would be for displaying records in a GUI
        /// without having to wait for all partitions to be read.
        /// </summary>
        /// <param name="resp"></param>
        public delegate void ExtraActionPerQuery1(Newtonsoft.Json.Linq.JArray resp);

        /// <summary>
        /// delegate definition for processing a bill
        /// </summary>
        /// <param name="bill"></param>
        public delegate void ExtraActionPerBill(Newtonsoft.Json.Linq.JToken  bill);

        /// <summary>
        /// get all the bills for a state/session combination.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="session"></param>
        /// <param name="cursor"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public async Task<Newtonsoft.Json.Linq.JArray> GetBillsAllAsync(string state, string session, string cursor, ExtraActionPerQuery1 f)
        {
            string Cursor = "";
            Newtonsoft.Json.Linq.JArray summaryAll = null;

            while (true)
            {
                //
                // get a set of bills up to the limit (usually 100)
                GraphQL.Common.Response.GraphQLResponse results = await GetBillsAsync(state, session, Cursor);
                Newtonsoft.Json.Linq.JArray summaryOne = results.Data.search_1.edges;
                int Count = GetCount(summaryOne);
                if (Count <= 0)
                {
                    //
                    // no more bills available
                    //
                    break;
                }
                //
                // bills available, so append into a summary of all bills in memory
                //
                AppendResults(ref summaryAll, summaryOne, Count);
                //
                // do any client specified extra work 
                // (like displaying results in a gui without having to wait for all the bills to
                // be read from Openstates.
                if (f != null)
                {
                    f(summaryOne);
                }
                //
                // get the cursor of the last bill just read
                //
                Cursor = GetNextCursor(summaryOne, Count);
                //
                // no more bills available
                //
                if (!IsCursorOK(Cursor))
                {
                    break;
                }
            }
            return summaryAll;
        }
        // --------------------------------------------------------------

            /// <summary>
            /// list n bills for a given jurisdiction (like a state) / Session / subject. 
            /// The classification of "bills" eliminates the less important "resolution"s.
            /// $state example is Texas
            /// $session example is 851
            /// Note: Texas subjects are pretty useless.  They dont describe the bills succintly,
            /// so search for a bill by a texas subject is not helpful.  Consequently, I read
            /// all the bills and filter by elements of the subject in my client application.
            /// </summary>
        private const string QsBillsBySubject = @"
query BillsByStateSession {
    search_1: bills(first: 100, after: ""$cursor"", jurisdiction: ""$state"", session:""$session"", classification:""bill"", subject:""$subject"") {
        edges {
            node {
                id
                identifier
                title
                classification
                updatedAt
                createdAt
                sponsorships {
                  name
                  entityType
                  primary
                  classification
                }
          votes {
      edges {
        node {
          counts {
            value
            option
          }
          votes {
            voterName
            voter {
              id
              contactDetails {
                value
                note
                type
              }
            }
            option
          }
        }
      }
    }
                legislativeSession {
                    identifier
                    jurisdiction
                    {
                        name
                    }
                }
                documents {
                    date
                    note
                    links {
                        url
                    }
                }
                sources {
                    url
                    note
                }
            }
            cursor
        }
    }
}";
        /// <summary>
        /// Get first 100 bills by subject from the cursor position specified.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="session"></param>
        /// <param name="subject"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetBillsBySubjectAsync(string state, string session, string subject, string cursor)
        {
            var graphQLRequest = new GraphQLRequest();

            string q = QsBillsBySubject;

            q = q.Replace("$state", state);
            q = q.Replace("$session", session);
            q = q.Replace("$subject", subject);
            if (cursor == null || cursor == "")
            {
                q = q.Replace(@"after:""$cursor"",", "");
            }
            else
            {
                q = q.Replace("$cursor", cursor);
            }

            graphQLRequest.Query = q;

            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// get all the bills by subject
        /// </summary>
        /// <param name="state"></param>
        /// <param name="session"></param>
        /// <param name="subject"></param>
        /// <param name="cursor"></param>
        /// <param name="f">method to call for each bill</param>
        /// <returns></returns>
        public async Task<Newtonsoft.Json.Linq.JArray> GetBillsBySubjectAllAsync(string state, string session, string subject, string cursor, ExtraActionPerQuery1 f)
        {
            string Cursor = "";
            // GraphQL.Common.Response.GraphQLResponse resultsAll = null;
            Newtonsoft.Json.Linq.JArray summaryAll = null;
            while (true)
            {
                //
                // get a set of bills up to the limit (usually 100)
                GraphQL.Common.Response.GraphQLResponse results = await GetBillsBySubjectAsync(state, session, subject,  Cursor);
                Newtonsoft.Json.Linq.JArray summaryOne = results.Data.search_1.edges;
                int Count = GetCount(summaryOne);
                if (Count <= 0)
                {
                    //
                    // no more bills available
                    //
                    break;
                }
                //
                // bills available, so append into a summary of all bills in memory
                //
                AppendResults(ref summaryAll, summaryOne, Count);
                //
                // do any client specified extra work 
                // (like displaying results in a gui without having to wait for all the bills to
                // be read from Openstates.
                if (f != null)
                {
                    f(summaryOne);
                }
                //
                // get the cursor of the last bill just read
                //
                Cursor = GetNextCursor(summaryOne, Count);
                //
                // no more bills available
                //
                if (!IsCursorOK(Cursor))
                {
                    break;
                }
            }
            return summaryAll;
        }

        /// <summary>
        /// This utility method helps find subjects bill results.
        /// The Texas bill subjects are not reasonable.
        /// Since each Texas subject is a set of long statements composed of multiple words,
        /// the subject parameter that you can pass to a query is pretty useless.
        /// So, my code has to resort to reading about all the bills looking for subject
        /// keywords in the bill subject strings.  This causes GraphQL to be alot less usefull.
        /// </summary>
        /// <param name="subjectstofind"></param>
        /// <param name="subjectsfound"></param>
        /// <returns></returns>
        public static bool IsBillSubjectFound(string subjectstofind, Newtonsoft.Json.Linq.JArray subjectsfound)
        {
            try
            {
                if (subjectsfound == null || subjectstofind == null ||
                    subjectsfound.Count <= 0 || subjectstofind == "")
                {
                    return true;
                }
                string[] SubjectsToFind = subjectstofind.Split();
                int n = subjectstofind.Length;
                foreach (Newtonsoft.Json.Linq.JToken tok in subjectsfound)
                {
                    if (tok != null)
                    {
                        string s = tok.ToString().ToLower();
                        for (int i = 0; i < n; i++)
                        {
                            string SubjectsToFindI = SubjectsToFind[i].ToLower();
                            if (SubjectsToFindI != null)
                            {
                                if (s.Contains(SubjectsToFindI))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {

            }
            return false;

        }

        /// <summary>
        /// extract session information into a form easier to deal with.
        /// Note: The startdates are particularly useful since this allows the user to 
        /// specify a date start or range that he is interested in and the application can retrieve
        /// just the bills that are of interest without the user having to know much about how texas
        /// defines a session.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="names"></param>
        /// <param name="ids"></param>
        /// <param name="startdates"></param>
        /// <returns></returns>
        private static bool ExtractSessionNames(GraphQL.Common.Response.GraphQLResponse  results, 
                                                 out List<string> names, out List<string> ids, out List<DateTime> startdates)
        {
            names = null;
            ids = null;
            startdates = null;
            try
            {
                int n = results.Data.jurisdiction.legislativeSessions.edges.Count;
                names = new List<string>(n);
                ids = new List<string>(n);
                startdates = new List<DateTime>(n);
                for (int i=0; i < n; i++)
                {
                    string NameI = results.Data.jurisdiction.legislativeSessions.edges[i].node.name.ToString();
                    string IdentifierI = results.Data.jurisdiction.legislativeSessions.edges[i].node.identifier.ToString();
                    string startDateStringI = results.Data.jurisdiction.legislativeSessions.edges[i].node.startDate.ToString();
                    DateTime startDateI;
                    bool OkDate = DateTime.TryParse(startDateStringI, out startDateI);
                    if (NameI != null && IdentifierI != null && OkDate)
                    {
                        names.Add(NameI);
                        ids.Add(IdentifierI);
                        startdates.Add(startDateI);
                    }
                }
                return true;
            }
            catch(System.Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// get all bills filtered by both subject and start dates.
        /// The start dates of the sessions helps keep us from making quite so many
        /// bill queries.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="startdate"></param>
        /// <param name="subjects">each space delimited token is searched for and all bill subjects that contain any token are returned</param>
        /// <param name="cursor"></param>
        /// <param name="f">action for each bill. null means no action.</param>
        public async Task<Newtonsoft.Json.Linq.JArray> GetBillsFilteredAsync(string state, DateTime startdate, DateTime enddate, string subjects, string cursor, ExtraActionPerBill f)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();

            Newtonsoft.Json.Linq.JArray AllBills = new Newtonsoft.Json.Linq.JArray();

            var results = await cli.GetStateLegesAsync(state);

            var aaa = results.Data;

            List<string> SessionNames;
            List<string> SessionIds;
            List<DateTime> SessionStarts;
            bool Ok = ExtractSessionNames(results, out SessionNames, out SessionIds, out SessionStarts);
            
            if (SessionNames != null && SessionIds != null)
            {
                int nn = SessionNames.Count;
                for (int ii = 0; ii < nn; ii++)
                {
                    string SessionI = SessionNames[ii];
                    string SessionIdI = SessionIds[ii];
                    if (startdate <= SessionStarts[ii])
                    {
                        Newtonsoft.Json.Linq.JArray Bills = await cli.GetBillsAllAsync(state, SessionIdI, "", null);
                        if (Bills != null)
                        {
                            int n = Bills.Count;
                            for (int i = 0; i < n; i++)
                            {
                                Newtonsoft.Json.Linq.JToken OneBill = Bills[i]["node"];

                                Newtonsoft.Json.Linq.JArray SubjectsFound = OneBill["subject"] as Newtonsoft.Json.Linq.JArray;
                                string updatedAt = OneBill["updatedAt"].ToString();
                                System.DateTime updatedAtDate;
                                bool OkUpdatedAtDate = System.DateTime.TryParse(updatedAt, out updatedAtDate);
                                if (OkUpdatedAtDate && updatedAtDate >= startdate &&
                                    updatedAtDate <= enddate &&
                                    OpenStateClientLib.OpenStateClient.IsBillSubjectFound(subjects, SubjectsFound))
                                {
                                    f?.Invoke(OneBill);
                                    AllBills.Add(OneBill);
                                }
                            }
                        }
                    }
                }
            }
            return AllBills;
        }



        // --------------------------------------------------------------


        private const string QsJurisdictions = @"query AllJurisdictions
                                    {
                                      jurisdictions {
                                        edges {
                                          node {
                                            id
                                            name
                                            legislativeSessions {
                                              edges {
                                                node {
                                                  identifier
                                                  name
                                                  classification
                                                  startDate
                                                  endDate
                                                }
                                              }
                                            }
                                          }
                                        }
                                      }
                                    }";

        /// <summary>
        /// get information about the jurisdictions (and the legislative sessions in particular)
        /// </summary>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetJurisdictionsAsync()
        {
            var graphQLRequest = new GraphQLRequest();

            string q = FixQueryQuotes(QsJurisdictions);

            graphQLRequest.Query = q;

            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// a bit of Linq magic to find the organization id for a particular state
        /// from the jurisdictions response data.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string FindJurisdictionId(Newtonsoft.Json.Linq.JArray array, string state)
        {
            Newtonsoft.Json.Linq.JObject jo2 = array.Children<Newtonsoft.Json.Linq.JObject>()
                .FirstOrDefault(o => o["node"]["name"].ToString() != null && o["node"]["name"].ToString() == state);

            string StateOrg = jo2["node"]["id"].ToString();
            return StateOrg;
        }



        private const string QsStateLeges = @"
query StateLeges {
  jurisdiction(name: ""$state"") {
    name
    url
    legislativeSessions {
      edges {
        node {
          name
          identifier
          startDate
        }
      }
    }
    organizations(classification: ""legislature"", first: 1)
    {
        edges {
            node {
                id
                name
                children(first: 40) {
                    edges {
                        node {
                            name
                            id
                        }
                    }
                }
            }
        }
    }
  }
}
";
        /// <summary>
        /// get legislatures for a given state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetStateLegesAsync(string state)
        {
            var graphQLRequest = new GraphQLRequest();

            string queryJurisdictionFixed = FixQueryQuotes(QsStateLeges);
            //
            // note: GraphQL "query variable" feature should be able to be used
            // but I havent got that working yet.  Maybe it's not implemented yet
            // for OpenStates.
            queryJurisdictionFixed = queryJurisdictionFixed.Replace("$state", state);
            graphQLRequest.Query = queryJurisdictionFixed;
            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// get people in all legislatures for a given state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<LegeSet> GetStatePeopleAsync(string state)
        {
            LegeSet inf = null;
            try
            {
                var graphQLRequest = new GraphQLRequest();

                string queryJurisdictionFixed = FixQueryQuotes(QsStateLeges);
                //
                // note: GraphQL "query variable" feature should be able to be used
                // but I havent got that working yet.  Maybe it's not implemented yet
                // for OpenStates.
                queryJurisdictionFixed = queryJurisdictionFixed.Replace("$state", state);
                graphQLRequest.Query = queryJurisdictionFixed;
                var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);

                int NumLege = response.Data.jurisdiction.organizations.edges.Count;
                inf = new LegeSet(state);
                for (int i = 0; i < NumLege; i++)
                {
                    string OneLegeName = response.Data.jurisdiction.organizations.edges[i].node.name.ToString();
                    string OneLegeId = response.Data.jurisdiction.organizations.edges[i].node.id.ToString();
                    int q = response.Data.jurisdiction.organizations.edges[i].node.children.edges.Count;
                    for (int k = 0; k < q; k++)
                    {
                        string OneName = response.Data.jurisdiction.organizations.edges[i].node.children.edges[k].node.name;
                        string OneID = response.Data.jurisdiction.organizations.edges[i].node.children.edges[k].node.id;

                        Newtonsoft.Json.Linq.JArray peeps = await GetPeopleByOrgIDAllAsync(OneID, null, null);
                        List<OnePerson> PeopleNames;
                        if (peeps != null)
                        {
                            int m = peeps.Count;
                            PeopleNames = new List<OnePerson>(m);
                            for (int j = 0; j < m; j++)
                            {
                                string NomJ = peeps[j]["node"]["name"].ToString();
                                OnePerson p = new OnePerson(NomJ);
                                PeopleNames.Add(p);
                            }
                        }
                        else
                        {
                            PeopleNames = new List<OnePerson>();
                        }
                        inf.AddLege(OneName, OneID, PeopleNames);
                    }
                }
            }
            catch(System.Exception)
            { }
            return inf;
        }



        public async Task<string> SummarizeStateLeges(string state)
        {        
            try
            {
                LegeSet lset = await GetStatePeopleAsync(state);
                string Summary = lset.SummarizeLeges();
                return Summary;
            }
            catch(System.Exception)
            {
            }
            return "";
        }



        private const string QsPeople = @"
        query PeopleQuery{             
            people(after:""$cursor"", first: 100)
            {
                search_1:
                edges {
                    node {
                        name
                           currentMemberships {
                                id
                           }
                    }
                    cursor
                }
            }
        }";

        /// <summary>
        /// get all the legislators
        /// </summary>
        /// <param name="cursor">starting point. null for the beginning</param>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetPeopleAsync(string cursor)
        {
            var graphQLRequest = new GraphQLRequest();
            string q = QsPeople;
            if (cursor == null || cursor == "")
            {
                q = q.Replace(@"after:""$cursor"",", "");
            }
            else
            {
                q = q.Replace("$cursor", cursor);
            }
            graphQLRequest.Query = QsPeople;
            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// get all the legislators
        /// </summary>
        /// <param name="cursor">for a starting position</param>
        /// <param name="f">and do something with each one</param>
        /// <returns></returns>
        public async Task<Newtonsoft.Json.Linq.JArray> GetPeopleAllAsync(string cursor, ExtraActionPerQuery1 f)
        {
            string Cursor = "";
            Newtonsoft.Json.Linq.JArray summaryAll = null;
            while (true)
            {
                //
                // get a set of people up to the limit (usually 100)
                GraphQL.Common.Response.GraphQLResponse results = await GetPeopleAsync(Cursor);

                Newtonsoft.Json.Linq.JArray summaryOne = results.Data.people.search_1;
                int Count = GetCount(summaryOne);
                if (Count <= 0)
                {
                    //
                    // no more people available
                    //
                    break;
                }
                //
                // people available, so append into a summary of all bills in memory
                //
                AppendResults(ref summaryAll, summaryOne, Count);
                //
                // do any client specified extra work 
                // (like displaying results in a gui without having to wait for all the bills to
                // be read from Openstates.
                f(summaryOne);
                //
                // get the cursor of the last bill just read
                //
                Cursor = GetNextCursor(summaryOne, Count);
                //
                // no more bills available
                //
                if (!IsCursorOK(Cursor))
                {
                    break;
                }
            }
            return summaryAll;
        }



        /// <summary>
        /// example id = ocd-organization/ddf820b5-5246-46b3-a807-99b5914ad39f
        /// </summary>
        private const string QsPeopleByOrgID = @"
        query PeopleByOrgID{
            people(memberOf:""$id"", after:""$cursor"", first: 10) {
                edges {
                    node {
                        name
                        id
                        party: currentMemberships(classification:""party"")
                        {
                            organization {
                                name
                            }
                        }
                        links {
                            url
                        }
                        sources {
                            url
                        }
                        chamber: currentMemberships(classification:[""upper"", ""lower""])
                        {
                            post {
                                label
                            }
                            organization {
                                name
                                classification
                                parent {
                                    name
                                }
                            }
                        }
                    }
                    cursor
                }
            }
        }";

        /// <summary>
        /// get all the people for a given organization (like a legislature)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetPeopleByOrgIDAsync(string id, string cursor)
        {
            var graphQLRequest = new GraphQLRequest();
            string q = QsPeopleByOrgID.Replace("$id", id);
            if (cursor == null || cursor == "")
            {
                q = q.Replace(@"after:""$cursor"",", "");
            }
            else
            {
                q = q.Replace("$cursor", cursor);
            }
            graphQLRequest.Query = q;
            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// get all the legislators for a legislature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cursor"></param>
        /// <param name="f">and do something for each one</param>
        /// <returns></returns>
        public async Task<Newtonsoft.Json.Linq.JArray> GetPeopleByOrgIDAllAsync(string id, string cursor, ExtraActionPerQuery1 f)
        {
            string Cursor = "";
            Newtonsoft.Json.Linq.JArray summaryAll = null;
            while (true)
            {
                //
                // get a set of people up to the limit (usually 100)
                GraphQL.Common.Response.GraphQLResponse results = await GetPeopleByOrgIDAsync(id, Cursor);

                Newtonsoft.Json.Linq.JArray summaryOne = results.Data.people.edges;
                int Count = GetCount(summaryOne);
                if (Count <= 0)
                {
                    //
                    // no more people available
                    //
                    break;
                }
                //
                // people available, so append into a summary of all bills in memory
                //
                AppendResults(ref summaryAll, summaryOne, Count);
                //
                // do any client specified extra work 
                // (like displaying results in a gui without having to wait for all the people to
                // be read from Openstates.
                if (f != null)
                {
                    f(summaryOne);
                }
                //
                // get the cursor of the last bill just read
                //
                Cursor = GetNextCursor(summaryOne, Count);
                //
                // no more bills available
                //
                if (!IsCursorOK(Cursor))
                {
                    break;
                }
            }
            return summaryAll;
        }


        /// <summary>
        /// example name = Lydia
        /// </summary>
        private const string QsPersonByName = @"
        query PersonByName{
            people(first: 100, name: ""$name"") {
                edges {
                    node {
                        name
                        party: currentMemberships(classification:""party"")
                        {
                            organization {
                                name
                                id
                            }
                        }
                        links {
                            url
                        }
                        sources {
                            url
                        }
                        chamber: currentMemberships(classification:[""upper"", ""lower""])
                        {
                            post {
                                label
                            }
                            organization {
                                name
                                id
                                classification
                                parent {
                                    name
                                }
                            }
                        }
                    }
                    cursor
                }
            }
        }";

        /// <summary>
        /// get information about some people by their name
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public async Task<GraphQL.Common.Response.GraphQLResponse> GetPersonByNameAsync(string nom)
        {
            var graphQLRequest = new GraphQLRequest();
            string q = QsPersonByName.Replace("$name", nom);
            graphQLRequest.Query = q;
            var response = await this.GraphQLClient.GetAsync(graphQLRequest).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// get all people who match a search by name
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public async Task<Newtonsoft.Json.Linq.JArray> GetPersonByNameAllAsync(string cursor, ExtraActionPerQuery1 f)
        {
            string Cursor = "";
            Newtonsoft.Json.Linq.JArray summaryAll = null;
            while (true)
            {
                //
                // get a set of people up to the limit (usually 100)
                GraphQL.Common.Response.GraphQLResponse results = await GetPersonByNameAsync(Cursor);

                Newtonsoft.Json.Linq.JArray summaryOne = results.Data.people.edges;
                int Count = GetCount(summaryOne);
                if (Count <= 0)
                {
                    //
                    // no more people available
                    //
                    break;
                }
                //
                // people available, so append into a summary of all bills in memory
                //
                AppendResults(ref summaryAll, summaryOne, Count);
                //
                // do any client specified extra work 
                // (like displaying results in a gui without having to wait for all items to
                // be read from OpenStates.
                f(summaryOne);
                //
                // get the cursor of the last bill just read
                //
                Cursor = GetNextCursor(summaryOne, Count);
                //
                // no more bills available
                //
                if (!IsCursorOK(Cursor))
                {
                    break;
                }
            }
            return summaryAll;
        }


        public static string DisplayOneBill(Newtonsoft.Json.Linq.JToken bill, string curstate, string curvoter)
        {
            string Msg = "";
            string CurVoterL;
            if (curvoter != null)
            {
                CurVoterL = curvoter.ToLower();
            }
            else
            {
                CurVoterL = "";
            }
            try
            {
                
                if (curstate == null)
                {
                    return Msg;
                }
                if (bill == null)
                {
                    return "bill is null.";
                }
                else
                {
                    // await context.PostAsync(bill.ToString());
                }
                Msg += (curstate + " Bill " + bill["identifier"].ToString());
                Msg += (" (" + bill["title"].ToString() + ")\n");


                // Msg += ("updatedAt = " + bill["updatedAt"] + "\n");
                // Msg += ("subject = " + bill["subject"].ToString() + "\n");

                Newtonsoft.Json.Linq.JArray Votes = bill["votes"]["edges"] as Newtonsoft.Json.Linq.JArray;
                int VoteCount = 0;
                if (Votes != null)
                {
                    VoteCount = Votes.Count;
                }
                if (VoteCount > 0)
                {
                    //Msg += ("votes = " + Votes.ToString() + "\n");
                    for (int i = 0; i < VoteCount; i++)
                    {
                        Msg += $"Vote # {i}\n"; 
                        var Votes2 = bill["votes"]["edges"][i];
                        // Msg += Votes2.Type.ToString() + Votes2.ToString();
                        Newtonsoft.Json.Linq.JArray Votes3 = Votes2["node"]["votes"] as Newtonsoft.Json.Linq.JArray;
                        // Msg += Votes3.Type.ToString() + Votes3.ToString();
                        int m = Votes3.Count;
                        for (int j = 0; j < m; j++)
                        {
                            string VoterName = Votes3[j]["voterName"].ToString();
                            if (curvoter == null || curvoter == "" || VoterName.ToLower().Contains(CurVoterL))
                            { 
                                string Option = Votes3[j]["option"].ToString();
                                string Msg2 = $" {VoterName}, Vote = {Option}\n";
                                Msg += Msg2;
                            }
                        }

                    }
                    
                }
                Newtonsoft.Json.Linq.JArray Documents = bill["documents"] as Newtonsoft.Json.Linq.JArray;

                int DocCount = 0;
                if (Documents != null)
                {
                    DocCount = Documents.Count;
                }

                if (DocCount > 0)
                {
                    //  bill["documents"][0]["links"][0]["url"]
                    // Msg += ("documents = " + Documents.ToString() + "\n");
                    for (int j = 0; j < DocCount; j++)
                    {
                        Newtonsoft.Json.Linq.JArray links = Documents[j]["links"] as Newtonsoft.Json.Linq.JArray;
                        int LinkCount = links.Count;
                        for (int k = 0; k < LinkCount; k++)
                        {
                            string Url = links[k]["url"].ToString();
                            Msg += ("document: " + Url + "\n");
                        }

                    }
                }

  
                return Msg;               
            }
            catch (System.Exception xyzzy)
            {
                return xyzzy.Message;
            }
        }

    }
}
