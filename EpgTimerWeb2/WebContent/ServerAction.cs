using EpgTimerWeb2.LiveWATCH;
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
using EpgTimerWeb2.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace EpgTimer
{
    public class ServerAction
    {

        private static void LoginURL(HttpContext Info)
        {
            Info.Response.Headers["Cache-Control"] = "no-cache";
            if (CheckCookie(Info))
            {
                HttpContext.Redirect(Info, "/");
                return;
            }
            Info.Response.Headers["Content-Type"] = "text/html";
            if (Info.Request.Method.ToUpper() == "POST")
            {
                DoLoginURL(Info);
            }
            else
            {
                string Login = Resources.Login;
                if (Info.Request.QueryStringRaw.ToLower() == "error")
                    Login = Login.Replace("<!--INSERT_MESSAGE_HERE--!>", "<div class='alert alert-danger' role='alert'>ログイン失敗</div>");
                else if (Info.Request.QueryStringRaw.ToLower() == "logout")
                    Login = Login.Replace("<!--INSERT_MESSAGE_HERE--!>", "<div class='alert alert-info' role='alert'>ログアウト済み</div>");
                HttpContext.SendResponse(Info, Login);
            }
        }
        private static void DoLoginURL(HttpContext Info)
        {
            var Param = HttpRequest.ParseQueryString(Info.Request.PostString);
            if (!Param.ContainsKey("user") || !Param.ContainsKey("pass"))
            {
                HttpContext.Redirect(Info, "/login?error");
                return;
            }
            HttpSession Session = new HttpSession(Param["user"], Param["pass"], Info.IpAddress);
            if (!Session.CheckAuth(Session.SessionKey, Info.IpAddress))
            {
                HttpContext.Redirect(Info, "/login?error");
                return;
            }
            Info.Response.Headers["Set-Cookie"] = Cookie.Generate(new Cookie(){
                {"session", Session.SessionKey}
            });
            HttpContext.Redirect(Info, "/");
        }
        private static bool CheckCookie(HttpContext Info)
        {
            if (Info.Request.Cookie.ContainsKey("session")
                && HttpSession.IsMatch(Info.Request.Cookie["session"], Info.IpAddress))
                return true;
            else
                return false;
        }
        private static void LogoutURL(HttpContext Info)
        {
            if (!CheckCookie(Info))
            {
                HttpContext.Redirect(Info, "/login?error");
                return;
            }
            HttpSession.Search(Info.Request.Cookie["session"], Info.IpAddress).LastTime = 0;
            Info.Response.Headers["Cache-Control"] = "no-cache";
            Cookie removeCookie = new Cookie(){
                {"session", "delete"}
            };
            removeCookie.Expire = DateTime.Now.AddYears(-5);
            Info.Response.Headers["Set-Cookie"] = Cookie.Generate(removeCookie);
            HttpContext.Redirect(Info, "/login?logout");
        }
        static Regex r = new Regex(@"^\/api\/(.*)\/json\/(\w*)\/$");
        static Regex r1 = new Regex(@"^\/api\/(.*)$");
        static Regex live = new Regex(@"^\/live\/(\w*)$");
        public static void DoProcess(HttpContext Info)
        {
            try
            {
                if (Info.Request.Url == "/index.htm") Info.Request.Url = "/index.html";
                if (Info.Request.Url == "/") Info.Request.Url = "/index.html";
                bool IsAuth = CheckCookie(Info);
                bool NeedsAuth = !string.IsNullOrEmpty(Setting.Instance.LoginUser) && 
                    !string.IsNullOrEmpty(Setting.Instance.LoginPassword);
                Debug.Print(Info.Request.RawUrl);
                if (PrivateSetting.Instance.SetupMode)
                {
                    Setup.SetupProcess(Info);
                    return;
                }
                else if (Info.Request.Url.ToLower() == "/logout" && NeedsAuth)
                {
                    LogoutURL(Info);
                }
                else if (Info.Request.Url.ToLower() == "/login" && NeedsAuth)
                {
                    LoginURL(Info);
                }
                else if (Info.Request.Url.ToLower() == "/index.html" && NeedsAuth && !IsAuth)
                {
                    Info.Response.Headers["Cache-Control"] = "no-cache";
                    HttpContext.Redirect(Info, "/login");
                }
                else if (new HttpContent().RequestUrl(Info, !NeedsAuth || IsAuth))
                {
                }
                else
                {
                    if (NeedsAuth && !IsAuth) throw new HttpResponseException(401, "Unauthorized", "");
                    if (Info.Request.Url.ToLower() == "/resource")
                    {
                        Info.Response.Headers["Content-Type"] = "text/javascript; charset=utf8";
                        WebCache.CachedResponse(Info, (Info2) =>
                        {
                            string cb = "if(typeof(ETW)==='undefined')ETW={};\nETW.Resource=" + JsonUtil.Serialize(CommonManagerJson.Instance, false) + ";";
                            cb += "\nETW.Resource.NeedsAuth=" + (NeedsAuth ? "true" : "false") + ";\n";
                            return cb;
                        });
                    }
                    else if (Info.Request.Url.ToLower() == "/ws") //WebSocket
                    {
                        Info.Response.Headers["Cache-Control"] = "no-cache";

                        SocketAction.Process(Info);
                    }
                    else if (r1.IsMatch(Info.Request.Url))
                    {
                        Info.Response.Headers["Content-Type"] = "text/javascript; charset=utf8";
                        ApiCache.CachedResponse(Info, (Info2) =>
                        {
                            return Api.Call(r1.Match(Info2.Request.Url).Groups[1].Value, Info2.Request.QueryString, false);
                        });
                    }
                    else if (live.IsMatch(Info.Request.Url))
                    {
                        WatchURLStarter.Handler(Info);
                    }
                    else
                    {
                        throw new HttpResponseException(404, "Not Found", "File not found");
                    }
                }
            }
            catch (HttpResponseException http)
            {
                Info.Response.SetStatus(http.StatusCode, http.StatusText);
                HttpResponse.StatusPage(Info, http.Reason);
                Info.Response.Send();
            }
            catch (Exception ex)
            {
                byte[] NotFound = Encoding.UTF8.GetBytes(@"<html>
<body>
<h1>500</h1><p>詳細: " + ex.Message + @"</p>
<hr />
EpgTimerWeb(v2) by YUKI
</body>
</html>");
                Info.Response.SetStatus(500, "Internal Server Error");
                Info.Response.OutputStream.Write(NotFound, 0, NotFound.Length);
                Info.Response.Send();
                Console.WriteLine("\n!!!! Exception !!!!");
            }

            Info.Close();
        }

    }
}
