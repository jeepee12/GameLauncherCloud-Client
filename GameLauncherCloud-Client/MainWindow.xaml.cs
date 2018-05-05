using System;
using System.Collections.Generic;
using System.IO;
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
        private const string DefaultImage = "pack://siteoforigin:,,,/Resources/controllerRezised.png";

        // TODO add the possibility to add games
        // TODO add the possibility to reorder games

        public MainWindow()
        {
            InitializeComponent();

            gameCalculator = new GameCalculator();
            AddGamesToUi();
            UpdateGameUi();
        }

        private void AddGamesToUi()
        {
            bool firstGame = true;
            foreach (Game game in gameCalculator.games)
            {
                RadioButton rb = AddAGameToUi(game);
                rb.IsChecked = firstGame;
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

            if (!string.IsNullOrEmpty(selectedGame.ImageUrl) && File.Exists(selectedGame.ImageUrl))
            {
                // For some reason, relative URL don't works
                GameImage.Source = new BitmapImage(new Uri(Path.GetFullPath(selectedGame.ImageUrl), UriKind.Absolute));
            }
            else
            {
                // Default image
                GameImage.Source = new BitmapImage(new Uri(DefaultImage));
            }

            RefreshGameTime();
        }

        private void RefreshGameTime()
        {
            NbDays.Content = selectedGame.Time.NbDays;
            NbHours.Content = selectedGame.Time.NbHours;
            NbMinutes.Content = selectedGame.Time.NbMinutes;
        }

        private RadioButton AddAGameToUi(Game game)
        {
            RadioButton rb = new RadioButton()
            {
                Width = 80,
                Height = 80,
                Margin = new Thickness(0, 0, 0, 0)
            };

            if (!string.IsNullOrEmpty(game.ImageUrl) && File.Exists(game.ImageUrl))
            {
                rb.Style = (Style)GameGrid.FindResource("GameImage");
                rb.Content = Path.GetFullPath(game.ImageUrl);
            }
            else
            {
                // TODO Support online image
                rb.Style = (Style)GameGrid.FindResource("GameName");
                rb.Content = game.Name;
            }

            rb.Checked += (sender, args) =>
            {
                selectedGame = game;
                UpdateGameUi();
            };

            GameGrid.Children.Add(rb);
            return rb;
        }

        private void GameImageUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedGame != null)
                selectedGame.ImageUrl = GameImageUrl.Text;
        }

        private void GameUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedGame != null)
                selectedGame.Url = GameUrl.Text;
        }

        private void GameName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (selectedGame != null)
                selectedGame.Name = GameName.Text;
        }

        private void AddAGameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            AddAGameToUi(gameCalculator.CreateANewGame());
        }
    }
}
