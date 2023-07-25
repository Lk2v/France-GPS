using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Linq;

namespace FranceGPS.src.Graphe
{
    public class Graphe
    {
        static HashSet<Ville> _listeVilles = new HashSet<Ville>();
        static HashSet<Route> _listeRoutes = new HashSet<Route>();

        public static HashSet<Ville> ListeVilles
        {
            get => _listeVilles;
        }

        public static HashSet<Ville> ListeVillesClone
        {
            get
            {
                HashSet<Ville> vl = new HashSet<Ville>();
                foreach (Ville v in _listeVilles)
                {
                    vl.Add(v.Clone());
                }
                return vl;
            }
        }


        public static HashSet<Route> ListeRoutes
        {
            get => _listeRoutes;
        }

        private static HashSet<Route> lstArretesT = new HashSet<Route>();

        private static int[,] matriceadjD = new int[0, 0];
        private static int[,] matriceadjT = new int[0, 0];

        public static void Initialiser()
        {
            Console.WriteLine("Chargement des données...");
            _listeVilles = Donnees.ChargerVilles();
            _listeRoutes = Donnees.ChargerRoutes(_listeVilles);
            Console.WriteLine("Donnée chargée !");

            matriceadjD = new int[_listeVilles.Count, _listeVilles.Count];
            matriceadjT = new int[_listeVilles.Count, _listeVilles.Count];

            CreationMatriceP();
        }

        public static int[,] MatriceAdjD
        {
            get { return matriceadjD; }
            set { matriceadjD = value; }
        }

        public static int[,] MatriceAdjT
        {
            get { return matriceadjT; }
            set { matriceadjT = value; }
        }

