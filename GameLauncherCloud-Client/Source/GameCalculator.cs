using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncherCloud_Client
{
    class GameCalculator
    {
        public List<Game> games;

        public GameCalculator()
        {
            // TODO try to load from cloud
            games = new List<Game>();

            games.Add(new Game("Starcraft"));
            games.Add(new Game("Hearthstone"));
        }

        public string LaunchGame(Game game)
        {
            string errorMessage = null;
            if (!string.IsNullOrWhiteSpace(game.Url))
            {
                try
                {
                    System.Diagnostics.Process.Start(game.Url);
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
        }

        public void SaveData()
        {
            // TODO Push the data to the cloud
        }
    }
}
