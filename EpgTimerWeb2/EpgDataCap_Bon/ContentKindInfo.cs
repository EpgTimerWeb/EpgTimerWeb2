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
    public class ContentKindInfo
    {
        public ContentKindInfo(string contentName, string subName, byte nibble1, byte nibble2)
        {
            this.ContentName = contentName;
            this.SubName = subName;
            this.Nibble1 = nibble1;
            this.Nibble2 = nibble2;
            this.ID = (ushort)(((ushort)nibble1) << 8 | nibble2);
        }
        public ushort ID { get; set; }
        public string ContentName { get; set; }
        public string SubName { get; set; }
        public byte Nibble1 { get; set; }
        public byte Nibble2 { get; set; }
        public override string ToString()
        {
            if (Nibble2 == 0xFF)
                return ContentName;
            else
                return "  " + SubName;
        }
    }
}
