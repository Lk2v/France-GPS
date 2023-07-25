using System;
namespace FranceGPS.src.MessageBus
{

    public class MessageBusViewSwitcher
    {
        public MessageBusViewSwitcher(WindowViews viewType)
        {
            ViewType = viewType;
        }

        public WindowViews ViewType { get; }
    }

    public enum WindowViews
    {
		Home,
		Map,
        Connexe,
        Adjacent,
        Distances,
    }
}