        public static int[,] CreationMatriceP()
        {
            string[] Ville = new string[_listeVilles.Count];
            int k = 0;
            foreach (Ville v in _listeVilles)
            {
                Ville[k] = v.Nom;
                k++;
            }
            for (int i = 0; i < _listeVilles.Count; i++)
            {
                for (int j = 0; j < _listeVilles.Count; j++)
                {
                    if (i == j) matriceadjD[i, j] = 0;
                    else
                    {
                        foreach (Route c in _listeRoutes)
                        {
                            if (c.V1.Nom == Ville[i] || c.V2.Nom == Ville[i])
                            {
                                if (c.V1.Nom == Ville[j] || c.V2.Nom == Ville[j])
                                {
                                    matriceadjD[i, j] = c.Distance;
                                    matriceadjT[i, j] = c.DureeParcours;
                                }
                            }
                        }
                    }

                }
            }
            return matriceadjD;
        }
        public static HashSet<Ville> Dijkstra(Ville source, MethodeItineraire methode)
        {
            HashSet<Ville> listeSommetsDistance = ListeVilles;

            Console.WriteLine(methode);

            int[,] matAdj;
            if (methode == MethodeItineraire.Temps)
            {

                matAdj = MatriceAdjT;
            }
            else
            {
                matAdj = MatriceAdjD;
            }

            foreach (Ville v in _listeVilles)
            {
                if (v.Nom == source.Nom) source.Num = v.Num;
            }
            int[] distances = new int[_listeVilles.Count]; // Tableau pour stocker les distances minimales
            bool[] tab = new bool[_listeVilles.Count]; // Tableau pour stocker les sommets visités
            int[,] mat = new int[_listeVilles.Count, _listeVilles.Count];
            string[,] matrice = new string[_listeVilles.Count, _listeVilles.Count];
            // Initialiser les distances à l'infini sauf pour la source

            for (int i = 0; i < _listeVilles.Count; i++)
            {
                distances[i] = int.MaxValue;
                tab[i] = false;
            }
            distances[source.Num] = 0; // La distance de la source à elle-même est de 0

            for (int count = 0; count < _listeVilles.Count - 1; count++)
            {
                int u = MinDistance(distances, tab);
                tab[u] = true;
                for (int v = 0; v < _listeVilles.Count; v++)
                {
                    if (!tab[v] && matAdj[u, v] != 0 && distances[u] != int.MaxValue && distances[u] + matAdj[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + matAdj[u, v];
                    }
                }
            }

            for (int i = 0; i < listeSommetsDistance.Count; i++)
            {
                if (methode == MethodeItineraire.Distance)
                {
                    listeSommetsDistance.ElementAt(i).Distance = distances[i];
                }
                else
                {
                    listeSommetsDistance.ElementAt(i).Temps = distances[i];
                }

            }

            return listeSommetsDistance;
        }
        private static int MinDistance(int[] distances, bool[] shortestPathSet)
        {
            int min = int.MaxValue;
            int minIndex = 0;
            for (int v = 0; v < _listeVilles.Count; v++)
            {
                if (!shortestPathSet[v] && distances[v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }

        public static Itineraire Dijkstra2(Ville source, MethodeItineraire methode, Ville arrive)
        {
            int[,] matAdj;
            Console.WriteLine(methode);
            if (methode == MethodeItineraire.Temps)
            {

                matAdj = MatriceAdjT;
            }
            else
            {
                matAdj = MatriceAdjD;
            }

            #region Initialisation
            foreach (Ville v in _listeVilles)
            {
                if (source.Nom == v.Nom) source = v;
                if (arrive.Nom == v.Nom) arrive = v;
            }
            List<Ville> lst = new List<Ville>();
            for (int i = 0; i < _listeVilles.Count; i++)
            {
                Ville s = new Ville(_listeVilles.ElementAt(i).Nom, i, null);
                lst.Add(s);
            }
            #endregion

            int[] distances = new int[_listeVilles.Count]; // Tableau pour stocker les distances minimales
            bool[] tab = new bool[_listeVilles.Count]; // Tableau pour stocker les sommets visités
            int[,] mat = new int[_listeVilles.Count, _listeVilles.Count];

            string[,] matrice = new string[_listeVilles.Count, _listeVilles.Count];
            // Initialiser les distances à l'infini sauf pour la source
            for (int i = 0; i < _listeVilles.Count; i++)
            {
                distances[i] = int.MaxValue;
                tab[i] = false;
            }
            distances[source.Num] = 0; // La distance de la source à elle-même est de 0
            for (int count = 0; count < _listeVilles.Count - 1; count++)
            {
                int u = MinDistance(distances, tab);
                tab[u] = true;
                for (int v = 0; v < _listeVilles.Count; v++)
                {
                    if (!tab[v] && matAdj[u, v] != 0 && distances[u] != int.MaxValue && distances[u] + matAdj[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + matAdj[u, v];
                        lst[v].Pred = lst[u];
                    }
                }
            }

            HashSet<Ville> tr = new HashSet<Ville>();
            int dep = source.Num;
            int f = arrive.Num;
            tr.Add(arrive);

            while (lst[f].Pred!.Num != dep)
            {
                tr.Add(lst[f].Pred!);
                f = lst[f].Pred!.Num;
            }
            tr.Add(source);

            HashSet<Route> trajet = new HashSet<Route>();
            for (int i = 0; i < tr.Count - 1; i++)
            {
                for (int w = 0; w < _listeRoutes.Count; w++)
                {
                    if (_listeRoutes.ElementAt(w).Contient(tr.ElementAt(i), tr.ElementAt(i + 1)))
                    {
                        trajet.Add(_listeRoutes.ElementAt(w));
                        break;
                    }
                }
            }
            return new Itineraire
            {
                Chemin = trajet
            };
        }

        public static HashSet<Ville> Adjacents_Ville(Ville s)
        {
            HashSet<Ville> liste = new HashSet<Ville>();
            foreach (Route r in _listeRoutes)
            {
                if (r.V1.Nom == s.Nom) liste.Add(r.V2);
            }
            return liste;
        }

        public static (bool, Route?) EstAdjacents_Ville(Ville s1, Ville s2)
        {
            Route? rt = null;
            for(int i = 0; i < ListeRoutes.Count; i++)
            {
                if(ListeRoutes.ElementAt(i).Contient(s1,s2))
                {
                    rt = (ListeRoutes.ElementAt(i));
                    break;
                } 
            }

            return (rt != null, rt);
        }

        public static (bool, HashSet<Route>) EstConnexe(Ville s1, Ville s2, Ville s3, Ville s4)
        {
            HashSet<Ville> a = Adjacents_Ville(s1);
            HashSet<Ville> visite = new HashSet<Ville>();
            visite.Add(s1);
            foreach (Ville v in a)
            {
                if (v.Nom == s1.Nom || v.Nom == s2.Nom || v.Nom == s3.Nom || v.Nom == s4.Nom)
                {
                    if(!visite.Contains(v))visite.Add(v);
                }
            }
            HashSet<Ville> b = Adjacents_Ville(s2);
            foreach (Ville v in b)
            {
                if (v.Nom == s1.Nom || v.Nom == s2.Nom || v.Nom == s3.Nom || v.Nom == s4.Nom)
                {
                    if (!visite.Contains(v)) visite.Add(v);
                }
            }
            HashSet<Ville> c = Adjacents_Ville(s3);
            foreach (Ville v in c)
            {

                if (v.Nom == s1.Nom || v.Nom == s2.Nom || v.Nom == s3.Nom || v.Nom == s4.Nom)
                {
                    if (!visite.Contains(v)) visite.Add(v);
                }

            }
            HashSet<Ville> d = Adjacents_Ville(s4);
            foreach (Ville v in d)
            {
                if (v.Nom == s1.Nom || v.Nom == s2.Nom || v.Nom == s3.Nom || v.Nom == s4.Nom)
                {
                    if (!visite.Contains(v)) visite.Add(v);
                }
            }

            HashSet<Route> trajet = new HashSet<Route>();
            for (int i = 0; i < visite.Count - 1; i++)
            {
                for (int w = 0; w < _listeRoutes.Count; w++)
                {
                    for (int j = i + 1; j < visite.Count; j++)
                    {
                        if (_listeRoutes.ElementAt(w).Contient(visite.ElementAt(i), visite.ElementAt(j)))
                        {

                            trajet.Add(_listeRoutes.ElementAt(w));
                        }
                    }

                }
            }

            for (int i = 0; i < trajet.Count - 1; i++)
            {
                for (int j = i + 1; j < trajet.Count; j++)
                {
                    if (trajet.ElementAt(i).V1.Nom == trajet.ElementAt(j).V2.Nom)
                    {
                        trajet.Remove(trajet.ElementAt(j));

                    }
                }
            }
            if (visite.Count() > 3)
            {
                return (true, trajet);
            }
            else
            {
                return (false, new HashSet<Route>());
            }
        }
        /*
        public static bool EstConnexe(Ville s1, Ville s2, Ville s3, Ville s4)
        {
            HashSet<Ville> a = Adjacents_Ville(s1);
            HashSet<Ville> visite = new HashSet<Ville>();

            visite.Add(s1);
            foreach (Ville v in a)
            {
                if (v.Nom == s1.Nom || v.Nom == s2.Nom || v.Nom == s3.Nom || v.Nom == s4.Nom) visite.Add(v);
            }
            HashSet<Ville> b = Adjacents_Ville(s2);
            foreach (Ville v in b)
            {
                if (v == s1 || v == s2 || v == s3 || v == s4)
                {
                    if (!visite.Contains(v)) visite.Add(v);
                }
            }
            HashSet<Ville> c = Adjacents_Ville(s3);
            foreach (Ville v in c)
            {
                if (v == s1 || v == s2 || v == s3 || v == s4)
                {
                    if (v == s1 || v == s2 || v == s3 || v == s4)
                    {
                        if (!visite.Contains(v)) visite.Add(v);
                    }
                }
            }
            HashSet<Ville> d = Adjacents_Ville(s4);
            foreach (Ville v in d)
            {
                if (v == s1 || v == s2 || v == s3 || v == s4)
                    if (v == s1 || v == s2 || v == s3 || v == s4)
                    {
                        if (!visite.Contains(v)) visite.Add(v);
                    }
            }

            if (visite.Count() >= 3) { return true; }
            else return false;
        }
        */

    }
    public enum MethodeItineraire
    {
        Temps,
        Distance,
    }
}