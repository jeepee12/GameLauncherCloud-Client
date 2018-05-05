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
        // Firebase with C#
        // Medium article: https://medium.com/step-up-labs/firebase-c-library-5c342989ad18
        // FirebaseDatabase.net https://github.com/step-up-labs/firebase-database-dotnet
        // FirebaseAuthentication.net https://github.com/step-up-labs/firebase-authentication-dotnet

        private const double OneMinuteInMs = 60000.0;

        public List<Game> games;
        private Timer gameTimer = new Timer();
        private Game currentGame;

        public GameCalculator()
        {
            // TODO try to load from cloud
            games = new List<Game>();

            games.Add(new Game(0, "Starcraft", "shortcut//StarCraft II", "Resources//SC2.png", new GameTime())); //"Resources/controllerRezised.png"
            games.Add(new Game(1, "Hearthstone", "shortcut//LancerHearthstone.bat", "Resources//HearthStone.jpeg", new GameTime()));


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

        public Game CreateANewGame()
        {
            // TODO Get a new id from the data base (maybe a GUIID)
            Game game = new Game(0);
            games.Add(game);
            return game;
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
