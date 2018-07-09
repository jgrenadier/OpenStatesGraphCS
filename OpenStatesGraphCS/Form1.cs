using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenStatesGraphCS
{
    public partial class OpenStatesGraphTestFrm : Form
    {
        public OpenStatesGraphTestFrm()
        {
            InitializeComponent();
        }

        //private void ExtraBillsAction(GraphQL.Common.Response.GraphQLResponse resp)
        //{
        //    string s = resp.Data.ToString();
        //    rtbResults.AppendText(s);
        //    rtbResults.AppendText("++++++++++++++++++++++++");
        //}

        private void ExtraGuiAction(Newtonsoft.Json.Linq.JArray resp)
        {
            string s = resp.ToString();
            rtbResults.AppendText(s);
            rtbResults.AppendText("++++++++++++++++++++++++");
        }

        private void ExtraGuiActionBill(Newtonsoft.Json.Linq.JToken bill)
        {
            //this.rtbBills.AppendText("identifier = " + bill["identifier"].ToString() + "\n");
            //this.rtbBills.AppendText("title = " + bill["title"].ToString() + "\n");

            //this.rtbBills.AppendText("updatedAt = " + bill["updatedAt"] + "\n");
            //this.rtbBills.AppendText("subject = " + bill["subject"].ToString() + "\n");

            //Newtonsoft.Json.Linq.JArray Votes = bill["votes"]["edges"] as Newtonsoft.Json.Linq.JArray;
            //int VoteCount = 0;
            //if (Votes != null)
            //{
            //    VoteCount = Votes.Count;
            //}
            //if (VoteCount > 0)
            //{
            //    this.rtbBills.AppendText("votes = " + Votes.ToString() + "\n");
            //}
            //Newtonsoft.Json.Linq.JArray Documents = bill["documents"] as Newtonsoft.Json.Linq.JArray;
            //int DocCount = 0;
            //if (Documents != null)
            //{
            //    DocCount = Documents.Count;
            //}
            //if (DocCount > 0)   
            //{
            //    //  bill["documents"][0]["links"][0]["url"]
            //    this.rtbBills.AppendText("documents = " + Documents.ToString() + "\n");
            //    for (int j = 0; j < DocCount; j++)
            //    {
            //        Newtonsoft.Json.Linq.JArray links = Documents[j]["links"] as Newtonsoft.Json.Linq.JArray;
            //        int LinkCount = links.Count;
            //        for (int k=0; k < LinkCount; k++)
            //        {
            //            string Url = links[k]["url"].ToString();
            //            this.rtbBills.AppendText("url = " + Url + "\n");
            //        }

            //    }
            //}
            string Msg = OpenStateClientLib.OpenStateClient.DisplayOneBill(bill, false, "Texas", "");
            rtbBills.AppendText(Msg);
        }

        private async void btnTestBills_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            // bool keepgoing = true;
            rtbResults.Clear();

            string Cursor = "";

            var ResultsC = await cli.GetBillsAllAsync("Texas", "851", Cursor, ExtraGuiAction);

        }

        private async void btnTestJurisdictions_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            var resultsJ = await cli.GetJurisdictionsAsync();
            string s = resultsJ.Data.ToString();
            // MessageBox.Show(s, "Jurisdictions");
            rtbResults.Clear();
            rtbResults.AppendText(s);
        }


    
        private async void btnTestJurisLeges_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            var resultsJL = await cli.GetStateLegesAsync("Texas");
            string s = resultsJL.Data.ToString();
            //rtbResults.Clear();
            //rtbResults.AppendText(s);

            OpenStateClientLib.LegeSet lset =  await cli.GetStatePeopleAsync("Texas");
            string ss = lset.SummarizeLeges();
            rtbResults.Clear();
            rtbResults.AppendText(ss);

        }

        private async void btnTestPeople_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
                      
            
            string Cursor = "";
 
            var resultsP = await cli.GetPeopleAllAsync(Cursor, ExtraGuiAction);

        }

   

        private async void btnTestPersonByName_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            // var resultsP = await cli.QueryPersonByNameAsync("Kirk Watson");
            var resultsP = await cli.GetPersonByNameAsync("Workman");
            string s = resultsP.Data.ToString();
            rtbResults.Clear();
            rtbResults.AppendText(s);
        }

        private async void btnTestPeopleByOrgID_Click(object sender, EventArgs e)
        {
            rtbResults.Clear();
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();

            string Cursor = "";
 
            // var resultsP = await cli.QueryPeopleByOrgIDAllAsync("ocd-organization/ddf820b5-5246-46b3-a807-99b5914ad39f", Cursor, ExtraGuiAction);
            var resultsP = await cli.GetPeopleByOrgIDAllAsync("ocd-organization/cabf1716-c572-406a-bfdd-1917c11ac629", Cursor, ExtraGuiAction);
        }

        private async void btnTestBillsBySubject_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            rtbResults.Clear();
            string Cursor = "";
  
            var resultsB = await cli.GetBillsBySubjectAllAsync("Texas", "851", "Courts--Civil Procedure (I0135)", Cursor, ExtraGuiAction);


        }

   
        private void SetPeopleListBox(ListBox l, Newtonsoft.Json.Linq.JArray toks)
        {
          
            List<string> noms = new List<string>(toks.Count);
            DataTable dt = new DataTable("hmmmm");
            dt.Columns.Add("name");
            dt.Columns.Add("index");
            dt.Columns.Add("id");
            int index = 0;
            foreach (var each in toks)
            {
                Newtonsoft.Json.Linq.JToken tok = each["node"];

                DataRow dr = dt.NewRow();
                dr["name"] = each["node"]["name"].ToString();
                dr["index"] = index;
                dr["id"] = each["node"]["id"].ToString();
                index++;
                dt.Rows.Add(dr);
            }
            DataView dv = new DataView(dt);
            dv.Sort = "name";
            l.DataSource = dv;
            l.DisplayMember = "name";


        }

        private async void btnShowTexasSenateLegislators_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            var resultsJ = await cli.GetJurisdictionsAsync();

            Newtonsoft.Json.Linq.JArray array = resultsJ.Data.jurisdictions.edges;

            string TexasOrgName = cli.FindJurisdictionId(array, "Texas");
            var resultsT = await cli.GetStateLegesAsync("Texas");

            string TexasSenateLegeID = "ocd-organization/cabf1716-c572-406a-bfdd-1917c11ac629";

            var ResultsP = await cli.GetPeopleByOrgIDAllAsync(TexasSenateLegeID, "", ExtraGuiAction);

            SetPeopleListBox(lbSenateLegislators, ResultsP);

            
        }

        private async void btnShowTexasHouseLegislators_Click(object sender, EventArgs e)
        {
     

            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
            var resultsJ = await cli.GetJurisdictionsAsync();

            Newtonsoft.Json.Linq.JArray array = resultsJ.Data.jurisdictions.edges;

            string TexasOrgName = cli.FindJurisdictionId(array, "Texas");
            var resultsT = await cli.GetStateLegesAsync("Texas");

            string TexasHouseLegeID = "ocd-organization/d6189dbb-417e-429e-ae4b-2ee6747eddc0";

            var ResultsP = await cli.GetPeopleByOrgIDAllAsync(TexasHouseLegeID, "", ExtraGuiAction);

            SetPeopleListBox(lbHouseLegislators, ResultsP);


        }

        private void SetSessionsListBox(ListBox l, GraphQL.Common.Response.GraphQLResponse  resp)
        {
            // [0].node.identifier.ToString();

            Newtonsoft.Json.Linq.JArray arr = resp.Data.jurisdiction.legislativeSessions.edges;

            List<string> noms = new List<string>(arr.Count);
            DataTable dt = new DataTable("hmmmm");
            dt.Columns.Add("name");
            dt.Columns.Add("index");
            dt.Columns.Add("id");
            int index = 0;
            foreach (var each in arr)
            {
                Newtonsoft.Json.Linq.JToken tok = each["node"];

                DataRow dr = dt.NewRow();
                string Id = each["node"]["identifier"].ToString();
                dr["id"] = Id;
                dr["name"] = "[" + Id + "]" +each["node"]["name"].ToString();
                dr["index"] = index;
                
                index++;
                dt.Rows.Add(dr);
            }
            DataView dv = new DataView(dt);
            dv.Sort = "name DESC";
            l.DataSource = dv;
            l.DisplayMember = "name";

           

        }


        private async void btnShowTexasSessions_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();
           
            var results = await cli.GetStateLegesAsync("Texas");

            var aaa = results.Data;

            string Name0 = results.Data.jurisdiction.legislativeSessions.edges[0].node.name.ToString();

            string Identifier0 = results.Data.jurisdiction.legislativeSessions.edges[0].node.identifier.ToString();

            SetSessionsListBox(this.lbTexasSessions, results);
        }

        private DataTable GetSessionTable()
        {
            if (this.lbTexasSessions.DataSource == null)
            {
                return null;
            }
            DataView dv = this.lbTexasSessions.DataSource as DataView;
            DataTable dt = dv.ToTable();
            return dt;
        }

 

        private async void btnShowBills_Click(object sender, EventArgs e)
        {
            OpenStateClientLib.OpenStateClient cli;
            cli = new OpenStateClientLib.OpenStateClient();

            string Session = "";
            if (this.lbTexasSessions.DataSource == null)
            {
               
                var results = await cli.GetStateLegesAsync("Texas");

                var aaa = results.Data;

                string Name0 = results.Data.jurisdiction.legislativeSessions.edges[0].node.name.ToString();

                string Identifier0 = results.Data.jurisdiction.legislativeSessions.edges[0].node.identifier.ToString();
                Session = Name0;

                SetSessionsListBox(this.lbTexasSessions, results);
            }
            else
            {
                Session = ((System.Data.DataRowView)this.lbTexasSessions.SelectedValue)["id"].ToString();
            }
 

            System.DateTime SinceDate = this.dateTimePickerBills.Value;
            string Subjects = this.txtSubject.Text;

            await cli.GetBillsFilteredAsync("Texas", SinceDate, System.DateTime.MaxValue, Subjects, null, ExtraGuiActionBill);

  
        }

        // 
    }
}
