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

            for (int i = 0; i < 4; i++)
            {
                RadioButton rb = new RadioButton() {
                    Content = Path.GetFullPath("Resources/controllerRezised.png"),
                    IsChecked = i == 0,
                    Style = (Style)GameList.FindResource("Flag"),
                    Width = 80,
                    Height = 80
                };
                rb.Checked += (sender, args) =>
                {
                    Console.WriteLine("Pressed " + (sender as RadioButton).Tag);
                };
                rb.Unchecked += (sender, args) => { /* Do stuff */ };
                rb.Tag = i;

                GameList.Children.Add(rb);
            }
        }
    }
}
