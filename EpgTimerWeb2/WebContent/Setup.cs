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
using System.Text;
using System.Web;

namespace EpgTimer
{
    public class Setup
    {
        public static void SetupProcess(HttpContext Context)
        {
            if (Context.Request.Url.StartsWith("/update"))
            {
                bool OK = false;
                var Param = HttpUtility.ParseQueryString(Context.Request.PostString);
                byte[] Form;
                if (Param["code"] == PrivateSetting.Instance.SetupCode)
                {
                    try
                    {
                        if (Param["ctrlhost"] == null || Param["ctrlport"] == null ||
                            Param["cbport"] == null || Param["http"] == null ||
                            Param["user"] == null || Param["pass"] == null)
                            throw new Exception("Bad Param");
                        string Host = Param["ctrlhost"];
                        int CtrlPort = int.Parse(Param["ctrlport"]);
                        int CbPort = int.Parse(Param["cbport"]);
                        int HttpPort = int.Parse(Param["http"]);
                        if (PrivateSetting.Instance.CmdConnect.StartConnect(Host, CbPort, CtrlPort))
                        {
                            Setting.Instance.HttpPort = (uint)HttpPort;
                            Setting.Instance.CtrlHost = Host;
                            Setting.Instance.CtrlPort = (uint)CtrlPort;
                            Setting.Instance.CallbackPort = (uint)CbPort;
                            if (Param["user"] != null && Param["pass"] != null)
                            {
                                Setting.Instance.LoginUser = Param["user"];
                                Setting.Instance.LoginPassword = Param["pass"];
                            }
                            Form = Encoding.UTF8.GetBytes("<html>\n<head></head>\n<body onload=\"setTimeout(function(){location.href = 'http://' + location.hostname + ':" + HttpPort + "\';}, 1500);\">\nplease wait....\n</body>\n</html>");
                            OK = true;
                        }
                        else
                        {
                            throw new Exception("Error: Unable to connect server");
                        }
                    }
                    catch (Exception ex)
                    {
                        Form = Encoding.Default.GetBytes(ex.Message);
                    }
                }
                else
                {
                    Form = Encoding.UTF8.GetBytes("Invalid Code");
                }
                if (OK)
                {
                    Setting.SaveToXmlFile(PrivateSetting.Instance.ConfigPath);
                    PrivateSetting.Instance.CmdConnect.StopConnect();
                    CtrlCmdConnect.Connect();
                    Context.Response.OutputStream.Write(Form, 0, Form.Length);
                    Context.Response.Headers["Content-Type"] = "text/html";
                    Context.Response.Send();
                    Context.Close();
                    PrivateSetting.Instance.SetupMode = false;
                    PrivateSetting.Instance.Server.Stop();
                    PrivateSetting.Instance.Server = new WebServer((int)Setting.Instance.HttpPort);
                    PrivateSetting.Instance.Server.OnRequest += ServerAction.DoProcess;
                    PrivateSetting.Instance.Server.Start();
                }
                else
                {
                    Context.Response.OutputStream.Write(Form, 0, Form.Length);
                    Context.Response.Headers["Content-Type"] = "text/html";
                    Context.Response.Send();
                    Context.Close();
                }
            }
            else
            {
                byte[] Form = Encoding.UTF8.GetBytes(@"
<html>
    <head>
        <title>Setup</title>
    </head>
    <body>
        <h1>EpgTimerWeb2 Configure</h1>
        <form action='/update' method='post'>
            <table>
                <tr>
                    <td>EpgTimer Server</td>
                    <td><input name='ctrlhost' placeholder='127.0.0.1' value='127.0.0.1' /></td>
                </tr>
                <tr>
                    <td>EpgTimer ServerPort</td>
                    <td><input name='ctrlport' placeholder='4510' value='4510' /></td>
                </tr>
                <tr>
                    <td>EpgTimer CallbackPort</td>
                    <td><input name='cbport' placeholder='4521' value='4521' /></td>
                </tr>
                <tr>
                    <td>WUI Username</td>
                    <td><input name='user' placeholder='user'  /></td>
                </tr>
                <tr>
                    <td>WUI Password</td>
                    <td><input name='pass' placeholder='pass'  /></td>
                </tr>
                <tr>
                    <td>WUI Port</td>
                    <td><input name='http' placeholder='8080' value='8080' /></td>
                </tr>
                <tr>
                    <td>Pincode</td>
                    <td><input name='code' /></td>
                </tr>
            </table>
            <p><input type='submit' value='Update config' /></p>     
        </form>
    </body>
</html>
".Replace("'", "\""));
                Context.Response.OutputStream.Write(Form, 0, Form.Length);
                Context.Response.Headers["Content-Type"] = "text/html";
                Context.Response.Headers["Cache-Control"] = "no-cache";
                Context.Response.Send();
                Context.Close();
            }
        }
    }
}
