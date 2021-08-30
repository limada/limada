/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
namespace Db4objects.Db4o.Foundation
{
    public class CRC32
    {
        private static uint[] crcTable;

        static CRC32()
        {
            BuildCRCTable();
        }

        private static void BuildCRCTable()
        {
            uint Crc32Polynomial = 0xEDB88320;
            uint i;
            uint j;
            uint crc;
            crcTable = new uint[256];
            for (i = 0; i <= 255; i++)
            {
                crc = i;
                for (j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = ((crc) >> (1 & 0x1f)) ^ Crc32Polynomial;
                    }
                    else
                    {
                        crc = crc >> (1 & 0x1f);
                    }
                }
                crcTable[i] = crc;
            }
        }

        public static long CheckSum(byte[] buffer, int start, int count)
        {
            uint temp1;
            uint temp2;
            int i = start;
            uint crc = 0xFFFFFFFF;
            while (count-- != 0)
            {
                temp1 = (crc) >> (8 & 0x1f);
                temp2 = crcTable[(crc ^ buffer[i++]) & 0xFF];
                crc = temp1 ^ temp2;
            }
            return (long)~crc & 0xFFFFFFFFL;
        }
    }
}
