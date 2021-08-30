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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public abstract class NetTypeHandler : PrimitiveHandler, INetType
	{
		private int i_linkLength;

		public virtual string DotNetClassName()
		{
			string className = this.GetType().FullName;
			int pos = className.IndexOf(".Net");
			if (pos >= 0)
			{
				return "System." + Sharpen.Runtime.Substring(className, pos + 4) + ", mscorlib";
			}
			return DefaultValue().GetType().FullName;
		}

		public override void RegisterReflector(IReflector reflector)
		{
			base.RegisterReflector(reflector);
			byte[] bytes = new byte[65];
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = 55;
			}
			// TODO: Why 55? This is a '7'. Remove.
			Write(PrimitiveNull(), bytes, 0);
			for (int i = 0; i < bytes.Length; i++)
			{
				if (bytes[i] == 55)
				{
					i_linkLength = i;
					break;
				}
			}
		}

		public virtual int GetID()
		{
			return TypeID();
		}

		// This method is needed for NetSimpleTypeHandler only during
		// initalisation and overloaded there. No abstract declaration 
		// here, so we don't have to implement the methods on .NET.
		public virtual string GetName()
		{
			return DotNetClassName();
		}

		public override int LinkLength()
		{
			return i_linkLength;
		}

		public override Type PrimitiveJavaClass()
		{
			return DefaultValue().GetType();
		}

		protected override Type JavaClass()
		{
			return base.JavaClass();
		}

		public abstract object Read(byte[] bytes, int offset);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			int offset = a_bytes._offset;
			object ret = Read(a_bytes._buffer, a_bytes._offset);
			a_bytes._offset = offset + LinkLength();
			return ret;
		}

		public abstract int TypeID();

		public abstract void Write(object obj, byte[] bytes, int offset);

		public override void Write(object a_object, ByteArrayBuffer a_bytes)
		{
			int offset = a_bytes._offset;
			if (a_object != null)
			{
				Write(a_object, a_bytes._buffer, a_bytes._offset);
			}
			a_bytes._offset = offset + LinkLength();
		}

		public override IPreparedComparison InternalPrepareComparison(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
