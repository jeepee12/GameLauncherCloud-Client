using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncherCloud_Client
{
    class Game
    {
        public string Name;
        public string Url;
        public string ImageUrl;
        public GameTime Time;

        public Game() : this("Game name", "", "", new GameTime())
        {
        }

        public Game(string name) : this(name, "", "", new GameTime())
        {
        }

        public Game(string name, string url, string imageUrl, GameTime time)
        {
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
            Time = time;
        }
    }
}
