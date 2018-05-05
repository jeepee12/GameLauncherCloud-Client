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
        [JsonProperty(PropertyName = "GameId")]
        public int GameId;
        [JsonProperty(PropertyName = "Name")]
        public string Name;
        [JsonProperty(PropertyName = "Url")]
        public string Url;
        [JsonProperty(PropertyName = "ImageUrl")]
        public string ImageUrl;

        [JsonProperty(PropertyName = "Gametimes")]
        public List<KeyValuePair<DateTime, GameTime>> GameTimes;

        public Game() : this(0, "Game name", "", "", new List<KeyValuePair<DateTime, GameTime>>())
        {
        }

        public Game(int id) : this(id, "Game name", "", "", new List<KeyValuePair<DateTime, GameTime>>())
        {
        }

        public Game(int id, string name) : this(id, name, "", "", new List<KeyValuePair<DateTime, GameTime>>())
        {
        }

        public Game(int id, string name, string url, string imageUrl, List<KeyValuePair<DateTime, GameTime>> gameTimes)
        {
            GameId = id;
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
            GameTimes = gameTimes;
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
