using System;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using FranceGPS.Views;
using FranceGPS.src.MessageBus;
using System.Reactive.Linq;

namespace FranceGPS
{
	public class MainWindowViewModel: ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        MessageBus ViewMessagesBus;

        public MainWindowViewModel()
		{
            ViewMessagesBus = new MessageBus();

            SetView(WindowViews.Home);

            ViewMessagesBus.Listen<MessageBusViewSwitcher>().Subscribe(MessageSubscribe);
        }

        void MessageSubscribe(MessageBusViewSwitcher m)
        {
            SetView(m.ViewType);
        }

        void SetView(WindowViews v)
        {
            IRoutableViewModel? _view = null;

            switch (v)
            {
                case WindowViews.Home:
                    _view = new HomeViewModel(this, ViewMessagesBus);
                    break;

                case WindowViews.Map:
                    _view = new PlanViewModel(this, ViewMessagesBus);
                    break;

                case WindowViews.Connexe:
                    _view = new ConnexeViewModel(this, ViewMessagesBus);
                    break;

                case WindowViews.Adjacent:
                    _view = new AdjacentViewModel(this, ViewMessagesBus);
                    break;

                case WindowViews.Distances:
                    _view = new DistancesViewModel(this, ViewMessagesBus);
                    break;

            }

            if (_view != null)
            {
                Router.Navigate.Execute(_view);
            }
        }
    }
}

