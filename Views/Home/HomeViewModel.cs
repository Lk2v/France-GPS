using System;
using System.Reactive;
using ReactiveUI;
using FranceGPS.src.MessageBus;
using FranceGPS.src.Graphe;
using System.Collections.Generic;

namespace FranceGPS.Views
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<WindowViews, Unit> Navigation { get; }

        public IMessageBus MessagesBus;

        public HomeViewModel(IScreen screen, IMessageBus bus)
        {
            HashSet<Ville> s = Graphe.ListeVilles;

            HostScreen = screen;
            MessagesBus = bus;

            Navigation = ReactiveCommand.Create<WindowViews>(Selection);
        }

        void Selection(WindowViews nav)
        {
            MessagesBus.SendMessage(new MessageBusViewSwitcher(nav));
        }
    }
}

