using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Timer.Interval = TimeSpan.FromSeconds(0.166);
            Timer.Tick += Timer_Tick;
            Timer.Stop();  //Wird in ButtonStart_Click enabled
        }
         
        private void Timer_Tick(object sender, EventArgs e) //Spielregeln mit Timer laufen lassen
        {
            if (Timer.IsEnabled)
            {
                Gameoflife(); //Spielregeln
            }
        }

        const int AnzahlFelderBreite = 50;
        const int AnzahlFelderHöhe = 50;    //Beide Werte können nach belieben verändert werden

        Rectangle[,] Felder = new Rectangle[AnzahlFelderHöhe, AnzahlFelderBreite];
        DispatcherTimer Timer = new DispatcherTimer();

        private void ButtonStart_Click(object sender, RoutedEventArgs e) // Start/Stop für den Timer bzw das GameOfLife
        {
            if (ButtonStart.Content.ToString() == "Start")
            {
                Timer.Start();
                ButtonStart.Content = "Stop";
            }
            else
            {
                Timer.Stop();
                ButtonStart.Content = "Start";
            }

        }

        private void Zeichenfläche_Loaded(object sender, RoutedEventArgs e)  //Aufbau des Spielfeldes
        {

            Random Münze = new Random(); //Zum auswürfen des Startwerts eines Rectangle

            for (int i = 0; i < AnzahlFelderHöhe; i++)
            {
                for (int j = 0; j < AnzahlFelderBreite; j++)
                {
                    Felder[i,j] = new Rectangle();

                    Felder[i, j].Height = (Zeichenfläche.ActualHeight / AnzahlFelderHöhe) - 2.0;
                    Felder[i, j].Width = (Zeichenfläche.ActualWidth / AnzahlFelderBreite) - 2.0;
                    if (Münze.Next(2) == 1)
                        Felder[i, j].Fill = Brushes.Olive;
                    else
                        Felder[i, j].Fill = Brushes.Cyan;
                    Zeichenfläche.Children.Add(Felder[i, j]);
                    Canvas.SetLeft(Felder[i, j], j * (Zeichenfläche.ActualWidth / AnzahlFelderBreite));
                    Canvas.SetTop(Felder[i, j], i * (Zeichenfläche.ActualHeight / AnzahlFelderHöhe));
                    Felder[i, j].MouseDown += R_MouseDown;
                }
            }
        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e) // Ändern der Feldfarbe eines Elements per Mausklick
        {
            //Button btn = sender as Button
            Rectangle r = sender as Rectangle;
            if (r.Fill == Brushes.Cyan)
                r.Fill = Brushes.Olive;
            else r.Fill = Brushes.Cyan;

        }

        private void Randomer_Click(object sender, RoutedEventArgs e) // Das Spielfeld neu würfeln/reseten
        {
            Random Münze = new Random();
            for (int i = 0; i < AnzahlFelderHöhe; i++)
            {
                for (int j = 0; j < AnzahlFelderBreite; j++)
                {
                    if (Münze.Next(2) == 1)
                        Felder[i, j].Fill = Brushes.Olive;
                    else
                        Felder[i, j].Fill = Brushes.Cyan;
                }
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e) // Manuel einen Tick ausführen mit Button
        {
            Gameoflife();
        }
        private void Gameoflife() //Spielregel des GameOfLife
        {
        
            int[,] nachbarn = new int[AnzahlFelderHöhe, AnzahlFelderBreite];

            for (int i = 0; i < AnzahlFelderHöhe; i++)
            {
                for (int j = 0; j < AnzahlFelderBreite; j++)
                {
                    int Oben, Mitte, Unten, Links, Mittig, Rechts = 0;

                    Oben = i - 1;
                    Mitte = i;
                    Unten = i + 1;

                    Links = j - 1;
                    Mittig = j;
                    Rechts = j + 1;

                    if (Links == -1)
                    {
                        Links = AnzahlFelderBreite - 1;
                    }
                    if (Rechts == AnzahlFelderBreite)
                    {
                        Rechts = 0;
                    }

                    if (Oben == -1)
                    {
                        Oben = AnzahlFelderHöhe - 1;
                    }
                    if (Unten == AnzahlFelderHöhe)
                    {
                        Unten = 0;
                    }

                    nachbarn[i, j] = 0;

                    if (Felder[Oben, Links].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Oben, Mittig].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Oben, Rechts].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Mitte, Links].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Mitte, Rechts].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Unten, Links].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Unten, Mittig].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                    if (Felder[Unten, Rechts].Fill == Brushes.Olive)
                        nachbarn[i, j]++;
                }
            }
            for (int i = 0; i < AnzahlFelderHöhe; i++)
            {
                for (int j = 0; j < AnzahlFelderBreite; j++)
                {

                    if (Felder[i, j].Fill == Brushes.Olive && ((nachbarn[i, j] < 2) || (nachbarn[i, j] > 3)))
                    {
                        Felder[i, j].Fill = Brushes.Cyan;
                    }
                    //  else
                    //  {
                    //Felder[i, j].Fill = Brushes.Blue;
                    //}

                    if (Felder[i, j].Fill == Brushes.Cyan && (nachbarn[i, j] == 3))
                    {
                        Felder[i, j].Fill = Brushes.Olive;
                    }
                }
            }
         
        }
    }
}
