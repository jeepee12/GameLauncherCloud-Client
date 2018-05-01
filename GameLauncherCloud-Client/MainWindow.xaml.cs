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
        private GameCalculator gameCalculator;
        private Game selectedGame;

        // TODO add the possibility to add games

        public MainWindow()
        {
            InitializeComponent();

            gameCalculator = new GameCalculator();
            AddGamesToUI();
        }

        private void AddGamesToUI()
        {
            bool firstGame = true;
            foreach (Game game in gameCalculator.games)
            {
                RadioButton rb = new RadioButton()
                {
                    IsChecked = firstGame,
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(0, 0, 0, 0)
                };

                if (string.IsNullOrEmpty(game.Url)) // TODO if the image exist we use it, else, we show the game name
                {
                    rb.Style = (Style)GameGrid.FindResource("GameImage");
                    rb.Content = Path.GetFullPath("Resources/controllerRezised.png");
                }
                else
                {
                    rb.Style = (Style)GameGrid.FindResource("GameName");
                    rb.Content = game.Name;
                }
                //rb.Tag = game;

                rb.Checked += (sender, args) =>
                {
                    selectedGame = game;
                    UpdateGameUi();
                };

                GameGrid.Children.Add(rb);

                if (firstGame)
                {
                    firstGame = false;
                    selectedGame = game;
                }
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            LaunchGame();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopGame();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            gameCalculator.SaveData();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshGameTime();
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopGame();
            gameCalculator.SaveData();
        }

        private void LaunchGame()
        {
            
            string error = gameCalculator.LaunchGame(selectedGame);
            if (string.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error);
            }
            else
            {
                SetGamesRadio(false);
            }
        }

        private void StopGame()
        {
            SetGamesRadio(false);
            gameCalculator.StopGame();
        }

        private void SetGamesRadio(bool enable)
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsEnabled = enable;
                }
            }
        }

        private void UpdateGameUi()
        {
            GameName.Text = selectedGame.Name;
            GameUrl.Text = selectedGame.Url;
            Uri imageUri = new Uri($"ms-appx:///Resources/{selectedGame.ImageUrl}");
            //Uri.TryCreate(selectedGame.ImageUrl, imageUri);
            //if (imageUri.)
            //GameImage.Source = new BitmapImage(imageUri);
            

            RefreshGameTime();
        }

        private void RefreshGameTime()
        {
            NbDays.Content = selectedGame.Time.NbDays;
            NbHours.Content = selectedGame.Time.NbHours;
            NbMinutes.Content = selectedGame.Time.NbMinutes;
        }
    }
}
