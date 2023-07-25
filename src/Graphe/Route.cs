using System;
namespace FranceGPS.src.Graphe
{
	public class Route
	{
		public Ville V1 { get; set; }
        public Ville V2 { get; set; }

		public int Distance { get; set; }
        public int DureeParcours { get; set;  }

        public Route(Ville s1, Ville s2, int d, int t)
        {
            V1 = s1;
            V2 = s2;
            Distance = d;
            DureeParcours = t;
        }

        public bool Contient(Ville s)
        {
            return (s.Nom == V1.Nom || s.Nom == V2.Nom);
        }

        public bool Contient(Ville s1, Ville s2)
        {
            return (s1.Nom == V1.Nom && s2.Nom == V2.Nom) || (s2.Nom == V1.Nom && s1.Nom == V2.Nom);
        }

        public override string ToString()
        {
            return $"({V1.Nom} - {V2.Nom}) : {Distance}km ; {DureeParcours}min";
        }

    }
}

