using System;
using ReactiveUI;

using FranceGPS.Views;

namespace FranceGPS
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
        {
            HomeViewModel context => new HomeView { DataContext = context },
            PlanViewModel context => new PlanView { DataContext = context },
            ConnexeViewModel context => new ConnexeView { DataContext = context },
            AdjacentViewModel context => new AdjacentView { DataContext = context },
            DistancesViewModel context => new DistancesView { DataContext = context },

            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}

