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
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace EpgTimer
{
    public class HttpRequest
    {
        public string Method { get; set; }
        public string RawUrl { get; set; }
        public string Url { get; set; }
        public string QueryStringRaw { get; set; }
        public byte[] PostData { get; set; }
        public string HttpVersion { get; set; }
        public HttpHeaderArray Headers { get; set; }
        public Cookie Cookie { get; set; }
        public Dictionary<string, string> QueryString { set; get; }

        public HttpRequest()
        {
            QueryString = new Dictionary<string, string>();
            PostData = new byte[0];
            Cookie = new Cookie();
            Url = "";
            RawUrl = "";
            QueryStringRaw = "";
        }

        public string PostString
        {
            get { return PostData.Length == 0 ? "" : Encoding.UTF8.GetString(PostData); }
        }
        public static Dictionary<string, string> ParseQueryString(string QueryString)
        {
            Dictionary<string, string> Arg = new Dictionary<string, string>();
            if (QueryString.StartsWith("?")) QueryString = QueryString.Substring(1);
            foreach (var ArgTemp in QueryString.Split('&'))
            {
                if (ArgTemp.IndexOf("=") > 0 && ArgTemp.IndexOf("=") + 1 < ArgTemp.Length)
                {
                    var Name = ArgTemp.Substring(0, ArgTemp.IndexOf("="));
                    var Val = ArgTemp.Substring(ArgTemp.IndexOf("=") + 1);
                    Val = Val.Replace("+", " ");
                    Arg[Name.ToLower()] = HttpUtility.UrlDecode(Val);
                }
                else
                {
                    var Name = ArgTemp.ToLower();
                    if (Name.EndsWith("=")) Name = Name.Substring(0, Name.Length - 1);
                    Arg[Name] = "";
                }
            }
            return Arg;
        }
        public static HttpRequest Parse(Stream Input)
        {
            var Start = HttpCommon.StreamReadLine(Input);
            var Request = Start.Split(' ');
            if (Request.Length != 3)
            {
                throw new Exception("Invalid Http Request " + Start);
            }
            var Headers = HttpHeaderArray.Parse(Input);
            var Res = new HttpRequest()
            {
                Method = Request[0].ToUpper(),
                RawUrl = Request[1],
                Headers = Headers,
                HttpVersion = Request[2]
            };
            if (Headers.ContainsKey("content-length")) //POSTかも
            {
                var ContentLength = 0;
                if (int.TryParse(Headers["content-length"], out ContentLength))
                {
                    if(ContentLength < 1024 * 1024 && ContentLength > 0)
                        Res.PostData = Util.ReadStream(Input, ContentLength);
                }
            }
            if (Res.RawUrl.IndexOf("?") > 0) //GETかも
            {
                Res.QueryStringRaw = Res.RawUrl.Substring(Request[1].IndexOf("?") + 1);
                Res.Url = Res.RawUrl.Substring(0, Res.RawUrl.IndexOf("?"));
                Res.QueryStringRaw = HttpUtility.UrlDecode(Res.QueryStringRaw);
                Res.QueryString = ParseQueryString(Res.QueryStringRaw);
            }
            else
            {
                Res.Url = Res.RawUrl;
            }
            Res.Url = HttpUtility.UrlDecode(Res.Url);
            
            if (Res.Headers.ContainsKey("Cookie"))
            {
                Res.Cookie = Cookie.Parse(Res.Headers["Cookie"]);
            }
            return Res;
        }
    }
}
