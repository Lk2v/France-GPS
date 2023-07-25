using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SkiaSharp;

namespace FranceGPS.Views;

public partial class ConnexeView : ReactiveUserControl<ConnexeViewModel>
{
    public ConnexeView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}