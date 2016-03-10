using EpgTimer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpgTimerWeb2.LiveWATCH
{
    public class WatchSession
    {
        public static Dictionary<string, WatchSession> Sessions { set; get; }
        public enum SessionType : uint
        {
            SESSION_FILE_VIEW = 0,
            SESSION_LIVE_VIEW = 1,
            SESSION_TIMESHIFT = 2
        }
        public string WatchID { set; get; }
        public SessionType Type { set; get; }
        public string FileName { set; get; }
        public EpgServiceInfo Service { set; get; }
        public uint ReserveID { set; get; }
        public uint ControlID { set; get; }
        public NWPlayTimeShiftInfo TimeShiftInfo { set; get; }
    }
}
