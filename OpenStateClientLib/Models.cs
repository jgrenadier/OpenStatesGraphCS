using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStateClientLib
{

    public class OnePerson
    {
        public OnePerson()
        {

        }
        public OnePerson(string nom)
        {
            Name = nom;
        }
        public string Name;
    }
    public class OneLege
    {
        public OneLege()
        {
            SetItem("", "", null);
        }

        public OneLege(string nom, string id, List<OnePerson> people)
        {
            SetItem(nom, id, people);
        }
        public string LegeName;
        public string LegeId;
        public List<OnePerson> People;

        public void SetItem(string nom, string id, List<OnePerson> people)
        {
            LegeName = nom;
            LegeId = id;
            People = people;
        }

    }
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


        private void Reset()
        {
            Leges = new List<OneLege>();
        }

        public List<OneLege> Leges;

        public string State = "";

        public void AddLege(string nom, string id, List<OnePerson> people)
        {
            if (Leges == null)
            {
                Leges = new List<OneLege>(2);
            }
            Leges.Add(new OneLege(nom, id, people));
        }

        private static string NonNull(string s)
        {
            if (s == null)
            {
                return "";
            }
            return s;
        }

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


    // -----------------

    public class data
    {
        people people
        {
            get;
            set;
        }
    }
    public class people
    {
        edges edges
        {
            get;
            set;
        }
    }
    public class edges
    {
        node[] node
        {
            get;
            set;
        }

    }
    public class node
    {
        string  name
        {
            get;
            set;
        }
    }
}
