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
    }
}
