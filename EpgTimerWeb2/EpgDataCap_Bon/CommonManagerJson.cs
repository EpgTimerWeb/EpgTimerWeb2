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
using System.Collections.Generic;

namespace EpgTimer
{
    public class CommonManagerJson
    {
        private static CommonManagerJson _instance;
        public static CommonManagerJson Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CommonManagerJson();
                return _instance;
            }
            set { _instance = value; }
        }
        public Dictionary<ushort, ContentKindInfo> ContentKindDictionary { set; get; }
        public Dictionary<ushort, ContentKindInfo> ContentKindDictionary2 { set; get; }
        public Dictionary<ushort, ComponentKindInfo> ComponentKindDictionary { set; get; }
        public CommonManagerJson()
        {
            ContentKindDictionary = CommonManager.Instance.ContentKindDictionary;
            ContentKindDictionary2 = CommonManager.Instance.ContentKindDictionary2;
            ComponentKindDictionary = CommonManager.Instance.ComponentKindDictionary;
        }
    }
}
