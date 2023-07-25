using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FranceGPS.src.Graphe
{
	public class Donnees
	{
        static string dossierProjet = Path.Combine(Directory.GetParent(Environment.CurrentDirectory!)!.Parent!.Parent!.FullName, "Assets");

        /*
            En utilisant la syntaxe "using", les objets StreamReader seront automatiquement fermés et libérés de la mémoire
            dès que le bloc de code sera terminé, même en cas d'exception.
            Cela peut donc améliorer la gestion des ressources et la performance
         */

        public static HashSet<Ville> ChargerVilles()
        {
            HashSet<Ville> ls_villes = new HashSet<Ville>();
            using (StreamReader sr = new StreamReader(Path.Combine(dossierProjet, "villes.csv")))
            {
                int i = 0;
                string ligne = "";
                string[] tab = new string[3];
                while (sr.Peek() > 0)
                {
                    ligne = sr.ReadLine()!;
                    tab = ligne.Split(';');

                    Ville v = new Ville
                    {
                        Nom = tab[0],
                        Num = i,
                        X = Convert.ToDouble(tab[1]),
                        Y = Convert.ToDouble(tab[2]),
                    };

                    ls_villes.Add(v);
                    i++;
                }
            }

            return ls_villes;
        }

        public static HashSet<Route> ChargerRoutes(HashSet<Ville> villes)
        {
            HashSet<Route> ls_routes = new HashSet<Route>();
            using (StreamReader sr = new StreamReader(Path.Combine(dossierProjet, "connections.csv")))
            {
                string ligne = "";

                string[] info = new string[4];

                while (sr.Peek() > 0)
                {
                    ligne = sr.ReadLine()!;
                    info = ligne.Split(',');

                    
                    Ville v1 = villes.First(item => item.Nom == info[0]);
                    Ville v2 = villes.First(item => item.Nom == info[1]);

                    int d = Convert.ToInt32(info[2]);

                    string[] infoh = info[3].Split(':');

                    int t = Convert.ToInt32(infoh[0]) * 60 + Convert.ToInt32(infoh[1]);

                    Route rd = new Route(v1, v2, d, t);

                    ls_routes.Add(rd);
                }
            }

            return ls_routes;
        }
    }
}

