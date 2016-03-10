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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EpgTimer
{
    public class HttpHeaderArray : IEnumerable<KeyValuePair<string, string>>
    {
        private List<KeyValuePair<string, string>> _items = null;
        public HttpHeaderArray()
        {
            _items = new List<KeyValuePair<string, string>>();
        }
        public string this[string key]
        {
            get
            {
                if (_items.Count(s => s.Key.ToLower() == key.ToLower()) > 0)
                    return _items.First(s => s.Key.ToLower() == key.ToLower()).Value;
                throw new KeyNotFoundException();
            }
            set
            {
                _items.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        public bool ContainsKey(string key)
        {
            return (_items.Count(s => s.Key.ToLower() == key.ToLower()) > 0);
        }
        public void Add(string Key, string Value)
        {
            _items.Add(new KeyValuePair<string, string>(Key, Value));
        }

        public static string Generate(HttpHeaderArray Input)
        {
            StringBuilder Ret = new StringBuilder();
            foreach (KeyValuePair<string, string> Item in Input)
            {
                Ret.AppendFormat("{0}: {1}\r\n", Item.Key, Item.Value);
            }
            return Ret.ToString();
        }
        public static HttpHeaderArray Parse(Stream Input)
        {
            string Line = "";
            HttpHeaderArray Dict = new HttpHeaderArray();
            while ((Line = HttpCommon.StreamReadLine(Input)) != null)
            {
                if (Line == "") return Dict;
                int Separator = Line.IndexOf(":");
                if (Separator == -1) throw new Exception("Invalid Http Header " + Line);
                var Name = Line.Substring(0, Separator);
                if (ConfigurationManager.AppSettings["DEBUG"] != null)
                    Console.WriteLine("Header: {0}", Line);
                Dict[Name] = Util.RemoveStartSpace(Line.Substring(Separator + 1));
            }
            return Dict;
        }
    }

}
