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
	public class UIntHandler : IntegralTypeHandler
	{
        public override Object DefaultValue(){
            return (uint)0;
        }
      
        public override Object Read(byte[] bytes, int offset){
            offset += 3;
            return (uint) (bytes[offset] & 255 | (bytes[--offset] & 255) << 8 | (bytes[--offset] & 255) << 16 | bytes[--offset] << 24);
        }

        public override int TypeID(){
            return 22;
        }
      
        public override void Write(Object obj, byte[] bytes, int offset){
            uint ui = (uint)obj;
            offset += 4;
            bytes[--offset] = (byte)ui;
            bytes[--offset] = (byte)(ui >>= 8);
            bytes[--offset] = (byte)(ui >>= 8);
            bytes[--offset] = (byte)(ui >>= 8);
        }
        public override object Read(IReadContext context)
        {
            byte[] bytes = new byte[4];
            context.ReadBytes(bytes);
            return (uint)(
                     bytes[3] & 255        | 
                    (bytes[2] & 255) << 8  | 
                    (bytes[1] & 255) << 16 | 
                    (bytes[0] & 255) << 24
                );
        }

        public override void Write(IWriteContext context, object obj)
        {
            uint ui = (uint)obj;
            context.WriteBytes(
                new byte[] { 
                    (byte)(ui>>24),
                    (byte)(ui>>16),
                    (byte)(ui>>8),
                    (byte)ui,
                });
        }

        public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<uint>(((uint)obj));
        }
    }
}
