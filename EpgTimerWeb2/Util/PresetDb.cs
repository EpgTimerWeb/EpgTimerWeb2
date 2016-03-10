using System;
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpgTimer
{
    public class PresetClass
    {
        public int ID { set; get; }
        public string DisplayName { set; get; }
        public RecSettingData Setting { set; get; }
    }
    public class ViewClass
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Extras { set; get; }
        public EpgSearchKeyInfo Search { set; get; }
    }
    public class PresetDb
    {
        private static PresetDb _instance;
        public static PresetDb Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PresetDb();
                return _instance;
            }
            set { _instance = value; }
        }

        public PresetDb()
        {

        }
        public int AddPreset(RecSettingData Data, string Name)
        {
            if (Setting.Instance.Presets.Count(s => s.DisplayName == Name) >= 1)
            {
                return -1;
            }
            int NextID = Setting.Instance.Presets.Count == 0 ?
                0 : Setting.Instance.Presets.OrderByDescending(s => s.ID).Last().ID + 1;
            if (Setting.Instance.Presets.Count(s => s.ID == NextID) > 0) return -1;
            Setting.Instance.Presets.Add(new PresetClass()
            {
                DisplayName = Name,
                ID = NextID,
                Setting = Data
            });
            return NextID;
        }
        public int AddView(EpgSearchKeyInfo Data, string Name, string Extra)
        {
            if (Setting.Instance.Views.Count(s => s.Name == Name) >= 1)
            {
                return -1;
            }
            int NextID = Setting.Instance.Views.Count == 0 ?
                0 : Setting.Instance.Views.OrderByDescending(s => s.ID).Last().ID + 1;
            if (Setting.Instance.Views.Count(s => s.ID == NextID) > 0) return -1;
            Setting.Instance.Views.Add(new ViewClass()
            {
                ID = NextID,
                Name = Name,
                Search = Data,
                Extras = Extra
            });
            return NextID;
        }
    }
}
