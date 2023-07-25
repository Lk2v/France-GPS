using System;
using System.Collections.Generic;

namespace FranceGPS.src.Graphe
{
	public class Itineraire
	{
		public HashSet<Route> Chemin { get; set; } = new HashSet<Route>();

		public int Distance
		{
			get
			{
                int d = 0;
                foreach (Route r in Chemin)
                {
                    d += r.Distance;
                }
                return d;
            }
		}

        public int Duree
        {
            get
            {
                int t = 0;
                foreach (Route r in Chemin)
                {
                    t += r.DureeParcours;
                }
                return t;
            }
        }

        public string DureeFormat
        {
            get
            {
                int somme_minutes = Duree;

                int heures = somme_minutes / 60;
                int minutes = somme_minutes % 60;
                return $"{heures}h {minutes}min";
            }
        }

    }
}

