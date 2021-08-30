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
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class DoubleHandler : LongHandler
	{
		private static readonly double Defaultvalue = System.Convert.ToDouble(0);

		public override object Coerce(IReflectClass claxx, object obj)
		{
			return Coercion4.ToDouble(obj);
		}

		public override object DefaultValue()
		{
			return Defaultvalue;
		}

		public override Type PrimitiveJavaClass()
		{
			return typeof(double);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public override object Read(MarshallerFamily mf, StatefulBuffer buffer, bool redirect
			)
		{
			return mf._primitive.ReadDouble(buffer);
		}

		internal override object Read1(ByteArrayBuffer buffer)
		{
			return PrimitiveMarshaller().ReadDouble(buffer);
		}

		public override void Write(object a_object, ByteArrayBuffer a_bytes)
		{
			a_bytes.WriteLong(Platform4.DoubleToLong(((double)a_object)));
		}

		public override object Read(IReadContext context)
		{
			long l = (long)base.Read(context);
			return Platform4.LongToDouble(l);
		}

		public override void Write(IWriteContext context, object obj)
		{
			context.WriteLong(Platform4.DoubleToLong(((double)obj)));
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			double sourceDouble = ((double)source);
			return new _IPreparedComparison_55(sourceDouble);
		}

		private sealed class _IPreparedComparison_55 : IPreparedComparison
		{
			public _IPreparedComparison_55(double sourceDouble)
			{
				this.sourceDouble = sourceDouble;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				double targetDouble = ((double)target);
				return sourceDouble == targetDouble ? 0 : (sourceDouble < targetDouble ? -1 : 1);
			}

			private readonly double sourceDouble;
		}
	}
}
