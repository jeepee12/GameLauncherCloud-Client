using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncherCloud_Client
{
    class Game
    {
        public int GameId;
        public string Name;
        public string Url;
        public string ImageUrl;
        public GameTime Time;

        public Game(int id) : this(id, "Game name", "", "", new GameTime())
        {
        }

        public Game(int id, string name) : this(id, name, "", "", new GameTime())
        {
        }

        public Game(int id, string name, string url, string imageUrl, GameTime time)
        {
            GameId = id;
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
            Time = time;
        }
    }
}
