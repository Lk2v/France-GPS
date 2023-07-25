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
using System.Windows.Input;
using Avalonia.Interactivity;
using FranceGPS.Components;

namespace FranceGPS.Views
{
    public class ConnexeViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
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


        bool? _estConnexe = null;
        public bool? EstConnexe
        {
            get => _estConnexe;
            set {
                this.RaiseAndSetIfChanged(ref _estConnexe, value);
                NotifyPropertyChanged(nameof(ResultatConnexe));
                NotifyPropertyChanged(nameof(AfficherMessage));
            }
        }

        public bool ResultatConnexe
        {
            get => _estConnexe == true;
        }

        public bool AfficherMessage
        {
            get => _estConnexe != null;
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

        HashSet<Route> _listeRouteConnexe = new HashSet<Route>();
        public HashSet<Route> ListeRouteConnexe
        {
            get => _listeRouteConnexe;
            set
            {
                this.RaiseAndSetIfChanged(ref _listeRouteConnexe, value);
                NotifyPropertyChanged(nameof(ListeRouteConnexe));
            }
        }

        public ConnexeViewModel(IScreen screen, IMessageBus bus)
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

        void VerifConnexe()
        {
            Console.WriteLine("Verification connexe");
            (EstConnexe, ListeRouteConnexe) = Graphe.EstConnexe(
                Selections.ElementAt(0),
                Selections.ElementAt(1),
                Selections.ElementAt(2),
                Selections.ElementAt(3)
            );
            Console.WriteLine($"Resultat : {EstConnexe}");
        }

        

        void ReinitialiserSelection()
        {
            for(int i = 0; i < ListeVilles.Count; i++)
            {
                ListeVilles.ElementAt(i).EstSelectionner = false;
            }
            EstConnexe = null;
            ListeRouteConnexe = new HashSet<Route>();
        }

        void SelecVille(Ville v)
        {
            if(_estConnexe != null)
            {
                ReinitialiserSelection();
            }

           
            v.EstSelectionner = !v.EstSelectionner;
            NotifyPropertyChanged(nameof(Selections));

            if (Selections.Count >= 4)
            {
                VerifConnexe();
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

