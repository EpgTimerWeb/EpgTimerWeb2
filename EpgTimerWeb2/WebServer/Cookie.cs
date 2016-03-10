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
using System.Linq;

namespace EpgTimer
{
    public class Cookie : Dictionary<string, string>
    {
        private static string GetExpireHeader(DateTime Time)
        {
            return Time.ToString("R");
        }
        private static string[] SplitCookieKVPair(string Cookie)
        {
            return Cookie.Split(';').Select(s => Util.RemoveStartSpace(s)).ToArray();
        }
        public DateTime Expire { set; get; }
        public string Path { set; get; }
        public Cookie(bool IsSession = true)
        {
            if (!IsSession)
                Expire = DateTime.Now.AddYears(1);
            else
                Expire = DateTime.MinValue;
            Path = "/";
        }
        public static string Generate(Cookie cookie)
        {
            cookie["path"] = cookie.Path;
            if (cookie.Expire != DateTime.MinValue)
                cookie["expires"] = GetExpireHeader(cookie.Expire);
            string Cookie = "";
            foreach (var item in cookie)
            {
                string line = item.Key + "=" + item.Value;
                if (Cookie != "")
                    Cookie += "; ";
                Cookie += line;
            }
            return Cookie;
        }
        public static Cookie Parse(string Input)
        {
            Cookie cookie = new Cookie();
            foreach (string item in SplitCookieKVPair(Input))
            {
                if (item.IndexOf("=") < 0) continue;
                string name = item.Substring(0, item.IndexOf("="));
                string value = item.Substring(item.IndexOf("=") + 1);
                cookie[name] = value;
            }
            return cookie;
        }
    }
}
