using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Firebase.Database;
using Firebase.Database.Query;

namespace GameLauncherCloud_Client
{
    class GameCalculator
    {
        // Firebase with C#
        // Medium article: https://medium.com/step-up-labs/firebase-c-library-5c342989ad18
        // FirebaseDatabase.net https://github.com/step-up-labs/firebase-database-dotnet
        // FirebaseAuthentication.net https://github.com/step-up-labs/firebase-authentication-dotnet
        // TODO add the possiblity to upload the games icon to the database
        // File upload https://github.com/step-up-labs/firebase-storage-dotnet

        private const double OneMinuteInMs = 60000.0;

        public List<Game> Games;
        private List<Game> gamesUpdated;

        private FirebaseClient firebaseClient;
        private Timer gameTimer = new Timer();
        private GameTime currentTime;
        private Game currentGame;
        private int lastUsedId;

        public GameCalculator()
        {
        }

        public async Task Start()
        {
            // TODO try to load from cloud
            firebaseClient = new FirebaseClient("https://tritor-game-launcher.firebaseio.com/");
            Games = new List<Game>();
            gamesUpdated = new List<Game>();
            // update last usedId (if game.id > lastusedid


            Game starcraft = new Game(0, "Starcraft", "shortcut//StarCraft II", "Resources//SC2.png",
                new List<KeyValuePair<DateTime, GameTime>>());
            Game hearhstone = new Game(1, "Hearthstone", "shortcut//LancerHearthstone.bat",
                "Resources//HearthStone.jpeg", new List<KeyValuePair<DateTime, GameTime>>());
            Games.Add(starcraft); //"Resources/controllerRezised.png"
            Games.Add(hearhstone);
            lastUsedId = 1;
            gamesUpdated.Add(starcraft);
            gamesUpdated.Add(hearhstone);

            //Run().Wait();
            //var task = Run();
            //task.ConfigureAwait(false);
            //task.Wait();
            await Task.Run(Run);


            gameTimer.Interval = OneMinuteInMs;
            gameTimer.Elapsed += OnTimedEvent;
        }


        private async Task Run()
        //private async void Run()
        {
            //var dinos = await firebaseClient
            //.Child("Games")
            //.OrderByKey().OnceAsync<Game>();
            var task = await firebaseClient.Child("Games").PostAsync(new Game()).ConfigureAwait(false);
            Games.Add(task.Object);
            //var task = firebaseClient.Child("Games").PostAsync(new Game());
            //task.Wait(1000);
            //Games.Add(task.Result.Object);
            //var task2 = await firebaseClient.Child("Games").PostAsync(Games[0]);
            //task.
            //task.RunSynchronously();
            //task.Start();
            //task.Wait();
            //var testGame = task.Result;
            //await firebaseClient
            //    .Child("dinosaurs")
            //    .Child("t-rex")
            //    .PutAsync(new Game());
            //var dinos = await firebaseClient
            //    .Child("dinosaurs")
            //    .OrderByKey()
            //    .StartAt("pterodactyl")
            //    .LimitToFirst(2)
            //    .OnceAsync<Game>();
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
                    currentTime = new GameTime();
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
            // Create a timestamp with the date of the time played
            currentGame?.GameTimes.Add(new KeyValuePair<DateTime, GameTime>(DateTime.Now, currentTime)); // Maybe we should push this new time directly to the database
        }

        public void SaveData()
        {
            // TODO Push the data to the cloud
            // Probably only push the games information (name, url, etc)
            foreach (Game game in gamesUpdated)
            {

            }
        }

        public Game CreateANewGame()
        {
            ++lastUsedId;
            Game game = new Game(lastUsedId);
            Games.Add(game);
            return game;
        }

        public void NotifyGameInformationUpdated(Game game)
        {
            gamesUpdated.Add(game);
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            currentTime.NbMinutes++;
        }
    }
}
