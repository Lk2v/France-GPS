using System;
using System.Reactive;
using ReactiveUI;
using FranceGPS.src.MessageBus;
using System.Collections.Generic;
using FranceGPS.src.Graphe;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Linq;
using Avalonia;

namespace FranceGPS.Views
{
    public class PlanViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, Unit> GoHome { get; }

        HashSet<Ville> _listeVilles = new HashSet<Ville>();
        public HashSet<Ville> ListeVilles {
            get => _listeVilles;
            set => this.RaiseAndSetIfChanged(ref _listeVilles, value);
        }

        Ville? _villeSelectionnerDepart;
        public Ville VilleSelectionnerDepart
        {
            get => _villeSelectionnerDepart != null ? _villeSelectionnerDepart : new Ville();
            set
            {
                this.RaiseAndSetIfChanged(ref _villeSelectionnerDepart, value);
                Itineraire();
            }
        }

        Ville? _villeSelectionnerArrivee;
        public Ville VilleSelectionnerArrivee
        {
            get => _villeSelectionnerArrivee != null ? _villeSelectionnerArrivee : new Ville();
            set
            {
                this.RaiseAndSetIfChanged(ref _villeSelectionnerArrivee, value);
                Itineraire();
            }
        }

        HashSet<Ville> _listeVillesSelec = new HashSet<Ville>();
        public HashSet<Ville> ListeVillesSelec
        {
            get => _listeVillesSelec;
            set => this.RaiseAndSetIfChanged(ref _listeVillesSelec, value);
        }

        Itineraire _routesItineraire = new Itineraire();
        public Itineraire RoutesItineraire
        {
            get => _routesItineraire;
            set => this.RaiseAndSetIfChanged(ref _routesItineraire, value);
        }

        int _optionChoisit = 0;
        public int OptionChoisit
        {
            get => _optionChoisit;
            set
            {
                this.RaiseAndSetIfChanged(ref _optionChoisit, value);
                Itineraire(); // reactualisation
            }
        }


        IMessageBus MessagesBus;

        public PlanViewModel(IScreen screen, IMessageBus bus)
        {
            HostScreen = screen;
            MessagesBus = bus;

            GoHome = ReactiveCommand.Create(BackHome);

            ListeVilles = Graphe.ListeVilles;

            if (ListeVilles.Count > 1)
            {
                _villeSelectionnerDepart = ListeVilles.ElementAt(0);
                _villeSelectionnerArrivee = ListeVilles.ElementAt(1);
                Itineraire();
            }
        }

        void BackHome()
        {
            MessagesBus.SendMessage(new MessageBusViewSwitcher(WindowViews.Home));
        }

        void Itineraire()
        {
            Ville a = VilleSelectionnerDepart;
            Ville b = VilleSelectionnerArrivee;
            Console.WriteLine("Construction de l'itinéraire...");
            if(a == b)
            {
                Console.WriteLine("Vous êtes déjà arrivée a desination");
                return;
            }

            // C'est partit
            Console.WriteLine($"{a} <-> {b}");

            
            // Dijkastra
            RoutesItineraire = Graphe.Dijkstra2(a, (MethodeItineraire) OptionChoisit, b);
            ListeVillesSelec = new HashSet<Ville>() { a, b };
        }

        // autre
        HashSet<Ville> EnleverVille(HashSet<Ville> source, Ville enlever)
        {
            HashSet<Ville> liste = new HashSet<Ville>();

            foreach(Ville v in source)
            {
                if(v != enlever) liste.Add(v);
            }

            return liste;
        }
    }
}

