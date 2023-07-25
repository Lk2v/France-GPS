using System;
using System.ComponentModel;

namespace FranceGPS.src.Graphe
{
    public class Ville : INotifyPropertyChanged
    {
        public string Nom { get; set; } = "";

        public double X { get; set; }
        public double Y { get; set; }

        public bool Visite { get; set; }
        public int Num { get; set; }
        public Ville? Pred { get; set; } = null;

        public int Distance { get; set; } //Dijkastra v1
        public int Temps { get; set; } //Dijkastra v1

        #region Pour partie : Est connexe
        bool _estSelectionner = false;

        public bool EstSelectionner
        {
            get => _estSelectionner;
            set
            {
                _estSelectionner = value;
                NotifyPropertyChanged(nameof(EstSelectionner));
            }
        }
        #endregion
        public Ville()
        {
            // Constructeur par defaut
        }

        public Ville(string nom)
        {
            Nom = nom;
        }

        public Ville(string nom, int num, Ville? pred)
        {
            Nom = nom;
            Num = num;
            Pred = pred;
        }
        public Ville(string nom, int num)
        {
            Nom = nom;
            Num = num;
        }
        public Ville(string nom, int num, double coordx, double coordy)
        {
            Nom = nom;
            Num = num;
            X = coordx;
            Y = coordy;
        }

        #region UI
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
        string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public override string ToString()
        {
            return $"({Num}) {Nom}";
        }

        public Ville Clone()
        {
            return new Ville
            {
                Nom = Nom,
                X =X,
                Y = Y,
                Visite = Visite,
                Num = Num,
                Pred = Pred,

                Distance = Distance,
                Temps = Temps,
            };
        }
    }
}

