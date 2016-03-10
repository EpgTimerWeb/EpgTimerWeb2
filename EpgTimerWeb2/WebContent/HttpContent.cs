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
using System.Drawing.Imaging;
using System.IO;

namespace EpgTimer
{
    public class HttpContent
    {

        public bool RequestUrl(HttpContext Context, bool IsAuth)
        {
            var Ret = false;
            var PartName = Context.Request.Url.ToLower().Replace("/", "\\");
            if (PartName.IndexOf("..\\") >= 0)
            {
                throw new HttpResponseException(403, "Forbidden", "");
            }
            if (File.Exists(".\\web\\" + PartName) && IsAuth)
            {
                string MimeType = Mime.Get(PartName, "text/javascript");
                if (!Mime.IsImage(PartName))
                    MimeType += ";charset=UTF-8";
                Context.Response.Headers["Content-Type"] = MimeType;
                HttpContext.SendFile(Context, ".\\web\\" + PartName);
                Ret = true;
            }
            else
            {
                switch (Context.Request.Url.ToLower())
                {
                    case "/js/jquery.js":
                    case "/js/bootstrap.js":
                    case "/js/jquery.datatables.min.js":
                    case "/js/datatables.bootstrap.js":
                    case "/js/jquery.datatables.min.css":
                    case "/css/bootstrap.css":
                    case "/css/bootstrap.css.map":
                    case "/img/not_thumb.png":
                    case "/img/loader.gif":
                    case "/fonts/glyphicons-halflings-regular.eot":
                    case "/fonts/glyphicons-halflings-regular.svg":
                    case "/fonts/glyphicons-halflings-regular.ttf":
                    case "/fonts/glyphicons-halflings-regular.woff":
                    case "/fonts/glyphicons-halflings-regular.woff2":
                        Ret = true;
                        if (Context.Request.Headers.ContainsKey("If-Modified-Since"))
                        {
                            Context.Response.SetStatus(304, "Not Modified");
                            Context.Response.Send();
                            return true;
                        }
                        Context.Response.Headers.Add("Last-Modified", "Sun, 09 Sep 2001 00:00:00 GMT");
                        break;
                }
                switch (Context.Request.Url.ToLower())
                {
                    case "/js/jquery.js":
                        Context.Response.Headers.Add("Content-Type", "text/javascript");
                        HttpContext.SendResponse(Context, Resources.JQuery);
                        Ret = true;
                        break;
                    case "/js/bootstrap.js":
                        Context.Response.Headers.Add("Content-Type", "text/javascript");
                        HttpContext.SendResponse(Context, Resources.BootStrap);
                        Ret = true;
                        break;
                    case "/js/jquery.datatables.min.js":
                        Context.Response.Headers.Add("Content-Type", "text/javascript");
                        HttpContext.SendResponse(Context, Resources.jqury_dataTables_js);
                        Ret = true;
                        break;
                    case "/js/datatables.bootstrap.js":
                        Context.Response.Headers.Add("Content-Type", "text/javascript");
                        HttpContext.SendResponse(Context, Resources.dataTables_bootstrap);
                        Ret = true;
                        break;
                    case "/js/jquery.datatables.min.css":
                        Context.Response.Headers.Add("Content-Type", "text/css");
                        HttpContext.SendResponse(Context, Resources.jquery_dataTables_css);
                        Ret = true;
                        break;
                    case "/css/bootstrap.css":
                        Context.Response.Headers.Add("Content-Type", "text/css");
                        HttpContext.SendResponse(Context, Resources.BootStrapStyle);
                        Ret = true;
                        break;
                    case "/css/bootstrap.css.map":
                        Context.Response.Headers.Add("Content-Type", "text/plain");
                        HttpContext.SendResponse(Context, Resources.BootStrapCssMap);
                        Ret = true;
                        break;
                    case "/img/not_thumb.png":
                        Context.Response.Headers.Add("Content-Type", "image/png");
                        var Stream = new MemoryStream();
                        Resources.NotThumbnail.Save(Stream, ImageFormat.Png);
                        HttpContext.SendResponse(Context, Stream.GetBuffer());
                        Stream.Close();
                        Ret = true;
                        break;
                    case "/img/loader.gif":
                        Context.Response.Headers.Add("Content-Type", "image/gif");
                        var Stream1 = new MemoryStream();
                        Resources.loader.Save(Stream1, ImageFormat.Gif);
                        HttpContext.SendResponse(Context, Stream1.GetBuffer());
                        Stream1.Close();
                        Ret = true;
                        break;
                    case "/fonts/glyphicons-halflings-regular.eot":
                        Ret = true;
                        HttpContext.SendResponse(Context, Resources.glyphicons_halflings_regular_eot);
                        break;
                    case "/fonts/glyphicons-halflings-regular.svg":
                        Ret = true;
                        HttpContext.SendResponse(Context, Resources.glyphicons_halflings_regular_svg);
                        break;
                    case "/fonts/glyphicons-halflings-regular.ttf":
                        Ret = true;
                        HttpContext.SendResponse(Context, Resources.glyphicons_halflings_regular_ttf);
                        break;
                    case "/fonts/glyphicons-halflings-regular.woff":
                        Ret = true;
                        HttpContext.SendResponse(Context, Resources.glyphicons_halflings_regular_woff);
                        break;
                    case "/fonts/glyphicons-halflings-regular.woff2":
                        Ret = true;
                        HttpContext.SendResponse(Context, Resources.glyphicons_halflings_regular_woff2);
                        break;
                }
            }
            return Ret;
        }
    }
}
