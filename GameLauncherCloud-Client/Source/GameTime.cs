using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameLauncherCloud_Client
{
    public class GameTime
    {
        public GameTime() : this(0)
        {
        }

        public GameTime(int nbMinutes)
        {
            NbMinutes = nbMinutes;
        }

        public bool GreaterThanZero()
        {
            return NbMinutes > 0;
        }

        public const int NbMinutesInHour = 60;
        public const int NbHoursInDay = 24;

        [JsonProperty(PropertyName = "NbMinutes")]
        public int NbMinutes
        {
            get;
            set;
        }

        [JsonIgnore]
        public int NbHours => NbMinutes / NbMinutesInHour;

        [JsonIgnore]
        public int NbDays => NbHours / NbHoursInDay;


        public static bool operator <(GameTime left, GameTime right)
        {
            return left.NbMinutes < right.NbMinutes;
        }

        public static bool operator >(GameTime left, GameTime right)
        {
            return left.NbMinutes > right.NbMinutes;
        }

        public static GameTime operator -(GameTime a, GameTime b)
        {
            return new GameTime(a.NbMinutes - b.NbMinutes);
        }
    }
}
