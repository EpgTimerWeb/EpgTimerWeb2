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
using EpgTimer;
using System;
using System.IO;
using System.Threading;
namespace EpgTimerWeb2
{
    class Program
    {
        public static ConsoleColor Default = ConsoleColor.DarkGray;
        public static void Main(string[] args)
        {
            Default = Console.ForegroundColor;

            Console.WriteLine("Usage: EpgTimerWeb2.EXE [-cfg=EpgTimerWeb2.xml]");
            using (var mutex = new Mutex(false, "EpgTimerWeb2"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (!mutex.WaitOne(1, false))
                {
                    Console.WriteLine("多重起動はできません。");
                    Console.ForegroundColor = Default;
                    return;
                }
                try
                {
                    foreach (string arg in args)
                    {
                        if (!arg.StartsWith("-"))
                        {
                            Console.WriteLine("コマンドラインがおかしいです");
                            Environment.Exit(1);
                        }
                        string varg = arg.Substring(1);
                        if (varg.IndexOf("=") > 0)
                        {
                            string val = varg.Substring(varg.IndexOf("=") + 1);
                            switch (varg.Substring(0, varg.IndexOf("=")))
                            {
                                case "cfg":
                                    PrivateSetting.Instance.ConfigPath = val;
                                    Console.WriteLine("設定ファイル: {0}", val);
                                    break;
                                default:
                                    Console.WriteLine("コマンドラインがおかしいです");
                                    Environment.Exit(1);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("コマンドラインがおかしいです({0})", ex.Message);
                    Environment.Exit(1);
                }
                if (File.Exists(PrivateSetting.Instance.ConfigPath))
                {
                    Setting.LoadFromXmlFile(PrivateSetting.Instance.ConfigPath);
                }
                else
                {
                    Console.WriteLine("設定ファイルがありません");
                    PrivateSetting.Instance.SetupMode = true;
                }
                Console.ForegroundColor = Default;
                if (!PrivateSetting.Instance.SetupMode)
                {
                    CtrlCmdConnect.Connect();
                }
                var r = new Random(new Random().Next(0xFFFFFF));
                for (int i = 0; i < 5; i++)
                {
                    PrivateSetting.Instance.SetupCode += r.Next(0, 99).ToString();
                }
                PrivateSetting.Instance.Server = new WebServer((int)Setting.Instance.HttpPort);
                PrivateSetting.Instance.Server.OnRequest += ServerAction.DoProcess;
                PrivateSetting.Instance.Server.Start();
                var reset = new ManualResetEvent(false);
                Console.CancelKeyPress += (a, b) =>
                {
                    PrivateSetting.Instance.Server.Stop();
                    if (!PrivateSetting.Instance.SetupMode)
                    {
                        Console.WriteLine("EpgTimerから切断...");
                        if (PrivateSetting.Instance.CmdConnect.StopConnect())
                            Console.WriteLine("EpgTimerから切断済み");
                        else
                            Console.WriteLine("EpgTimerから切断できませんでした");
                        Setting.SaveToXmlFile(PrivateSetting.Instance.ConfigPath);
                    }

                    reset.Set();
                };
                if (PrivateSetting.Instance.SetupMode)
                {
                    Console.WriteLine("初期設定を行います。\nのURLに接続してください。http://localhost:" + Setting.Instance.HttpPort);
                    Console.WriteLine("PINコード: {0}", PrivateSetting.Instance.SetupCode);
                    while (PrivateSetting.Instance.SetupMode) Thread.Sleep(1);
                }

                reset.WaitOne();
                mutex.ReleaseMutex();
            }
        }
    }
}
