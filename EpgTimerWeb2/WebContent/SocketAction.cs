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
using System.Text;
using System.Text.RegularExpressions;

namespace EpgTimer
{
    public class SocketAction
    {
        static List<HttpContext> Sockets = new List<HttpContext>();
        static Regex r2 = new Regex(@"^([^ ]*) (.*)$");
        public static void Process(HttpContext Info)
        {
            Sockets.Add(Info);
            WebSocket.EventLoop(Info, UnMask =>
            {
                Debug.Print(UnMask);
                if (r2.IsMatch(UnMask))
                {
                    var match = r2.Match(UnMask);
                    string Command = match.Groups[2].Value;
                    string Id = match.Groups[1].Value;
                    string Arg = "";
                    if (Command.IndexOf("?") > 0)
                    {
                        Arg = Command.Substring(Command.IndexOf("?") + 1);
                        Command = Command.Substring(0, Command.IndexOf("?"));
                    }
                    string JsonData = Api.Call(Command, HttpRequest.ParseQueryString(Arg), false).JsonData;
                    byte[] Response = WebSocket.Mask(
                                Encoding.UTF8.GetBytes("ERR No API"), 0x1);
                    if (JsonData != "")
                    {
                        Response = Encoding.UTF8.GetBytes("+OK" + Id + " " + JsonData);
                    }
                    HttpResponse.SendResponseBody(Info, WebSocket.Mask(
                                    Response, 0x1));
                }
                else
                {
                    byte[] Response = WebSocket.Mask(
                                Encoding.UTF8.GetBytes("ERR"), 0x1);
                    HttpResponse.SendResponseBody(Info, Response);
                }
            });
            Sockets.Remove(Info);
        }
        public static void SendAllMessage(string Mes)
        {
            try
            {
                byte[] Response = WebSocket.Mask(
                                Encoding.UTF8.GetBytes(Mes), 0x1);
                var a = Sockets.ToArray();
                foreach (var Con in a)
                {
                    if (!Con.Client.Connected) Sockets.Remove(Con);
                    HttpResponse.SendResponseBody(Con, Response);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("SocketAction Error: {0}", ex.Message);
            }
        }
    }
}
