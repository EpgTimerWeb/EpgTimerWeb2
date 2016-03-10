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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace EpgTimer
{
    public class PrivateSetting
    {
        private static PrivateSetting _instance;
        public static PrivateSetting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PrivateSetting();
                return _instance;
            }
            set { _instance = value; }
        }
        public PrivateSetting()
        {
            Sessions = new List<HttpSession>();
            CmdConnect = new CtrlCmdConnect();
            Server = null;
            SetupMode = false;
            ConfigPath = "EpgTimerWeb2.xml";
            SetupCode = "";
        }
        public List<HttpSession> Sessions { set; get; }
        public CtrlCmdConnect CmdConnect { get; set; }
        public WebServer Server { get; set; }
        public bool SetupMode { set; get; }
        public string ConfigPath { set; get; }
        public string SetupCode { set; get; }
    }
    public class ContentColorItem : IEquatable<ContentColorItem>
    {
        public uint ContentLevel1 { set; get; }
        public string Color { set; get; }
        public ContentColorItem()
        {
            ContentLevel1 = 15;
            Color = "#f0f0f0";
        }
        public ContentColorItem(uint Content, string color)
        {
            Color = color;
            ContentLevel1 = Content;
        }
        public override int GetHashCode()
        {
            return this.ContentLevel1.GetHashCode();
        }
        bool IEquatable<ContentColorItem>.Equals(ContentColorItem obj)
        {
            if (obj == null) return false;
            return obj.ContentLevel1 == this.ContentLevel1;
        }
    }

    public class Setting
    {
        [NonSerialized]
        private static Setting _instance;
        [XmlIgnore]
        public static Setting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Setting();
                return _instance;
            }
            set { _instance = value; }
        }
        public Setting()
        {
            CtrlHost = "127.0.0.1";
            CtrlPort = 4510;
            CallbackPort = 4521;
            HttpPort = 8080;
            LocalMode = false;
            LoginUser = "";
            LoginPassword = "";
            ContentToColorTable = new List<ContentColorItem>(){
                new ContentColorItem(0, "#ffffe0"),
                new ContentColorItem(1, "#e0e0ff"),
                new ContentColorItem(2, "#ffe0f0"),
                new ContentColorItem(3, "#ffe0e0"),
                new ContentColorItem(4, "#e0ffe0"),
                new ContentColorItem(5, "#e0ffff"),
                new ContentColorItem(6, "#fff0e0"),
                new ContentColorItem(7, "#ffe0ff"),
                new ContentColorItem(8, "#ffffe0"),
                new ContentColorItem(9, "#fff0e0"),
                new ContentColorItem(10, "#e0f0ff"),
                new ContentColorItem(11, "#e0f0ff"),
                new ContentColorItem(15, "#f0f0f0")
            };
            MaxUploadSize = 1024 * 1024 * 3;
            SessionExpireSecond = 60 * 60 * 1;
            Presets = new List<PresetClass>();
            Views = new List<ViewClass>();
        }

        public string CtrlHost { get; set; }
        public UInt32 CtrlPort { get; set; }
        public UInt32 CallbackPort { get; set; }
        public UInt32 HttpPort { get; set; }
        public bool LocalMode { get; set; }
        public string LoginUser { set; get; }
        public string LoginPassword { set; get; }
        public List<ContentColorItem> ContentToColorTable { set; get; }
        public long MaxUploadSize { set; get; }
        public long SessionExpireSecond { set; get; }
        public List<PresetClass> Presets { set; get; }
        public List<ViewClass> Views { set; get; }

        public static void SaveToXmlFile(string path)
        {
            try
            {
                Instance.ContentToColorTable = Instance.ContentToColorTable.Distinct().ToList();
                FileStream fs = new FileStream(path,
                    FileMode.Create,
                    FileAccess.Write, FileShare.None);
                XmlSerializer xs = new XmlSerializer(typeof(Setting));
                xs.Serialize(fs, Instance);
                fs.Close();
            }
            catch (Exception ex)
            {
                Debug.Print("Config Error: {0}", ex.Message);
                //MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        public static void LoadFromXmlFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return;
                FileStream fs = new FileStream(path,
                   FileMode.Open,
                   FileAccess.Read);
                XmlSerializer xs = new XmlSerializer(typeof(Setting));
                object obj = xs.Deserialize(fs);
                fs.Close();
                Instance = (Setting)obj;
            }
            catch (Exception ex)
            {
                Debug.Print("Config Error: {0}", ex.Message);
            }
            finally
            {
                if (Instance.ContentToColorTable == null) Instance.ContentToColorTable = new List<ContentColorItem>();
                if (Instance.ContentToColorTable.Count == 0)
                {
                    Instance.ContentToColorTable = new List<ContentColorItem>(){
                        new ContentColorItem(0, "#ffffe0"),
                        new ContentColorItem(1, "#e0e0ff"),
                        new ContentColorItem(2, "#ffe0f0"),
                        new ContentColorItem(3, "#ffe0e0"),
                        new ContentColorItem(4, "#e0ffe0"),
                        new ContentColorItem(5, "#e0ffff"),
                        new ContentColorItem(6, "#fff0e0"),
                        new ContentColorItem(7, "#ffe0ff"),
                        new ContentColorItem(8, "#ffffe0"),
                        new ContentColorItem(9, "#fff0e0"),
                        new ContentColorItem(10, "#e0f0ff"),
                        new ContentColorItem(11, "#e0f0ff"),
                        new ContentColorItem(15, "#f0f0f0")
                    };
                }
                if (Instance.Presets == null) Instance.Presets = new List<PresetClass>();
                if (Instance.Views == null) Instance.Views = new List<ViewClass>();
                if (Instance.MaxUploadSize == 0) Instance.MaxUploadSize = 1024 * 1024 * 3;
                if (Instance.SessionExpireSecond == 0) Instance.SessionExpireSecond = 60 * 60 * 1;
                if (Instance.CtrlPort == 0) Instance.CtrlPort = 4510;
                if (Instance.CallbackPort == 0) Instance.CallbackPort = 4521;
                if (Instance.HttpPort == 0) Instance.HttpPort = 8080;
                Instance.ContentToColorTable = Instance.ContentToColorTable.Distinct().ToList();
            }
        }
    }
}
