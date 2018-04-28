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

using Path = System.IO.Path;

namespace GameLauncherCloud_Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            int numberOfGame = 30;
            
            for (int i = 0; i < numberOfGame; i++)
            {
                RadioButton rb = new RadioButton()
                {
                    IsChecked = i == 0,
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(0, 0, 0, 0)
                };
                if (i % 2 == 0) // TODO if the image exist we use it, else, we show the game name
                {
                    rb.Style = (Style)GameGrid.FindResource("GameImage");
                    rb.Content = Path.GetFullPath("Resources/controllerRezised.png");
                }
                else
                {
                    rb.Style = (Style)GameGrid.FindResource("GameName");
                    rb.Content = i.ToString();
                }
                rb.Checked += (sender, args) =>
                {
                    Console.WriteLine("Pressed " + (sender as RadioButton).Tag);
                };
                rb.Unchecked += (sender, args) => { /* Do stuff */ };
                rb.Tag = i; // TODO store the game object in the tag.
                
                GameGrid.Children.Add(rb);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            // TODO launch the selected game
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // TODO stop the timer
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO create and call save function (save to cloud)
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO refresh UI
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO save
        }
    }
}
