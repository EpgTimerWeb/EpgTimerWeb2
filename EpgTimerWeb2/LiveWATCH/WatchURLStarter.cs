using EpgTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpgTimerWeb2.LiveWATCH
{
    public class WatchURLStarter
    {
        public static void Handler(HttpContext Context)
        {
            if (Context.Request.Url.StartsWith("/video/"))
            {

            }
        }
    }
}
