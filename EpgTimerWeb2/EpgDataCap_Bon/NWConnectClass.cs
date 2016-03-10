using System;
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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EpgTimer
{
    public class NWConnect
    {
        private CMD_CALLBACK_PROC cmdProc = null;
        private object cmdParam = null;

        private bool connectFlag;
        private uint serverPort;
        private TcpListener server = null;

        private string connectedIP;
        private uint connectedPort = 0;
        private uint callbackPort = 0;

        private CtrlCmdUtil cmd = null;

        public bool IsConnected
        {
            get
            {
                return connectFlag;
            }
        }

        public string ConnectedIP
        {
            get
            {
                return connectedIP;
            }
        }

        public uint ConnectedPort
        {
            get
            {
                return connectedPort;
            }
        }

        public uint CallbackPort
        {
            get
            {
                return callbackPort;
            }
        }

        public NWConnect(CtrlCmdUtil ctrlCmd)
        {
            connectFlag = false;
            cmd = ctrlCmd;
        }

        private static void SendMagicPacket(IPAddress broad, byte[] physicalAddress)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            for (int i = 0; i < 6; i++)
            {
                writer.Write((byte)0xff);
            }
            for (int i = 0; i < 16; i++)
            {
                writer.Write(physicalAddress);
            }

            UdpClient client = new UdpClient();
            client.EnableBroadcast = true;
            client.Send(stream.ToArray(), (int)stream.Position, new IPEndPoint(broad, 0));
        }

        public bool ConnectServer(string srvIP, uint srvPort, uint waitPort, CMD_CALLBACK_PROC pfnCmdProc, object pParam)
        {
            if (srvIP.Length == 0)
            {
                return false;
            }
            connectFlag = false;

            cmdProc = pfnCmdProc;
            cmdParam = pParam;
            if (!StartTCPServer(waitPort)) return false;
            
            cmd.SetConnectTimeOut(500);
            cmd.SetSendMode(true);
            cmd.SetNWSetting(srvIP, srvPort);
            if (cmd.SendRegistTCP(waitPort) == ErrCode.CMD_SUCCESS)
            {
                connectFlag = true;
                connectedIP = srvIP;
                connectedPort = srvPort;
                callbackPort = waitPort;
                return true;
            }
            return false;
        }

        public bool StartTCPServer(uint port)
        {
            if (serverPort == port && server != null)
            {
                return true;
            }
            if (server != null)
            {
                server.Stop();
                server = null;
            }
            try
            {
                serverPort = port;
                server = new TcpListener(IPAddress.Any, (int)port);
                server.Start();
                server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), server);
            }
            catch (Exception ex)
            {
                Debug.Print("NWConnect Error: {0}", ex.Message);
                return false;
            }
            return true;
        }

        public bool StopTCPServer()
        {
            if (server != null)
            {
                server.Stop();
                server = null;
            } 
            return true;
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            if (server == null) return;
            TcpListener listener = (TcpListener)ar.AsyncState;
            if (!this.IsConnected) return;
            try
            {
                TcpClient client = listener.EndAcceptTcpClient(ar);
                client.ReceiveBufferSize = 1024 * 1024;


                NetworkStream stream = client.GetStream();

                CMD_STREAM stCmd = new CMD_STREAM();
                CMD_STREAM stRes = new CMD_STREAM();
                //コマンド受信
                if (cmdProc != null)
                {
                    byte[] bHead = new byte[8];

                    if (stream.Read(bHead, 0, bHead.Length) == 8)
                    {
                        stCmd.uiParam = BitConverter.ToUInt32(bHead, 0);
                        stCmd.uiSize = BitConverter.ToUInt32(bHead, 4);
                        if (stCmd.uiSize > 0)
                        {
                            stCmd.bData = new byte[stCmd.uiSize];
                        }
                        int readSize = 0;
                        while (readSize < stCmd.uiSize)
                        {
                            readSize += stream.Read(stCmd.bData, readSize, (int)stCmd.uiSize);
                        }
                        cmdProc.Invoke(cmdParam, stCmd, ref stRes);

                        Array.Copy(BitConverter.GetBytes(stRes.uiParam), 0, bHead, 0, sizeof(uint));
                        Array.Copy(BitConverter.GetBytes(stRes.uiSize), 0, bHead, 4, sizeof(uint));
                        stream.Write(bHead, 0, 8);
                        if (stRes.uiSize > 0)
                        {
                            stream.Write(stRes.bData, 0, (int)stRes.uiSize);
                        }
                    }
                }
                else
                {
                    stRes.uiSize = 0;
                    stRes.uiParam = 1;
                }
                stream.Dispose();
                client.Client.Close();

                server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), server);
            }
            catch (Exception ex)
            {
                Debug.Print("NWConnect Error: {0}", ex.Message);
            }
        }
    
    }
}
