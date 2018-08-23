using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameLauncherCloud_Client
{
    public class Game
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name;
        [JsonProperty(PropertyName = "Url")]
        public string Url;
        [JsonProperty(PropertyName = "ImageUrl")]
        public string ImageUrl;
        [JsonProperty(PropertyName = "IsSteamGame")]
        public bool IsSteamGame;

        [JsonProperty(PropertyName = "Gametimes")]
        public List<KeyValuePair<DateTime, GameTime>> GameTimes;

        public Game() : this("Game name", "", "", new List<KeyValuePair<DateTime, GameTime>>())
        {
        }

        public Game(string name) : this(name, "", "", new List<KeyValuePair<DateTime, GameTime>>())
        {
        }

        public Game(string name, string url, GameTime totalGameTime, bool isSteamGame) : this(name, url, "", new List<KeyValuePair<DateTime, GameTime>>(){ new KeyValuePair<DateTime, GameTime>(DateTime.Now, totalGameTime)}, isSteamGame)
        {
        }

        public Game(string name, string url, string imageUrl, List<KeyValuePair<DateTime, GameTime>> gameTimes) : this(name, url, imageUrl, gameTimes, false)
        {
        }

        public Game(string name, string url, string imageUrl, List<KeyValuePair<DateTime, GameTime>> gameTimes, bool isSteamGame)
        {
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
            GameTimes = gameTimes;
            IsSteamGame = isSteamGame;
        }

        public GameTime CalculateTotalTime()
        {
            GameTime total = new GameTime();
            foreach (KeyValuePair<DateTime, GameTime> pair in GameTimes)
            {
                total.NbMinutes += pair.Value.NbMinutes;
            }

            return total;
        }
    }
}
