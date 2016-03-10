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
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace EpgTimer
{
    public class Reader
    {
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;

        public Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }

        public string ReadLine(int timeOutMillisecs)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                return input;
            else
                return null;
        }
    }
    public class CtrlCmdConnect
    {
        public static void Connect()
        {

            Console.WriteLine("EpgTimer接続中...");
            if (!PrivateSetting.Instance.CmdConnect.
                StartConnect(Setting.Instance.CtrlHost, (int)Setting.Instance.CallbackPort, (int)Setting.Instance.CtrlPort))
            {
                Console.WriteLine("{0}:{1} ({2})に接続できません。", Setting.Instance.CtrlHost, Setting.Instance.CtrlPort, Setting.Instance.CallbackPort);
                Environment.Exit(1);
            }
            Console.WriteLine("データ読み込み中...");
            CommonManager.Instance.DB.ReloadAll(true);
        }
        private int OutsideCmdCallback(object pParam, CMD_STREAM pCmdParam, ref CMD_STREAM pResParam)
        {
            Trace.WriteLine((CtrlCmd)pCmdParam.uiParam);
            switch ((CtrlCmd)pCmdParam.uiParam)
            {
                case CtrlCmd.CMD_TIMER_GUI_SHOW_DLG:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_UPDATE_RESERVE:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.ReserveInfo);
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.RecInfo);
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.AutoAddEpgInfo);
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.AutoAddManualInfo);

                        CommonManager.Instance.DB.ReloadReserveInfo();
                        CommonManager.Instance.DB.ReloadRecFileInfo();
                        CommonManager.Instance.DB.ReloadEpgAutoAddInfo();
                        CommonManager.Instance.DB.ReloadManualAutoAddInfo();

                        WebCache.Instance.Clear();
                        SocketAction.SendAllMessage("UPDATED RESERVE");
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_UPDATE_EPGDATA:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.EpgData);
                        CommonManager.Instance.DB.ReloadEpgData();
                        WebCache.Instance.Clear();
                        SocketAction.SendAllMessage("UPDATED EPG");
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_VIEW_EXECUTE:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                        String exeCmd = "";
                        (new CtrlCmdReader(new System.IO.MemoryStream(pCmdParam.bData, false))).Read(ref exeCmd);
                        try
                        {
                            string[] cmd = exeCmd.Split('\"');
                            Process process;
                            if (cmd.Length >= 3)
                            {
                                process = Process.Start(cmd[1], cmd[2]);
                            }
                            else if (cmd.Length >= 2)
                            {
                                process = Process.Start(cmd[1]);
                            }
                            else
                            {
                                process = Process.Start(cmd[0]);
                            }
                            var w = new CtrlCmdWriter(new System.IO.MemoryStream());
                            w.Write(process.Id);
                            w.Stream.Close();
                            pResParam.bData = w.Stream.ToArray();
                            pResParam.uiSize = (uint)pResParam.bData.Length;
                        }
                        catch
                        {
                        }
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_QUERY_SUSPEND:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_QUERY_REBOOT:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_SRV_STATUS_CHG:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                        UInt16 status = 0;
                        (new CtrlCmdReader(new System.IO.MemoryStream(pCmdParam.bData, false))).Read(ref status);

                        if (status == 1) //Rec
                        {
                        }
                        else if (status == 2) //EPG
                        {
                        }
                        else
                        {

                        }

                    }
                    break;
                case CtrlCmd.CMD_TIMER_GUI_SRV_STATUS_NOTIFY2:
                    {
                        pResParam.uiParam = (uint)ErrCode.CMD_SUCCESS;
                        var Status = new NotifySrvInfo();
                        var r = new CtrlCmdReader(new System.IO.MemoryStream(pCmdParam.bData, false));
                        ushort version = 0;
                        r.Read(ref version);
                        r.Version = version;
                        r.Read(ref Status);
                        var Notify = new NotifySrvInfoItem();
                        Notify.NotifyInfo = Status;

                        if (Notify.Title != "")
                        {
                            SocketAction.SendAllMessage("EVENT " + JsonUtil.Serialize(Notify, false));
                            //Console.WriteLine("\n" + (Notify.Title + Notify.LogText).Replace("\n", ""));
                            EventStore.Instance.AddMessage(Notify);
                        }
                        NotifyStatus(Status);
                        Debug.Print(JsonUtil.Serialize(Notify));
                    }
                    break;
                default:
                    pResParam.uiParam = (uint)ErrCode.CMD_NON_SUPPORT;
                    break;
            }
            return 0;
        }
        private void NotifyStatus(NotifySrvInfo status)
        {
            if ((UpdateNotifyItem)status.notifyID != UpdateNotifyItem.SrvStatus)
                WebCache.Instance.Clear();
            switch ((UpdateNotifyItem)status.notifyID)
            {
                case UpdateNotifyItem.EpgData:
                    {
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.EpgData);
                        CommonManager.Instance.DB.ReloadEpgData();
                        SocketAction.SendAllMessage("UPDATED EPG");
                    }
                    break;
                case UpdateNotifyItem.ReserveInfo:
                    {
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.ReserveInfo);
                        CommonManager.Instance.DB.ReloadReserveInfo();
                        SocketAction.SendAllMessage("UPDATED RESERVE");
                    }
                    break;
                case UpdateNotifyItem.RecInfo:
                    {
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.RecInfo);
                        CommonManager.Instance.DB.ReloadRecFileInfo();
                        SocketAction.SendAllMessage("UPDATED REC");
                    }
                    break;
                case UpdateNotifyItem.AutoAddEpgInfo:
                    {
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.AutoAddEpgInfo);
                        CommonManager.Instance.DB.ReloadEpgAutoAddInfo();
                        SocketAction.SendAllMessage("UPDATED AUTO");
                    }
                    break;
                case UpdateNotifyItem.AutoAddManualInfo:
                    {
                        CommonManager.Instance.DB.SetUpdateNotify((UInt32)UpdateNotifyItem.AutoAddManualInfo);
                        CommonManager.Instance.DB.ReloadManualAutoAddInfo();
                        SocketAction.SendAllMessage("UPDATED MAN");
                    }
                    break;
                case UpdateNotifyItem.SrvStatus:
                    {
                        if (status.param1 == 1)
                        {
                            Console.Title = "録画中: EpgTimerWeb2";
                        }
                        else if (status.param1 == 2)
                        {
                            Console.Title = "EPG取得中: EpgTimerWeb2";
                        }
                        else
                        {
                            Console.Title = "EpgTimerWeb2";
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public bool StopConnect()
        {
            if (!CommonManager.Instance.NW.IsConnected) return true;
            if ((ErrCode)CommonManager.Instance.CtrlCmd.SendUnRegistTCP(CommonManager.Instance.NW.CallbackPort) != ErrCode.CMD_SUCCESS ||
            !CommonManager.Instance.NW.StopTCPServer()) return false;
            return true;
        }
        public bool StartConnect(string IP, int TcpPort, int CtrlPort)
        {
            var ping = new Ping();
            if (ping.Send(IP).Status != IPStatus.Success)
            {
                Console.Write("サーバーがPINGに応答しません。接続しますか？\n10秒でキャンセルされます。(y/n):");
                string Res = new Reader().ReadLine(10000);
                if (Res == null || !Res.ToLower().StartsWith("y"))
                    return false;
            }
            CommonManager.Instance.NWMode = true;
            if (!CommonManager.Instance.NW.ConnectServer(IP, (uint)CtrlPort
                , (uint)TcpPort, OutsideCmdCallback, this)) return false;
            byte[] binData;
            if (CommonManager.Instance.CtrlCmd.SendFileCopy("ChSet5.txt", out binData) == ErrCode.CMD_SUCCESS)
            {
                string filePath = @".\Setting";
                Directory.CreateDirectory(filePath);
                filePath += @"\ChSet5.txt";
                using (BinaryWriter w = new BinaryWriter(File.Create(filePath)))
                {
                    w.Write(binData);
                    w.Close();
                }
                ChSet5.LoadFile();
                return true;
            }
            return false;
        }
    }
}
