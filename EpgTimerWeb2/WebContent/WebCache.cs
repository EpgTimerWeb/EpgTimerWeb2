using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpgTimer
{
    public class WebCache
    {
        private static WebCache _instance = null;
        public static WebCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WebCache();
                return _instance;
            }
            set { _instance = value; }
        }
        public class CacheEntry
        {
            public byte[] Data { set; get; }
            public DateTime LastModified { set; get; }
            public CacheEntry()
            {
                Data = null;
                LastModified = DateTime.Now;
            }
        }
        public Dictionary<string, CacheEntry> CacheList { set; get; }
        private object Lock = null;
        public WebCache()
        {
            CacheList = new Dictionary<string, CacheEntry>();
            Lock = new object();
        }
        public CacheEntry Get(string Name)
        {
            if (CacheList.ContainsKey(Name) &&
                CacheList.Count(s => s.Value.Data != null && (DateTime.Now - s.Value.LastModified).TotalSeconds < 3600) > 0)
            {
                return CacheList[Name];
            }
            return null;
        }
        public void Set(string Name, byte[] Value)
        {
            lock (Lock)
            {
                if (CacheList.ContainsKey(Name))
                    CacheList.Remove(Name);
                CacheList.Add(Name, new CacheEntry() { LastModified = DateTime.Now, Data = Value });
            }
        }
        public void Clear()
        {
            lock (Lock)
            {
                CacheList.Clear();
            }
        }
        public static void CachedResponse(HttpContext Info, Func<HttpContext, string> RequestProcess)
        {
            byte[] Res = null;

            var entry = WebCache.Instance.Get(Info.Request.RawUrl);
            if (entry == null)
            {
                string Result = RequestProcess(Info);
                Res = Encoding.UTF8.GetBytes(Result);
                WebCache.Instance.Set(Info.Request.RawUrl, Res);
            }
            else
            {
                if (Info.Request.Headers.ContainsKey("If-Modified-Since"))
                {
                    var IfModifiedSinceStr = Info.Request.Headers["If-Modified-Since"];
                    if (IfModifiedSinceStr != entry.LastModified.ToString("R"))
                    {
                        Res = entry.Data;
                    }
                }
                else
                {
                    Res = entry.Data;
                }
            }
            if (entry != null)
                Info.Response.Headers["Last-Modified"] = entry.LastModified.ToString("R");
            else
                Info.Response.Headers["Last-Modified"] = DateTime.Now.ToString("R");

            if (Res != null)
                Info.Response.OutputStream.Write(Res, 0, Res.Length);
            else
                Info.Response.SetStatus(304, "Not Modified");
            Info.Response.Send();
        }
    }
}
