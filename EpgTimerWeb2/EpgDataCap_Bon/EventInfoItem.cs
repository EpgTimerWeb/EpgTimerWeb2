/*
 *  EpgTimerWeb2
 *  Copyright (C) 2016 EpgTimerWeb <webmaster@epgtimerweb.net>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
namespace EpgTimer
{
    public class EventInfoItem
    {
        private EpgEventInfo EventInfo = null;
        public EventInfoItem(EpgEventInfo EventInfo)
        {
            this.EventInfo = EventInfo;
        }
        public DateTime Start
        {
            get
            {
                if (EventInfo.IsStartTime != 1) return DateTime.MinValue;
                return EventInfo.StartTime;
            }
        }

        public DateTime End
        {
            get
            {
                if (EventInfo.IsStartTime != 1 || EventInfo.IsDuration != 1) return DateTime.MinValue;
                return Start.AddSeconds(EventInfo.DurationSec);
            }
        }
         
        public ushort ONID
        {
            get
            {
                return EventInfo.ONID;
            }
        }

        public ushort TSID
        {
            get
            {
                return EventInfo.TSID;
            }
        }

        public ushort SID
        {
            get
            {
                return EventInfo.SID;
            }
        }

        public ushort EventID
        {
            get
            {
                return EventInfo.EventID;
            }
        }

        public string Key
        {
            get { return CommonManager.Create64PgKey(ONID, TSID, SID, EventID).ToString(); }
        }

        public byte FreeCA
        {
            get
            {
                return EventInfo.FreeCA;
            }
        }

        public EpgComponentInfo Component
        {
            get
            {
                return EventInfo.ComponentInfo;
            }
        }

        public EpgAudioComponentInfo Audio
        {
            get
            {
                return EventInfo.AudioInfo;
            }
        }

        public EpgContentInfo Content
        {
            get
            {
                return EventInfo.ContentInfo;
            }
        }

        public EpgEventGroupInfo EventGroup
        {
            get
            {
                return EventInfo.EventGroupInfo;
            }
        }

        public EpgEventGroupInfo EventRelay
        {
            get
            {
                return EventInfo.EventRelayInfo;
            }
        }

        public EpgExtendedEventInfo Ext
        {
            get
            {
                return EventInfo.ExtInfo;
            }
        }

        public EpgShortEventInfo Short
        {
            get
            {
                return EventInfo.ShortInfo;
            }
        }
    }
}
