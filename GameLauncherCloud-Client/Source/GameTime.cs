using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncherCloud_Client
{
    class GameTime
    {
        int NbMinutes;

        public int NbHours => NbMinutes / 60;

        public int NbDays => NbHours / 24;
    }
}
