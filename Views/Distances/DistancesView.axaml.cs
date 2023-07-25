using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SkiaSharp;

namespace FranceGPS.Views;

public partial class DistancesView : ReactiveUserControl<DistancesViewModel>
{
    public DistancesView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}