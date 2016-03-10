using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpgTimerWeb
{
    public class WebSocketManager
    {
        private static List<HttpContext> ConnectedSession { get; set; }
        public void AddSession(HttpContext Context){
            ConnectedSession.Add(Context);
        }
    }
}
