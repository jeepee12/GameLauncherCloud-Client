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
        private RadioButton selectedGameRadioButton;
        private const string DefaultImage = "pack://siteoforigin:,,,/Resources/controllerRezised.png";
        private bool isClosing = false;

        // TODO add the possibility to reorder games

        public MainWindow()
        {
            InitializeComponent();

            gameCalculator = new GameCalculator();
            UpdateGames();
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
                if (game.Object.IsSteamGame)
                    continue;
                
                RadioButton rb = AddAGameToUi(game);
                rb.IsChecked = firstGame;
                if (firstGame)
                {
                    firstGame = false;
                    rb.IsChecked = true;
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
            // Thx to : https://stackoverflow.com/questions/16656523/awaiting-asynchronous-function-inside-formclosing-event
            if (!isClosing)
            {
                e.Cancel = true;
                CloseAsync();
            }
        }

        private async Task CloseAsync()
        {
            StopGame();
            var saving = gameCalculator.SaveData();
            ControlGrid.IsEnabled = false;
            GameGrid.Children.Clear();
            GameGrid.Children.Add(new Label() {Content = "Closing..."});

            await Task.Yield();
            await saving;
            isClosing = true;

            Close();
        }

        private void LaunchGame()
        {

            string error = gameCalculator.LaunchGame(selectedGameDB);
            if (!string.IsNullOrWhiteSpace(error))
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
            SetGamesRadio(true);
            gameCalculator.StopGame();
            RefreshGameTime();
        }

        private void SetGamesRadio(bool enable)
        {
            PlayBtn.IsEnabled = enable;
            StopBtn.IsEnabled = !enable;
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
            GameImageUrl.Text = SelectedGame.ImageUrl;

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

        private void UpdateRadioButtonImage(RadioButton rb, Game game)
        {
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
        }

        private void RefreshGameTime()
        {
            GameTime total = SelectedGame.CalculateTotalTime();
            NbDays.Content = total.NbDays;
            NbHours.Content = total.NbHours % GameTime.NbHoursInDay;
            NbMinutes.Content = total.NbMinutes % GameTime.NbMinutesInHour;
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

            UpdateRadioButtonImage(rb, game);

            rb.Checked += (sender, args) =>
            {
                selectedGameDB = gameDB;
                selectedGameRadioButton = rb;
                UpdateGameUi();
            };

            GameGrid.Children.Add(rb);
            return rb;
        }

        private void GameImageUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null && SelectedGame.ImageUrl != GameImageUrl.Text)
            {
                SelectedGame.ImageUrl = GameImageUrl.Text;
                gameCalculator.NotifyGameInformationUpdated(selectedGameDB);
                UpdateGameUi();
                UpdateRadioButtonImage(selectedGameRadioButton, SelectedGame);
            }
        }

        private void GameUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null && SelectedGame.Url != GameUrl.Text)
            {
                SelectedGame.Url = GameUrl.Text;
                gameCalculator.NotifyGameInformationUpdated(selectedGameDB);
            }
        }

        private void GameName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedGame != null && SelectedGame.Name != GameName.Text)
            {
                SelectedGame.Name = GameName.Text;

                gameCalculator.NotifyGameInformationUpdated(selectedGameDB);
            }
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
