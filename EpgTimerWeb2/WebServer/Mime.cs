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
using Microsoft.Win32;

namespace EpgTimer
{
    public class Mime
    {
        public static bool IsImage(string path)
        {
            if (path.IndexOf(".") < 0) return false;
            var split = path.Split(new char[] { '.' });
            var ext = split[split.Length - 1];
            switch (ext.ToLower())
            {
                case "png":
                case "jpg":
                case "bmp":
                case "ico":
                    return true;
                default:
                    return false;
            }
        }
        public static string Get(string path, string mimeProposed)
        {
            if (path.IndexOf(".") < 0) return mimeProposed;
            var split = path.Split(new char[] { '.' });
            var ext = split[split.Length - 1];
            if (ext == "css") return "text/css";
            if (ext == "html") return "text/html";
            if (ext == "js") return "text/javascript";
            var key = Registry.ClassesRoot.OpenSubKey("." + ext);
            if (key == null)
                return mimeProposed;
            var mime = key.GetValue("Content Type");
            if (mime == null)
                return mimeProposed;
            else
                return (string)mime;
        }
    }
}
