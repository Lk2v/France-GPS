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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FranceGPS.Views
{
    public class AdjacentViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, Unit> GoHome { get; }
        public ReactiveCommand<Ville, Unit> SelectionnerVille { get; }

        ObservableCollection<Ville> _collectionVilles = new ObservableCollection<Ville>();
        public ObservableCollection<Ville> CollectionVilles
        {
            get => _collectionVilles;
            set => this.RaiseAndSetIfChanged(ref _collectionVilles, value);
        }
       
        HashSet<Ville> _listeVilles = new HashSet<Ville>();
        public HashSet<Ville> ListeVilles
        {
            get => _listeVilles;
            set => this.RaiseAndSetIfChanged(ref _listeVilles, value);
        }

        HashSet<Route> _listeRoutes = new HashSet<Route>();
        public HashSet<Route> ListeRoutes
        {
            get => _listeRoutes;
            set
            {
                this.RaiseAndSetIfChanged(ref _listeRoutes, value);
                NotifyPropertyChanged(nameof(ListeRoutes));
            }
        }

        bool? _estAdjacent = null;
        public bool? EstAdjacent
        {
            get => _estAdjacent;
            set {
                this.RaiseAndSetIfChanged(ref _estAdjacent, value);
                NotifyPropertyChanged(nameof(ResultatAdjacent));
                NotifyPropertyChanged(nameof(AfficherMessage));
            }
        }

        public bool ResultatAdjacent
        {
            get => _estAdjacent == true;
        }

        public bool AfficherMessage
        {
            get => _estAdjacent != null;
        }

        public HashSet<Ville> Selections
        {
            get
            {
                HashSet<Ville> selection = new HashSet<Ville>();
                foreach (Ville v in ListeVilles)
                {
                    if (v.EstSelectionner)
                    {
                        selection.Add(v);
                    }
                }
                return selection;
            }
        }

        IMessageBus MessagesBus;

        public AdjacentViewModel(IScreen screen, IMessageBus bus)
        {
            HostScreen = screen;
            MessagesBus = bus;

            SelectionnerVille = ReactiveCommand.Create<Ville>(SelecVille);


            ListeVilles = Graphe.ListeVillesClone;
            CollectionVilles = new ObservableCollection<Ville>(ListeVilles);

            
            GoHome = ReactiveCommand.Create(BackHome);
        }

        void BackHome()
        {
            MessagesBus.SendMessage(new MessageBusViewSwitcher(WindowViews.Home));
        }

        void VerifAdjacent()
        {
            Console.WriteLine("Verification connexe");
            Route? route; 
            (EstAdjacent, route) = Graphe.EstAdjacents_Ville(
                Selections.ElementAt(0),
                Selections.ElementAt(1)
            );

            if(EstAdjacent == true)
            {
                ListeRoutes = new HashSet<Route>() { (Route) route! };
            }

            Console.WriteLine($"Resultat : {EstAdjacent}");
        }

        

        void ReinitialiserSelection()
        {
            for(int i = 0; i < ListeVilles.Count; i++)
            {
                ListeVilles.ElementAt(i).EstSelectionner = false;
            }
            EstAdjacent = null;

            ListeRoutes = new HashSet<Route>();
        }

        void SelecVille(Ville v)
        {
            if (EstAdjacent != null)
            {
                ReinitialiserSelection();
            }

            v.EstSelectionner = !v.EstSelectionner;

            NotifyPropertyChanged(nameof(Selections));

            if (Selections.Count >= 2)
            {
                VerifAdjacent();
            }
            
        }

        public new event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
        string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

