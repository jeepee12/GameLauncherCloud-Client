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
using Firebase.Database;
using Path = System.IO.Path;

namespace GameLauncherCloud_Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameCalculator gameCalculator;
        private FirebaseObject<Game> selectedGameDB;
        private Game SelectedGame => selectedGameDB?.Object;
        private const string DefaultImage = "pack://siteoforigin:,,,/Resources/controllerRezised.png";
        
        // TODO add the possibility to reorder games

        public MainWindow()
        {
            InitializeComponent();

            gameCalculator = new GameCalculator();
            UpdateGames();
            // TODO add something in the UI to say that we are loading
        }

        private async Task UpdateGames()
        {
            await gameCalculator.Start();
            AddGamesToUi();
            UpdateGameUi();
        }

        private void AddGamesToUi()
        {
            // Remove the loading label
            GameGrid.Children.Clear();
            ControlGrid.IsEnabled = true;

            bool firstGame = true;
            foreach (FirebaseObject<Game> game in gameCalculator.Games)
            {
                RadioButton rb = AddAGameToUi(game);
                rb.IsChecked = firstGame;
                if (firstGame)
                {
                    firstGame = false;
                    selectedGameDB = game;
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
            
            string error = gameCalculator.LaunchGame(selectedGameDB);
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
            GameName.Text = SelectedGame.Name;
            GameUrl.Text = SelectedGame.Url;

            if (!string.IsNullOrEmpty(SelectedGame.ImageUrl) && File.Exists(SelectedGame.ImageUrl))
            {
                // For some reason, relative URL don't works
                GameImage.Source = new BitmapImage(new Uri(Path.GetFullPath(SelectedGame.ImageUrl), UriKind.Absolute));
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
            GameTime total = SelectedGame.CalculateTotalTime();
            NbDays.Content = total.NbDays;
            NbHours.Content = total.NbHours;
            NbMinutes.Content = total.NbMinutes;
        }

        private RadioButton AddAGameToUi(FirebaseObject<Game> gameDB)
        {
            Game game = gameDB.Object;
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
                selectedGameDB = gameDB;
                UpdateGameUi();
            };

            GameGrid.Children.Add(rb);
            return rb;
        }

        private void GameImageUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null)
            {
                SelectedGame.ImageUrl = GameImageUrl.Text;
                // TODO update game logo in the grid
            }
        }

        private void GameUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null)
                SelectedGame.Url = GameUrl.Text;
        }

        private void GameName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null)
                SelectedGame.Name = GameName.Text;
        }

        private void AddAGameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            AddANewGameToUiAsync();
        }

        private async Task AddANewGameToUiAsync()
        {
            AddAGameToUi(await gameCalculator.CreateANewGame());
        }
    }
}
