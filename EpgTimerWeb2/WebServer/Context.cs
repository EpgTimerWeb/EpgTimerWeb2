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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EpgTimer
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { set; get; }
        public string StatusText { set; get; }
        public string Reason { set; get; }
        public HttpResponseException(int code, string text)
        {
            StatusCode = code;
            StatusText = text;
            Reason = "Something...";
        }

        public HttpResponseException(int code, string text, string reason)
        {
            StatusCode = code;
            StatusText = text;
            Reason = reason;
        }
    }
    public class HttpContext
    {
        public HttpContext(TcpClient client)
        {
            Client = client;
            Response = new HttpResponse(this);
            IpAddress = "0.0.0.0";
            LockObject = new object();
            Request = HttpRequest.Parse(this.HttpStream);
            IpAddress = ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString();
            if(Request.Headers.ContainsKey("Accept-Encoding")
                && Request.Headers["Accept-Encoding"].IndexOf("gzip") >= 0){
                    Response.UseGZip = true;
            }
        }
        public HttpRequest Request { set; get; }
        public HttpResponse Response { set; get; }
        public string IpAddress { set; get; }
        private TcpClient _Client;
        public object LockObject { set; get; }
        public TcpClient Client
        {
            get
            {
                return _Client;
            }
            set
            {
                _Client = value;
                HttpStream = _Client.GetStream();
            }
        }
        public Stream HttpStream {set;get;}

        public void Close()
        {
            HttpStream.Flush();
            HttpStream.Close();
            Request = null;
            Response = null;
            Client.Close();
        }
        public static void SendResponse(HttpContext Context, byte[] Bytes)
        {
            Context.Response.OutputStream.Write(Bytes, 0, Bytes.Length);
            Context.Response.Send();
        }
        public static void SendResponse(HttpContext Context, string Str)
        {
            SendResponse(Context, Encoding.UTF8.GetBytes(Str));
        }
        public static void SendFile(HttpContext Context, string Path)
        {
            DateTime LastModified = File.GetLastWriteTimeUtc(Path);
            if (Context.Request.Headers.ContainsKey("If-Modified-Since"))
            {
                if (Context.Request.Headers["If-Modified-Since"] == LastModified.ToString("R"))
                {
                    Context.Response.SetStatus(304, "Not Modified");
                    Context.Response.Send();
                    return;
                }
            }
            Context.Response.Headers.Add("Last-Modified", LastModified.ToString("R"));

            HttpContext.SendResponse(Context, File.ReadAllBytes(Path));
        }
        public static void Redirect(HttpContext Context, string Url)
        {
            var Domain = "localhost:8080";
            if (Context.Request.Headers.ContainsKey("host"))
            {
                Domain = Context.Request.Headers["host"];
            }
            Context.Response.StatusCode = 301;
            Context.Response.StatusText = "Moved Permanently";
            Context.Response.Headers["Location"] = "http://" + Domain + Url;
            Context.Response.Send();
        }
    }
}
