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
using System.Collections.Generic;

namespace EpgTimer
{
    public class EventStore
    {
        private static EventStore _instance;
        public static EventStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventStore();
                return _instance;
            }
            set { _instance = value; }
        }
        private List<NotifySrvInfoItem> _events = null;
        public List<NotifySrvInfoItem> Events { get { return _events; } }
        public EventStore()
        {
            _events = new List<NotifySrvInfoItem>();
        }
        public void AddMessage(NotifySrvInfoItem item)
        {
            _events.Add(item);
        }
    }
}
