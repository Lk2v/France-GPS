using System;
using System.Data.Common;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace FranceGPS.Components;

public partial class NavButton : TemplatedControl
{
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    private ICommand? _command;

    public ICommand Command
    {
        get
        {
            return _command!;
        }
        set
        {

            SetAndRaise(CommandProperty, ref _command!, value);
        }
    }

    public object CommandParameter
    {
        get
        {
            return GetValue(CommandParameterProperty);
        }
        set
        {
            SetValue(CommandParameterProperty, value);
        }
    }

    // Property


    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<NavButton, string>(
        nameof(Title), "Nom");

    public static readonly StyledProperty<string> IconProperty = AvaloniaProperty.Register<NavButton, string>(
        nameof(Icon), "");

    public static readonly DirectProperty<NavButton, ICommand> CommandProperty =
        AvaloniaProperty.RegisterDirect<NavButton, ICommand>(
            nameof(Command),
            (NavButton button) => button.Command,
            delegate (NavButton button, ICommand c) {
                button.Command = c;
            }, defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<NavButton, object>(nameof(CommandParameter));
}