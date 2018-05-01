using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameLauncherCloud_Client
{
    class GameCalculator
    {
        private const double OneMinuteInMs = 60000.0;

        public List<Game> games;
        private Timer gameTimer = new Timer();
        private Game currentGame;

        public GameCalculator()
        {
            // TODO try to load from cloud
            games = new List<Game>();

            games.Add(new Game("Starcraft", "shortcut//StarCraft II", "SC2.png", new GameTime()));
            games.Add(new Game("Hearthstone", "shortcut//LancerHearthstone.bat", "HearthStone.jpeg", new GameTime()));


            gameTimer.Interval = OneMinuteInMs;
            gameTimer.Elapsed += OnTimedEvent;
        }

        public string LaunchGame(Game game)
        {
            string errorMessage = null;
            currentGame = game;
            if (!string.IsNullOrWhiteSpace(currentGame.Url))
            {
                try
                {
                    System.Diagnostics.Process.Start(currentGame.Url);
                    gameTimer.Enabled = true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            else
            {
                errorMessage = "The game Url is null or empty";
            }
            return errorMessage;
        }

        public void StopGame()
        {
            gameTimer.Enabled = false;
        }

        public void SaveData()
        {
            // TODO Push the data to the cloud
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            currentGame.Time.NbMinutes++;
        }
    }
}
