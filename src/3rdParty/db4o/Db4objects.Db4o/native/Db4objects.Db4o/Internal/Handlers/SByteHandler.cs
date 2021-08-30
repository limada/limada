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
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	public class SByteHandler : IntegralTypeHandler
	{
		public override object Coerce(IReflectClass claxx, object obj)
		{
			return Coercion4.ToSByte(obj);
		}
	
        public override Object DefaultValue(){
            return (sbyte)0;
        }
      
        public override Object Read(byte[] bytes, int offset){
            return (sbyte)  ((bytes[offset]) - 128) ;
        }

        public override int TypeID(){
            return 20;
        }
      
        public override void Write(Object obj, byte[] bytes, int offset){
            bytes[offset] = (byte)(((sbyte)obj) + 128);
        }

        public override object Read(IReadContext context)
        {
            return (sbyte)(context.ReadByte() - 128);
        }

        public override void Write(IWriteContext context, object obj)
        {
            context.WriteByte((byte)(((sbyte)obj) + 128));
        }
        
        public override IPreparedComparison InternalPrepareComparison(object obj)
        {
            return new PreparedComparisonFor<sbyte>(((sbyte)obj));
        }
    }
}
