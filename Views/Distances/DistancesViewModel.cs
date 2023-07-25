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
    public class DistancesViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
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

        HashSet<Ville> _selections = new HashSet<Ville>();
        public HashSet<Ville> Selections
        {
            get => _selections;
            set {
                this.RaiseAndSetIfChanged(ref _selections, value);
                NotifyPropertyChanged(nameof(Selections));
            }
        }

        int _optionChoisit = 0;

        public int OptionChoisit
        {
            get => _optionChoisit;
            set
            {
                this.RaiseAndSetIfChanged(ref _optionChoisit, value);
                CalculerDistances();
            }
        }

        // Ville selectionner
        Ville _villeSelec = new Ville();
        public Ville VilleSelec
        {
            get => _villeSelec;
            set => this.RaiseAndSetIfChanged(ref _villeSelec, value);
        }

        // Liste Ville + distance
        ObservableCollection<Ville> _distancesVilles = new ObservableCollection<Ville>();
        public ObservableCollection<Ville> DistancesVilles
        {
            get => _distancesVilles;
            set {
                this.RaiseAndSetIfChanged(ref _distancesVilles, value);
                NotifyPropertyChanged(nameof(DistancesVilles));
            }
        }


        public bool TempsChoisit
        {
            get => ((MethodeItineraire)OptionChoisit) == MethodeItineraire.Temps;
        }


        public bool DistanceChoisit
        {
            get => ((MethodeItineraire)OptionChoisit) == MethodeItineraire.Distance;
        }

        IMessageBus MessagesBus;

        public DistancesViewModel(IScreen screen, IMessageBus bus)
        {
            HostScreen = screen;
            MessagesBus = bus;

            SelectionnerVille = ReactiveCommand.Create<Ville>(SelecVille);

            ListeVilles = new HashSet<Ville>(Graphe.ListeVilles);
            CollectionVilles = new ObservableCollection<Ville>(ListeVilles);

            GoHome = ReactiveCommand.Create(BackHome);
        }

        void BackHome()
        {
            MessagesBus.SendMessage(new MessageBusViewSwitcher(WindowViews.Home));
        }

        void ReinitialiserSelection()
        {
            VilleSelec = new Ville();

        }

        void CalculerDistances()
        {
            DistancesVilles = new ObservableCollection<Ville>(Graphe.Dijkstra(VilleSelec, (MethodeItineraire)OptionChoisit));
        }

        void SelecVille(Ville v)
        {
            Console.WriteLine($"Selection {v}");
            VilleSelec = v;
            Selections = new HashSet<Ville>() { VilleSelec  };

            CalculerDistances();
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

