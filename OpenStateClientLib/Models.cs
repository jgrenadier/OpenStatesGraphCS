using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStateClientLib
{
    /// <summary>
    /// a single voter (usually a legislator or some kind)
    /// </summary>
    public class OnePerson
    {
        /// <summary>
        /// constructor
        /// </summary>
        public OnePerson()
        {

        }
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="nom"></param>
        public OnePerson(string nom)
        {
            Name = nom;
        }
        /// <summary>
        /// name of voter
        /// Note: I have seen that the names used in the list of votes
        /// for a particular bill may not match exactly the names listed
        /// as members of a legislative body.  So, my gui is encourage the
        /// user to enter a case insensitive substring that is sure to at 
        /// least be in the either name representation.
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// a legislative body
    /// Note: This class is especially useful to make legislative info
    /// available in methods that use the .Net Asynchronous Programming model
    /// and therefore require that all the return info be assembled in to a single
    /// class instance.
    /// </summary>
    public class OneLege
    {
        /// <summary>
        /// constructor with defaults
        /// Note: there are more elegant ways of doing this with 
        /// recent versions of C#.
        /// </summary>
        public OneLege()
        {
            SetItem("", "", null);
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="id"></param>
        /// <param name="people"></param>
        public OneLege(string nom, string id, List<OnePerson> people)
        {
            SetItem(nom, id, people);
        }
        /// <summary>
        /// name of the legislative body
        /// </summary>
        public string LegeName;
        /// <summary>
        /// the unique id of this legislative body
        /// </summary>
        public string LegeId;
        /// <summary>
        /// the people in this body
        /// </summary>
        public List<OnePerson> People;

        /// <summary>
        /// sets most commonly used properties
        /// </summary>
        /// <param name="nom">name</param>
        /// <param name="id">unique id</param>
        /// <param name="people">people in the legislative body</param>
        private void SetItem(string nom, string id, List<OnePerson> people)
        {
            LegeName = nom;
            LegeId = id;
            People = people;
        }

    }
    /// <summary>
    /// a set of legislative bodies.
    /// In Texas, a "legislature" is bicameral.  
    /// There is a House and a Senate.  I use the concept of a 
    /// LegeSet to mean a Legislature.  The OpenStates data model is 
    /// a bit more flexible so that the representation of a legislature
    /// can be a nested tree.  Maybe they use this committees and such.
    /// However, for simplicity, I just have one level of elements under
    /// a LegeSet.  This should handle both Unicameral and Bicameral and even
    /// n-cameral legislatures and be easier for the GUI to display.
    /// </summary>
    public class LegeSet
    {
        /// <summary>
        /// constructor
        /// </summary>
        public LegeSet(string state)
        {
            State = state;
            Reset();
        }
        
        /// <summary>
        /// clean out the legislative body list
        /// </summary>
        private void Reset()
        {
            Leges = new List<OneLege>();
        }

        /// <summary>
        /// The legislative body list
        /// </summary>
        public List<OneLege> Leges;

        /// <summary>
        /// the state name
        /// </summary>
        public string State = "";

        /// <summary>
        /// add a legislative body
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="id"></param>
        /// <param name="people"></param>
        public void AddLege(string nom, string id, List<OnePerson> people)
        {
            if (Leges == null)
            {
                Leges = new List<OneLege>(2);
            }
            Leges.Add(new OneLege(nom, id, people));
        }

        /// <summary>
        /// utility to make sure strings are non-null.
        /// Note: There are more elegant ways of doing this with the
        /// more recent versions of C#.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string NonNull(string s)
        {
            if (s == null)
            {
                return "";
            }
            return s;
        }

        /// <summary>
        /// find a legislator by name.
        /// Note: the name is a case insensitive substring
        /// to make finding the legislator more likely.
        /// Note: This method lets you know if there are more
        /// than one match.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int FindLegislator(string nom, out string msg)
        {
            string nom_lower = nom.ToLower();
            string NomFound = "";
            msg = "";
            int Total = 0;
            if (nom == null || nom == "")
            {
                msg = "Invalid legislator name";
                return Total;
            }
            try
            {
                if (this.Leges == null || Leges.Count <= 0)
                {
                    msg = "Legislative info not available.";
                    return 0;
                }
                int n = this.Leges.Count;
                for (int i = 0; i < n; i++)
                {
                    OneLege one = this.Leges[i];
                    if (one != null && one.People != null)
                    {
                        int m = one.People.Count;
                        for (int j = 0; j < m; j++)
                        {
                            string s = one.People[j].Name.ToLower();
                            if (s != null && s.Contains(nom_lower))
                            {
                                Total++;
                                NomFound = one.People[j].Name;
                            }
                        }
                    }
                }
                if (Total <= 0)
                {
                    msg = "No legislator with the name " + nom;
                }
                else if (Total == 1)
                {
                    msg = $"{NomFound} found.";
                }
                else
                {
                    msg = $"There are {Total} legislators with names containing \"{nom}\".";
                }
            }
            catch (System.Exception)
            {

            }
            return Total;
        }

        /// <summary>
        /// summarize the legislature into a text string.
        /// </summary>
        /// <returns></returns>
        public string SummarizeLeges()
        {
            string Msg = "";
            try
            {
                if (Leges != null)
                {
                    int n = Leges.Count;
                    for (int i = 0; i < n; i++)
                    {
                        OneLege leg = Leges[i];
                        string LegeNom = leg.LegeName;
                        if (LegeNom != null)
                        {
                            string s = $"----- {this.State} {LegeNom} -----\n";
                            Msg += s;
                            if (leg.People != null)
                            {
                                int m = leg.People.Count;
                                for (int j = 0; j < m; j++)
                                {
                                    OnePerson p = leg.People[j];
                                    if (p != null && p.Name != null)
                                    {
                                        string PersonName = p.Name;
                                        string s2 = $"  {PersonName}\n";
                                        Msg += s2;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {

            }
            return Msg;
        }

    }
  
}
