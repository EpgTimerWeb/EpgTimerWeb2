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
using System.Threading;

namespace EpgTimer
{
    public class HttpCommon
    {
        public static string StreamReadLine(Stream Input)
        {
            int Next;
            string Data = "";
            int To = 0;
            while (true)
            {
                Next = Input.ReadByte();
                if (Next == '\n') break;
                if (Next == '\r') { continue; }
                if (Next == -1)
                {
                    Thread.Sleep(1);
                    To++;
                    if (To > 1000) throw new TimeoutException();
                    continue;
                }
                Data += Convert.ToChar(Next);
            }
            return Data;
        }
    }
}
