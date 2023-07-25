using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using FranceGPS.src.Graphe;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace FranceGPS.Components
{
    public class Carte : Canvas
    {
        public static readonly StyledProperty<HashSet<Ville>> VillesProperty =
            AvaloniaProperty.Register<Carte, HashSet<Ville>>(nameof(Villes), new HashSet<Ville>());

        public static readonly StyledProperty<HashSet<Route>> RoutesProperty =
            AvaloniaProperty.Register<Carte, HashSet<Route>>(nameof(Routes), new HashSet<Route>());

        public static readonly StyledProperty<HashSet<Ville>> VillesSelectionnerProperty =
            AvaloniaProperty.Register<Carte, HashSet<Ville>>(nameof(VillesSelectionner), new HashSet<Ville>());

        // Evenements
        public static readonly RoutedEvent<RoutedEventArgs> VilleClickEvent =
    RoutedEvent.Register<Carte, RoutedEventArgs>(nameof(VilleClick), RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> VilleClick
        {
            add { AddHandler(VilleClickEvent, value); }
            remove { RemoveHandler(VilleClickEvent, value); }
        }

        public HashSet<Ville> Villes
        {
            get { return GetValue(VillesProperty); }
            set { SetValue(VillesProperty, value); }
        }

        public HashSet<Route> Routes
        {
            get { return GetValue(RoutesProperty); }
            set { SetValue(RoutesProperty, value); }
        }

        public HashSet<Ville> VillesSelectionner
        {
            get { return GetValue(VillesSelectionnerProperty); }
            set { SetValue(VillesSelectionnerProperty, value); }
        }

        private ICommand? _villeCommand;

        public ICommand VilleCommand
        {
            get
            {
                return _villeCommand!;
            }
            set
            {

                SetAndRaise(VilleCommandProperty, ref _villeCommand!, value);
            }
        }

        public static readonly DirectProperty<Carte, ICommand> VilleCommandProperty =
        AvaloniaProperty.RegisterDirect<Carte, ICommand>(
            nameof(_villeCommand),
            (Carte c) => c.VilleCommand,
            delegate (Carte c, ICommand cmd) {
                c.VilleCommand = cmd;
            }, defaultBindingMode: BindingMode.OneWay);


        public Carte()
        {
            this.GetObservable(VillesProperty).Subscribe(_ => InvalidateVisual());
            this.GetObservable(RoutesProperty).Subscribe(_ => InvalidateVisual());
            this.GetObservable(VillesSelectionnerProperty).Subscribe(_ => InvalidateVisual());
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                Point clickPoint = e.GetPosition(this);

                foreach (Ville v in Villes)
                {
                    double distance = Math.Sqrt(Math.Pow(clickPoint.X - v.X, 2) + Math.Pow(clickPoint.Y - v.Y, 2)); // Pythagore on cherche (hypothenus=distance)

                    if (distance <= 10) // Rayon de l'ellipse
                    {
                        if (VilleCommand != null && VilleCommand.CanExecute(null))
                        {
                            VilleCommand.Execute(v);
                        }
                    }
                }
            }
        }

        public override void Render(DrawingContext context)
        {
            Console.WriteLine($"Mise à jour de la carte");


            Pen pen = new Pen(Brushes.Black, 5);
            Pen ligne = new Pen(Brushes.White, 5, DashStyle.DashDotDot);

            // Routes
            foreach (Route r in Routes)
            {
                context.DrawLine(ligne, new Point(r.V1.X, r.V1.Y), new Point(r.V2.X, r.V2.Y));
            }

            // Villes
            foreach (Ville v in Villes)
            {
                int rayon = 10;
                int taillePolice = 35;

                FontWeight weight = FontWeight.Normal;

                if (Routes.Count > 0)
                {
                    foreach(Route r in Routes)
                    {
                        if(r.Contient(v))
                        {
                            weight = FontWeight.Bold;
                        }
                    }
                }

                if (VillesSelectionner.Contains(v))
                {
                    rayon = 20;

                    taillePolice = 40;
                }

                var formattedText = new FormattedText
                {
                    Text = v.Nom,
                    Typeface = new Typeface("Arial", FontStyle.Normal, weight),
                    TextAlignment = TextAlignment.Center,
                    FontSize = taillePolice,
                };

                context.DrawEllipse(Brushes.White, pen, new Point(v.X, v.Y), rayon, rayon);
                context.DrawText(Brushes.White, new Point(v.X-formattedText.Bounds.Width/2, v.Y+20), formattedText);
            }
        }
    }
}