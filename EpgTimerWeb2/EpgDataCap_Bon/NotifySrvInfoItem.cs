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
    public class NotifySrvInfoItem
    {
        public NotifySrvInfo NotifyInfo
        {
            get;
            set;
        }
        public DateTime Time
        {
            get
            {
                return NotifyInfo.time;
            }
        }
        public string Title
        {
            get
            {
                if (NotifyInfo != null)
                {
                    switch ((UpdateNotifyItem)NotifyInfo.notifyID)
                    {
                        case UpdateNotifyItem.PreRecStart:
                            return "予約録画開始準備";
                        case UpdateNotifyItem.RecStart:
                            return "録画開始";
                        case UpdateNotifyItem.RecEnd:
                            return "録画終了";
                        case UpdateNotifyItem.RecTuijyu:
                            return "追従発生";
                        case UpdateNotifyItem.ChgTuijyu:
                            return "番組変更";
                        case UpdateNotifyItem.PreEpgCapStart:
                            return "EPG取得";
                        case UpdateNotifyItem.EpgCapStart:
                            return "EPG取得";
                        case UpdateNotifyItem.EpgCapEnd:
                            return "EPG取得";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        public string LogText
        {
            get
            {
                if (NotifyInfo != null)
                {
                    switch ((UpdateNotifyItem)NotifyInfo.notifyID)
                    {
                        case UpdateNotifyItem.PreRecStart:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.RecStart:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.RecEnd:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.RecTuijyu:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.ChgTuijyu:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.PreEpgCapStart:
                            return NotifyInfo.param4;
                        case UpdateNotifyItem.EpgCapStart:
                            return "開始";
                        case UpdateNotifyItem.EpgCapEnd:
                            return "終了";
                        default:
                            return NotifyInfo.notifyID.ToString();
                    }
                }
                return "";
            }
        }

        public string FileLogText
        {
            get
            {
                if (NotifyInfo != null)
                {
                    return NotifyInfo.time.ToString("yyyy/MM/dd HH:mm:ss.fff") + 
                        " [" + Title + "] " + LogText + "\n";
                }
                return "";
            }
        }
    }
}
