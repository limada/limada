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
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Marshall;


namespace Db4objects.Db4o.Internal.Handlers
{
	public class UShortHandler : IntegralTypeHandler
	{
        public override Object DefaultValue(){
            return (ushort)0;
        }
      
        public override Object Read(byte[] bytes, int offset){
            offset += 1;
            return (ushort) (bytes[offset] & 255 | (bytes[--offset] & 255) << 8);
        }
      
        public override int TypeID(){
            return 24;
        }
      
        public override void Write(Object obj, byte[] bytes, int offset){
            ushort us = (ushort)obj;
            offset += 2;
            bytes[--offset] = (byte)us;
            bytes[--offset] = (byte)(us >>= 8);
        }

        public override object Read(IReadContext context)
        {
            byte[] bytes = new byte[2];
            context.ReadBytes(bytes);
            return (ushort)(
                     bytes[1] & 255 |
                    (bytes[0] & 255) << 8
                );
        }

        public override void Write(IWriteContext context, object obj)
        {
            ushort us = (ushort)obj;
            context.WriteBytes(
                new byte[] { 
                    (byte)(us>>8),
                    (byte)us,
                });
        }

        public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<ushort>(((ushort)obj));
        }
    }
}
