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

namespace EpgTimer
{
    public class _ServiceItem
    {
        public EpgServiceInfo ServiceInfo
        {
            get;
            set;
        }
        public ulong ID
        {
            get { return CommonManager.Create64Key(ServiceInfo.ONID, ServiceInfo.TSID, ServiceInfo.SID); }
        }
        public string ServiceName
        {
            get { return ServiceInfo.ServiceName; }
        }
        public string NetworkName
        {
            get
            {
                return CommonManager.GetNetworkName(ServiceInfo.ONID);
            }
        }
    }
}
