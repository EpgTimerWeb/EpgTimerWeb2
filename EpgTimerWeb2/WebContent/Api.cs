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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace EpgTimer
{
    public class ApiResult
    {
        public string JsonData { set; get; }
        public bool Cacheable { set; get; }
        public UpdateNotifyItem CacheDelete { set; get; }
        public ApiResult()
        {
            JsonData = "";
            Cacheable = false;
            CacheDelete = UpdateNotifyItem.No;
        }
    }
    public class Api
    {
        private static bool ContainsMultipleKeys(IDictionary<string, string> Arg, params string[] Keys)
        {
            foreach (string Key in Keys)
            {
                if (!Arg.ContainsKey(Key)) return false;
            }
            return true;
        }
        private static RecSettingData GetPreset(Dictionary<string, string> Arg, RecSettingData Old)
        {
            RecSettingData pInfo = Old;
            pInfo.ServiceMode = (byte)((Arg.ContainsKey("savedata") || Arg.ContainsKey("savecaption")) || Arg.ContainsKey("nousedata") ? 1 : 0);
            if (Arg.ContainsKey("savecaption"))
                pInfo.ServiceMode |= 0x10;
            if (Arg.ContainsKey("savedata"))
                pInfo.ServiceMode |= 0x20;
            pInfo.IsPittari = (byte)(Arg.ContainsKey("usepittari") ? 1 : 0);
            pInfo.IsTuijyuu = (byte)(Arg.ContainsKey("usetuijyuu") ? 1 : 0);
            pInfo.IsReboot = (byte)(Arg.ContainsKey("reboot") ? 1 : 0);
            pInfo.SuspendMode = 0;
            if (Arg.ContainsKey("suspendmode"))
            {
                if (byte.Parse(Arg["suspendmode"]) < 5 && byte.Parse(Arg["suspendmode"]) > 0)
                    pInfo.SuspendMode = byte.Parse(Arg["suspendmode"]);
            }
            pInfo.UseMargin = (byte)(Arg.ContainsKey("marginstart") && Arg.ContainsKey("marginend") ? 1 : 0);
            if (ContainsMultipleKeys(Arg, "marginstart", "marginend"))
            {
                int startSec = 0;
                int startMinus = 1;
                if (Arg["marginstart"].IndexOf("-") == 0)
                {
                    startMinus = -1;
                }
                string[] startArray = Arg["marginstart"].Split(':');
                if (startArray.Length == 2)
                {
                    startSec = int.Parse(startArray[0]) * 60;
                    startSec += int.Parse(startArray[1]) * startMinus;
                }
                else if (startArray.Length == 3)
                {
                    startSec = int.Parse(startArray[0]) * 60 * 60;
                    startSec += int.Parse(startArray[1]) * 60 * startMinus;
                    startSec += int.Parse(startArray[2]) * startMinus;
                }
                else
                {
                    startSec = int.Parse(startArray[0]);
                }

                int endSec = 0;
                int endMinus = 1;
                if (Arg["marginend"].IndexOf("-") == 0)
                {
                    endMinus = -1;
                }
                string[] endArray = Arg["marginend"].Split(':');
                if (endArray.Length == 2)
                {
                    endSec = int.Parse(endArray[0]) * 60;
                    endSec += int.Parse(endArray[1]) * endMinus;
                }
                else if (endArray.Length == 3)
                {
                    endSec = int.Parse(endArray[0]) * 60 * 60;
                    endSec += int.Parse(endArray[1]) * 60 * endMinus;
                    endSec += int.Parse(endArray[2]) * endMinus;
                }
                else
                {
                    endSec = int.Parse(endArray[0]);
                }

                pInfo.StartMargin = startSec;
                pInfo.EndMargin = endSec;
            }
            if (ContainsMultipleKeys(Arg, "usepartial", "partialdir"))
            {
                pInfo.IsPartialRec = 1;
                string[] s = Arg["partialdir"].Split('*');
                pInfo.PartialRecFolder.AddRange(s.Select(p =>
                {
                    if (p.Split(':').Length != 3 || p.Split(':').Length != 2) return null;
                    string[] k = p.Split(':');
                    if (!CommonManager.Instance.DB.WritePlugInList.ContainsValue(k[1])) return null;
                    if (k.Length == 3 && !CommonManager.Instance.DB.RecNamePlugInList.ContainsValue(k[2])) return null;
                    return new RecFileSetInfo()
                    {
                        RecNamePlugIn = k.Length == 3 ? k[2] : "",
                        WritePlugIn = k[1],
                        RecFolder = k[0]
                    };
                }).Where(p => p != null));
                if (pInfo.PartialRecFolder.Count == 0) pInfo.IsPartialRec = 0;
            }
            if (Arg.ContainsKey("continuerec"))
                pInfo.IsContinueRec = 1;

            pInfo.TunerID = 0;
            if (Arg.ContainsKey("tuner") && CommonManager.Instance.DB.TunerReserveList.Values.Where(s => s.TunerID == uint.Parse(Arg["tuner"])).Count() > 0 && uint.Parse(Arg["tuner"]) != 0xFFFFFFFF)
                pInfo.TunerID = uint.Parse(Arg["tuner"]);
            pInfo.Priority = 2;
            if (Arg.ContainsKey("priority") && byte.Parse(Arg["priority"]) >= 1 && byte.Parse(Arg["priority"]) <= 5)
                pInfo.Priority = byte.Parse(Arg["priority"]);
            pInfo.RecMode = 1;
            if (Arg.ContainsKey("recmode") && byte.Parse(Arg["recmode"]) >= 0 && byte.Parse(Arg["recmode"]) <= 5)
                pInfo.RecMode = byte.Parse(Arg["recmode"]);
            return pInfo;
        }
        private static long GetServiceKeyFromValue(string val)
        {
            if (val.IndexOf("-") > 0)
            {
                string[] split = val.Split(new char[] { '-' });
                if (split.Length != 3) throw new InvalidDataException();
                return (long)CommonManager.Create64Key(ushort.Parse(split[0]),
                    ushort.Parse(split[1]),
                    ushort.Parse(split[2]));
            }
            else
            {
                return long.Parse(val);
            }
        }
        private static long GetServiceKey(Dictionary<string, string> Arg, bool ScanIDParam = true)
        {
            if (ContainsMultipleKeys(Arg, "tsid", "onid", "sid") && ScanIDParam)
            {
                return (long)CommonManager.Create64Key(ushort.Parse(Arg["onid"]),
                    ushort.Parse(Arg["tsid"]),
                    ushort.Parse(Arg["sid"]));
            }
            else if (Arg.ContainsKey("key"))
            {
                return GetServiceKeyFromValue(Arg["key"]);
            }
            else
            {
                return 0;
            }
        }
        private static long GetEventKeyFromValue(string val)
        {
            if (val.IndexOf("-") > 0)
            {
                string[] split = val.Split(new char[] { '-' });
                if (split.Length != 4) throw new InvalidDataException();
                return (long)CommonManager.Create64PgKey(ushort.Parse(split[0]),
                    ushort.Parse(split[1]),
                    ushort.Parse(split[2]),
                    ushort.Parse(split[3]));
            }
            else
            {
                return long.Parse(val);
            }
        }
        private static long GetEventKey(Dictionary<string, string> Arg)
        {
            if (ContainsMultipleKeys(Arg, "tsid", "onid", "sid", "eid"))
            {
                return (long)CommonManager.Create64PgKey(ushort.Parse(Arg["onid"]),
                    ushort.Parse(Arg["tsid"]),
                    ushort.Parse(Arg["sid"]),
                    ushort.Parse(Arg["eid"]));
            }
            else
            {
                return GetEventKeyFromValue(Arg["key"]);
            }
        }
        private static DateTime GetDateTimeFromString(string str)
        {
            if (str.Length == 10 || str.Length == 9)
            {
                long unix = 0;
                if (long.TryParse(str, out unix))
                {
                    return UnixTime.FromUnixTime(unix);
                }
            }
            else
            {
                DateTime date;
                if (DateTime.TryParse(str, out date))
                {
                    return date;
                }
            }
            throw new FormatException("DateTime invalid");
        }
        public static EpgSearchKeyInfo GetEpgSKey(Dictionary<string, string> Arg, EpgSearchKeyInfo Old)
        {
            EpgSearchKeyInfo e = Old;
            if (Arg.ContainsKey("srvlist") && Arg["srvlist"] != "*")
            {
                e.ServiceList = Arg["srvlist"].Split(',').Select(s => GetServiceKeyFromValue(s)).ToList();
            }
            else
            {
                foreach (var ch in ChSet5.Instance.ChList)
                {
                    e.ServiceList.Add((long)ch.Value.Key);
                }
            }
            if (Arg.ContainsKey("content"))
            {
                e.ContentList.AddRange(Arg["content"].Split(',').Select(s =>
                {
                    string[] c = s.Split('.');
                    if (c.Length != 4) return null;
                    return new EpgContentData()
                    {
                        Content1 = byte.Parse(c[0]),
                        Content2 = byte.Parse(c[1]),
                        User1 = byte.Parse(c[2]),
                        User2 = byte.Parse(c[3])
                    };
                }).Where(s => s != null));
            }
            if (Arg.ContainsKey("notcontent")) e.IsNotContent = 1;
            if (Arg.ContainsKey("useregex")) e.IsRegExp = 1;
            if (Arg.ContainsKey("useregex")) e.IsAimai = 0;
            if (Arg.ContainsKey("aimai")) e.IsAimai = 1;
            if (Arg.ContainsKey("aimai")) e.IsRegExp = 0;
            if (Arg.ContainsKey("tonly")) e.IsTitleOnly = 1;
            if (Arg.ContainsKey("kw"))
            {
                e.AndKey = Arg["kw"];
            }
            if (Arg.ContainsKey("notkw"))
            {
                e.NotKey = Arg["notkw"];
            }
            if (Arg.ContainsKey("freeca"))
            {
                e.FreeCA = byte.Parse(Arg["freeca"]);
            }
            if (Arg.ContainsKey("date"))
            {
                e.DateList.AddRange(Arg["date"].Split(',').Select(s =>
                {
                    if (s.IndexOf("-") < 0)
                        return null;
                    if (s.Split('-')[0].IndexOf(".") < 0 || s.Split('-')[1].IndexOf(".") < 0)
                        return null;
                    string[] a = s.Split('-');
                    string[] b = a[0].Split('.');
                    string[] c = a[1].Split('.');
                    if (b.Length != 3 || c.Length != 3 ||
                    uint.Parse(c[1]) > 24 || uint.Parse(b[1]) > 24 || uint.Parse(c[2]) > 60 ||
                    uint.Parse(b[2]) > 60 || uint.Parse(c[0]) > 7 || uint.Parse(b[0]) > 7)
                        return null;

                    return new EpgSearchDateInfo()
                    {
                        StartDayOfWeek = byte.Parse(b[0]),
                        StartHour = ushort.Parse(b[1]),
                        StartMin = ushort.Parse(b[2]),
                        EndDayOfWeek = byte.Parse(c[0]),
                        EndHour = ushort.Parse(c[1]),
                        EndMin = ushort.Parse(c[2])
                    };
                }).Where(p => p != null));
            }
            if (Arg.ContainsKey("notdate"))
            {
                e.IsNotDate = 1;
            }
            return e;
        }
        public static ApiResult Call(string Command, Dictionary<string, string> Arg, bool Indent = true, bool NotUseCache = false)
        {
            ApiResult Result = new ApiResult();
            JsonResult Data = new JsonResult(null, false, ErrCode.CMD_NO_ARG);
            try
            {
                if (Arg.ContainsKey("noindent")) Indent = false;
                if (Arg.ContainsKey("indent")) Indent = true;
                if (Command == "EnumService")
                {
                    Result.Cacheable = true;
                    Data = new JsonResult(ChSet5.Instance.ChList.Values);
                }

                #region 番組情報関係(EnumServiceEvent,GetEpgEvent)
                else if (Command == "EnumServiceEvent")
                {
                    Result.Cacheable = true;
                    Result.CacheDelete = UpdateNotifyItem.EpgData;

                    bool Modified = true;
                    if (Arg.ContainsKey("if_last"))
                    {
                        if (GetDateTimeFromString(Arg["if_last"]) <= CommonManager.Instance.DB.ServiceEventLastUpdate)
                        {
                            Data = new JsonResult(null, ErrCode.CMD_NO_RES);
                            Modified = false;
                        }
                    }
                    if (Modified)
                    {
                        ulong Key = (ulong)GetServiceKey(Arg);
                        ulong Since = 0;
                        ulong Max = 0;
                        DateTime SinceDate = DateTime.Now;
                        SinceDate = SinceDate.AddHours(-SinceDate.Hour);
                        SinceDate = SinceDate.AddMinutes(-SinceDate.Minute);
                        SinceDate = SinceDate.AddSeconds(-SinceDate.Second);
                        DateTime MaxDate = DateTime.MaxValue;
                        bool FindDate = false;

                        // API: EventID取得はKeyを指定する
                        if (Arg.ContainsKey("usedate") || Key == 0)
                        {
                            FindDate = true;
                            if (Arg.ContainsKey("sincedate"))
                                SinceDate = GetDateTimeFromString(Arg["sincedate"]);
                            if (Arg.ContainsKey("maxdate"))
                                MaxDate = GetDateTimeFromString(Arg["maxdate"]);
                        }
                        else
                        {
                            if (Arg.ContainsKey("since"))
                                Since = ulong.Parse(Arg["since"]);
                            if (Arg.ContainsKey("max"))
                                Max = ulong.Parse(Arg["max"]);
                        }
                        var Out = new Dictionary<string, List<EventInfoItem>>();
                        if (FindDate)
                        {
                            foreach (var a in CommonManager.Instance.DB.ServiceEventList)
                            {
                                if (Key != 0 && a.Key != Key) continue;
                                Out.Add(a.Key.ToString(),
                                    a.Value.EventList
                                    .Where(b => b.StartTime > SinceDate)
                                    .Where(c => c.IsDuration == 0 || c.StartTime.AddSeconds(c.DurationSec) < MaxDate)
                                    .Select(d => new EventInfoItem(d))
                                    .ToList()
                                    );
                            }
                        }
                        else
                        {
                            foreach (var a in CommonManager.Instance.DB.ServiceEventList)
                            {
                                if (Key != 0 && a.Key != Key) continue;
                                Out.Add(a.Key.ToString(),
                                    a.Value.EventList
                                    .Where(b => Since == 0 || b.EventID >= Since)
                                    .Where(c => Max == 0 || c.EventID <= Max)
                                    .Select(d => new EventInfoItem(d))
                                    .ToList()
                                    );
                            }
                        }
                        Data = new JsonResult(new { Epg = Out, Last = CommonManager.Instance.DB.ServiceEventLastUpdate });
                    }
                }
                else if (Command == "GetEpgEvent")
                {
                    Result.Cacheable = true;
                    Result.CacheDelete = UpdateNotifyItem.EpgData;
                    ulong Key = (ulong)GetEventKey(Arg);
                    if (Key == 0)
                    {
                        Data = new JsonResult(null, ErrCode.CMD_NO_ARG);
                    }
                    else
                    {
                        ulong ServiceKey = Key >> 16;
                        ulong EventID = Key & 0xFFFF;
                        EpgEventInfo Event = null;
                        if (CommonManager.Instance.DB.ServiceEventList.ContainsKey(ServiceKey))
                        {
                            var service = CommonManager.Instance.DB.ServiceEventList[ServiceKey];
                            foreach (var _event in service.EventList)
                            {
                                if (_event.EventID == EventID)
                                {
                                    Event = _event;
                                    break;
                                }
                            }
                        }
                        if (Event == null)
                            Data = new JsonResult(null, false, ErrCode.CMD_NO_RES);
                        else
                            Data = new JsonResult(new EventInfoItem(Event));
                    }
                }
                #endregion
                #region プラグイン
                else if (Command == "EnumWritePlugInList")
                {
                    Result.Cacheable = true;
                    Data = new JsonResult(CommonManager.Instance.DB.WritePlugInList);
                }
                else if (Command == "EnumRecNamePlugInList")
                {
                    Result.Cacheable = true;
                    Data = new JsonResult(CommonManager.Instance.DB.RecNamePlugInList);
                }
                #endregion
                #region EDCB操作関係
                else if (Command == "EpgCapNow")
                {
                    ErrCode Err = (ErrCode)CommonManager.Instance.CtrlCmd.SendEpgCapNow();
                    Data = new JsonResult(null, Err);
                }
                else if (Command == "EpgReload")
                {
                    ErrCode Err = (ErrCode)CommonManager.Instance.CtrlCmd.SendReloadEpg();
                    Data = new JsonResult(null, Err);
                }
                #endregion
                #region 予約関連(EnumReserve,AddReserve,UpdateReserve,RemoveReserve,EnumTunerReserve)
                else if (Command == "EnumReserve")
                {
                    Result.Cacheable = true;
                    Result.CacheDelete = UpdateNotifyItem.ReserveInfo;
                    Data = new JsonResult(CommonManager.Instance.DB.ReserveList.Values);
                }
                else if (Command == "AddReserve")
                {
                    bool IsProgram = Arg.ContainsKey("program");
                    if (!IsProgram && ContainsMultipleKeys(Arg, "tsid", "onid", "sid", "eid")) //最低限必要
                    {
                        ushort ONID = ushort.Parse(Arg["onid"]);
                        ushort SID = ushort.Parse(Arg["sid"]);
                        ushort TSID = ushort.Parse(Arg["tsid"]);
                        ushort EventID = ushort.Parse(Arg["eid"]);
                        ulong Key = CommonManager.Create64Key(ONID, TSID, SID);
                        ulong PGKey = CommonManager.Create64PgKey(ONID, TSID, SID, EventID);
                        EpgEventInfo Event = null;
                        if (!CommonManager.Instance.DB.ServiceEventList.ContainsKey(Key))
                        {
                            Data = new JsonResult(null, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                        if (CommonManager.Instance.DB.ServiceEventList[Key].EventList.Count(e => e.EventID == EventID) == 1)
                        {
                            Event = CommonManager.Instance.DB.ServiceEventList[Key].EventList.First(e => e.EventID == EventID);
                        }
                        else
                        {
                            CommonManager.Instance.CtrlCmd.SendGetPgInfo(PGKey, ref Event);
                        }
                        if (Event != null)
                        {
                            ReserveData Reserve = new ReserveData();
                            if (Event.ShortInfo != null) Reserve.Title = Event.ShortInfo.EventName;
                            Reserve.StartTime = Event.StartTime;
                            Reserve.StartTimeEpg = Event.StartTime;
                            if (Event.IsDuration == 0)
                            {
                                Reserve.DurationSecond = 10 * 60;
                            }
                            else
                            {
                                Reserve.DurationSecond = Event.DurationSec;
                            }
                            if (ChSet5.Instance.ChList.ContainsKey(Key)) Reserve.StationName = ChSet5.Instance.ChList[Key].ServiceName;
                            Reserve.ONID = Event.ONID;
                            Reserve.TSID = Event.TSID;
                            Reserve.SID = Event.SID;
                            Reserve.EventID = Event.EventID;
                            RecSettingData Setting = GetPreset(Arg, new RecSettingData());
                            Reserve.RecSetting = Setting;
                            ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendAddReserve(new List<ReserveData> { Reserve });
                            Data = new JsonResult(Reserve, err);
                        }
                        else
                        {
                            Data = new JsonResult(null, false, ErrCode.CMD_NO_RES);
                        }
                    }
                    else if (IsProgram && ContainsMultipleKeys(Arg, "tsid", "onid", "sid", "start", "end", "title"))
                    {
                        ushort ONID = ushort.Parse(Arg["onid"]);
                        ushort SID = ushort.Parse(Arg["sid"]);
                        ushort TSID = ushort.Parse(Arg["tsid"]);
                        ulong Key = CommonManager.Create64Key(ONID, TSID, SID);
                        ReserveData Reserve = new ReserveData();
                        
                        Reserve.Title = Arg["title"];

                        Reserve.ONID = ONID;
                        Reserve.TSID = TSID;
                        Reserve.SID = SID;
                        Reserve.EventID = 0xFFFF;
                        if (ChSet5.Instance.ChList.ContainsKey(Key))
                        {
                            Reserve.StationName = ChSet5.Instance.ChList[Key].ServiceName;
                        }
                        else
                        {
                            Reserve.StationName = Arg.ContainsKey("station") ? Arg["station"] : "Unknown";
                        }
                        DateTime StartTime = GetDateTimeFromString(Arg["start"]);
                        DateTime EndTime = GetDateTimeFromString(Arg["end"]);
                        if (StartTime > EndTime)
                        {
                            Data = new JsonResult(null, false, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                        else
                        {
                            Reserve.StartTime = StartTime;
                            Reserve.DurationSecond = (uint)((EndTime - StartTime).TotalSeconds);
                            RecSettingData Setting = GetPreset(Arg, new RecSettingData());
                            Setting.IsPittari = 0;
                            Setting.IsTuijyuu = 0;
                            Reserve.RecSetting = Setting;
                            ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendAddReserve(new List<ReserveData> { Reserve });
                            Data = new JsonResult(Reserve, err);
                        }
                    }
                    else
                    {
                        Data = new JsonResult(null, false, ErrCode.CMD_NO_ARG);
                    }
                }
                else if (Command == "UpdateReserve")
                {
                    if (Arg.ContainsKey("id") &&
                        CommonManager.Instance.DB.ReserveList.ContainsKey(uint.Parse(Arg["id"])))
                    {
                        ReserveData Target = CommonManager.Instance.DB.ReserveList[uint.Parse(Arg["id"])];
                        Target.RecSetting = GetPreset(Arg, Target.RecSetting);
                        if (Target.EventID == 0xFFFF)
                        {
                            if (ContainsMultipleKeys(Arg, "tsid", "onid", "sid"))
                            {
                                ushort ONID = ushort.Parse(Arg["onid"]);
                                ushort SID = ushort.Parse(Arg["sid"]);
                                ushort TSID = ushort.Parse(Arg["tsid"]);
                                ulong Key = CommonManager.Create64Key(ONID, TSID, SID);
                                Target.StationName = Arg.ContainsKey("station") ? Arg["station"] : Target.StationName;

                                Target.ONID = ONID;
                                Target.TSID = TSID;
                                Target.SID = SID;
                                if (ChSet5.Instance.ChList.ContainsKey(Key))
                                {
                                    Target.StationName = ChSet5.Instance.ChList[Key].ServiceName;
                                }
                            }
                            if (Arg.ContainsKey("title"))
                            {
                                Target.Title = Arg["title"];
                            }
                            if (ContainsMultipleKeys(Arg, "start", "end"))
                            {
                                DateTime StartTime = GetDateTimeFromString(Arg["start"]);
                                DateTime EndTime = GetDateTimeFromString(Arg["end"]);
                                if (StartTime < EndTime)
                                {
                                    Target.StartTime = StartTime;
                                    Target.DurationSecond = (uint)((EndTime - StartTime).TotalSeconds);
                                }
                            }
                            Target.RecSetting.IsPittari = 0;
                            Target.RecSetting.IsTuijyuu = 0;
                        }
                        ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendChgReserve(new List<ReserveData> { Target });
                        Data = new JsonResult(Target, err);
                    }
                }
                else if (Command == "RemoveReserve")
                {
                    if (Arg.ContainsKey("id"))
                    {
                        ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendDelReserve(new List<uint> { uint.Parse(Arg["id"]) });
                        Data = new JsonResult(null, err);
                    }
                }
                else if (Command == "EnumTunerReserve")
                {
                    Result.Cacheable = true;
                    Result.CacheDelete = UpdateNotifyItem.ReserveInfo;
                    Data = new JsonResult(CommonManager.Instance.DB.TunerReserveList.Select(s => s.Value).ToList());
                }
                #endregion
                #region 自動予約(EPG)関連(EnumAutoReserve,AddAutoReserve,UpdateAutoReserve,RemoveAutoReserve)
                else if (Command == "EnumAutoReserve")
                {
                    Result.Cacheable = true;
                    Result.CacheDelete = UpdateNotifyItem.AutoAddEpgInfo;
                    Data = new JsonResult(CommonManager.Instance.DB.EpgAutoAddList);
                }
                else if (Command == "AddAutoReserve")
                {
                    var Preset = GetPreset(Arg, new RecSettingData());
                    var Search = GetEpgSKey(Arg, new EpgSearchKeyInfo());
                    if (ContainsMultipleKeys(Arg, "overlap", "overlap_day"))
                    {
                        Search.ChkRecDay = ushort.Parse(Arg["overlap_day"]);
                        Search.ChkRecEnd = 1;
                    }

                    ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendAddEpgAutoAdd(new List<EpgAutoAddData>{
                        new EpgAutoAddData(){
                            Search = Search,
                            Setting = Preset,
                        }
                    });
                    Data = new JsonResult(new EpgAutoAddData()
                    {
                        Search = Search,
                        Setting = Preset
                    }, err);
                }
                else if (Command == "UpdateAutoReserve")
                {
                    if (Arg.ContainsKey("id"))
                    {
                        uint Id = uint.Parse(Arg["id"]);
                        if (CommonManager.Instance.DB.EpgAutoAddList.ContainsKey(Id))
                        {
                            var Target = CommonManager.Instance.DB.EpgAutoAddList[Id];
                            var Preset = GetPreset(Arg, Target.Setting);
                            var Search = GetEpgSKey(Arg, Target.Search);
                            ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendChgEpgAutoAdd(new List<EpgAutoAddData>{
                                new EpgAutoAddData(){
                                    Search = Search,
                                    Setting = Preset,
                                    ID = Id
                                }
                            });
                            Data = new JsonResult(new EpgAutoAddData()
                            {
                                Search = Search,
                                Setting = Preset
                            }, err);
                        }
                    }
                }
                else if (Command == "RemoveAutoReserve")
                {
                    if (Arg.ContainsKey("id"))
                    {
                        ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendDelEpgAutoAdd(new List<uint> { uint.Parse(Arg["id"]) });
                        Data = new JsonResult(null, err);
                    }
                }
                #endregion
                #region プリセット関係(EnumPresets,AddPreset)
                else if (Command == "EnumPresets")
                {
                    //Result.Cacheable = true;
                    Data = new JsonResult(Setting.Instance.Presets);
                }
                else if (Command == "AddPreset")
                {
                    if (!Arg.ContainsKey("name"))
                    {
                        Data = new JsonResult(null, ErrCode.CMD_NO_ARG);
                    }
                    else
                    {
                        if (Setting.Instance.Presets.Count(p => p.DisplayName == Arg["name"]) > 0)
                        {
                            Data = new JsonResult(null, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                        else
                        {
                            int ID = PresetDb.Instance.AddPreset(GetPreset(Arg, new RecSettingData()), Arg["name"]);
                            Data = new JsonResult(new { ID = ID }, ID != -1);
                        }
                    }
                }
                #endregion
                #region カスタムView関係
                else if (Command == "EnumViews")
                {
                    //Result.Cacheable = true;
                    Data = new JsonResult(Setting.Instance.Views);
                }
                else if (Command == "AddView")
                {
                    if (ContainsMultipleKeys(Arg, "srvlist", "name", "extra"))
                    {
                        int ID = PresetDb.Instance.AddView(GetEpgSKey(Arg, new EpgSearchKeyInfo()), Arg["name"], Arg["extra"]);
                        Data = new JsonResult(new { ID = ID }, ID != -1);
                    }
                    else
                    {
                        Data = new JsonResult(null, false, ErrCode.CMD_NO_ARG);
                    }
                }
                #endregion
                #region EPG検索
                else if (Command == "EpgSearch")
                {
                    Result.Cacheable = true;
                    if (Arg.ContainsKey("srvlist"))
                    {
                        List<EpgEventInfo> EpgResult = new List<EpgEventInfo>();
                        ErrCode code = (ErrCode)CommonManager.Instance.CtrlCmd.SendSearchPg(new List<EpgSearchKeyInfo> { GetEpgSKey(Arg, new EpgSearchKeyInfo()) }, ref EpgResult);
                        Data = new JsonResult(EpgResult.Select(s => new EventInfoItem(s)), code);
                    }
                    else
                    {
                        Data = new JsonResult(null, false, ErrCode.CMD_NO_ARG);
                    }
                }
                #endregion
                else if (Command == "EnumEvents")
                {
                    //Result.Cacheable = true;
                    Data = new JsonResult(EventStore.Instance.Events);
                }
                #region EPG色
                else if (Command == "GetContentColorTable")
                {
                    //Result.Cacheable = true;
                    Data = new JsonResult(Setting.Instance.ContentToColorTable);
                }
                else if (Command == "SetContentColorTable")
                {
                    Regex ColorRegex = new Regex(@"[0-9a-fA-F]{6}");
                    if (ContainsMultipleKeys(Arg, "id", "color") && uint.Parse(Arg["id"]) >= 0 && ColorRegex.IsMatch(Arg["color"]))
                    {
                        if (Setting.Instance.ContentToColorTable.Count(s => s.ContentLevel1 == uint.Parse(Arg["id"])) == 0)
                        {
                            Setting.Instance.ContentToColorTable.Add(new ContentColorItem(uint.Parse(Arg["id"]), "#" + Arg["color"]));
                        }
                        else
                        {
                            Setting.Instance.ContentToColorTable.RemoveAll(s => s.ContentLevel1 == uint.Parse(Arg["id"]));
                            Setting.Instance.ContentToColorTable.Add(new ContentColorItem(uint.Parse(Arg["id"]), "#" + Arg["color"]));
                        }

                        Data = new JsonResult(null);
                    }
                }
                #endregion
                else if (Command == "RemoveManualAdd")
                {
                    if (Arg.ContainsKey("id"))
                    {
                        ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendDelManualAdd(new List<uint> { uint.Parse(Arg["id"]) });
                        Data = new JsonResult(null, err);
                    }
                }
                #region 録画済みファイル関係(EnumRecFileInfo,RemoveRecFile,ProtectRecFile)
                else if (Command == "EnumRecFileInfo")
                {
                    Result.Cacheable = true;
                    Data = new JsonResult(CommonManager.Instance.DB.RecFileInfo.Values);
                }
                else if (Command == "RemoveRecFile")
                {
                    if (Arg.ContainsKey("id"))
                    {
                        ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendDelRecInfo(new List<uint> { uint.Parse(Arg["id"]) });
                        Data = new JsonResult(null, err);
                    }
                }
                else if (Command == "ProtectRecFile")
                {
                    if (ContainsMultipleKeys(Arg, "id", "stat"))
                    {
                        uint Id = uint.Parse(Arg["id"]);
                        if (CommonManager.Instance.DB.RecFileInfo.ContainsKey(Id))
                        {
                            var Target = CommonManager.Instance.DB.RecFileInfo[Id];
                            Target.IsProtect = (byte)(Arg["stat"] == "on" ? 1 : 0);
                            ErrCode err = (ErrCode)CommonManager.Instance.CtrlCmd.SendChgProtectRecInfo(new List<RecFileInfo> { Target });
                            Data = new JsonResult(null, err);
                        }
                    }
                }
                #endregion
                else if (Command == "Hello")
                {
                    Result.Cacheable = true;
                    Data = new JsonResult(VersionInfo.Instance);
                }
                else if (Command == "ReloadLocal")
                {
                    WebCache.Instance.Clear();
                    ApiCache.Instance.Clear();
                    CommonManager.Instance.DB.ReloadAll(Arg.ContainsKey("force"));
                }
                else if (Command == "ChangePassword")
                {
                    if (ContainsMultipleKeys(Arg, "now", "new"))
                    {
                        if (Setting.Instance.LoginPassword == Arg["now"])
                        {
                            Setting.Instance.LoginPassword = Arg["new"];
                            Data = new JsonResult(null, ErrCode.CMD_SUCCESS);
                        }
                        else
                        {
                            Data = new JsonResult(null, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                    }
                }
                else if (Command == "FileBrowse")
                {
                    if (Arg.ContainsKey("path"))
                    {
                        if (Arg["path"].IndexOf(":") == 1
                            && Arg["path"].IndexOf("\\..\\") < 0
                            && Arg["path"].IndexOf("\\.\\") < 0
                            && Arg["path"].EndsWith("\\")
                            && Directory.Exists(Arg["path"]))
                        {
                            List<string> Items = new List<string>();
                            Items.AddRange(Directory.GetFiles(Arg["path"]));
                            Items.AddRange(Directory.GetDirectories(Arg["path"]).Select(s => s + "\\"));
                            Data = new JsonResult(Items);
                        }
                        else
                        {
                            Data = new JsonResult(null, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                    }
                    else
                    {
                        Data = new JsonResult(Directory.GetLogicalDrives());
                    }
                }
                else if (Command == "FFprobe")
                {
                    if (Arg.ContainsKey("path"))
                    {
                        Debug.Print(Arg["path"]);
                        if (Arg["path"].IndexOf(":") == 1
                            && Arg["path"].IndexOf("\\.\\") < 0
                            && Arg["path"].IndexOf("\\..\\") < 0
                            && File.Exists(Arg["path"]))
                        {
                            Data = new JsonResult(FFprobe.ProbeFormat(Arg["path"]));
                        }
                        else
                        {
                            Data = new JsonResult(null, ErrCode.CMD_ERR_INVALID_ARG);
                        }
                    }
                }
                else if (Command == "Debug")
                {
                    Data = new JsonResult(Arg);
                }
                Result.JsonData = JsonUtil.Serialize(Data, Indent);
            }
            catch (Exception ex)
            {
                Result.Cacheable = false;
                Result.JsonData = JsonUtil.Serialize(new JsonResult(new { Reason = ex.Message }, ErrCode.CMD_ERR), Indent);
                Debug.Print(ex.Message);
            }
            return Result;
        }
    }
}
