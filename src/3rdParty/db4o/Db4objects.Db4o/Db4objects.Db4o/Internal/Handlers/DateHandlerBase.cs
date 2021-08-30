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
	/// <summary>Shared (java/.net) logic for Date handling.</summary>
	/// <remarks>Shared (java/.net) logic for Date handling.</remarks>
	public abstract class DateHandlerBase : LongHandler
	{
		public override object Coerce(IReflectClass claxx, object obj)
		{
			return ClassReflector().IsAssignableFrom(claxx) ? obj : No4.Instance;
		}

		public abstract object CopyValue(object from, object to);

		public abstract override object DefaultValue();

		public abstract override object NullRepresentationInUntypedArrays();

		public override Type PrimitiveJavaClass()
		{
			return null;
		}

		protected override Type JavaClass()
		{
			return DefaultValue().GetType();
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public override object Read(MarshallerFamily mf, StatefulBuffer writer, bool redirect
			)
		{
			return mf._primitive.ReadDate(writer);
		}

		internal override object Read1(ByteArrayBuffer a_bytes)
		{
			return PrimitiveMarshaller().ReadDate(a_bytes);
		}

		public override void Write(object a_object, ByteArrayBuffer a_bytes)
		{
			// TODO: This is a temporary fix to prevent exceptions with
			// Marshaller.LEGACY.  
			if (a_object == null)
			{
				a_object = new DateTime(0);
			}
			a_bytes.WriteLong(((DateTime)a_object).Ticks);
		}

		public static string Now()
		{
			return Platform4.Format(Platform4.Now(), true);
		}

		public override object Read(IReadContext context)
		{
			long milliseconds = ((long)base.Read(context));
			return new DateTime(milliseconds);
		}

		public override void Write(IWriteContext context, object obj)
		{
			long milliseconds = ((DateTime)obj).Ticks;
			base.Write(context, milliseconds);
		}

		public override IPreparedComparison InternalPrepareComparison(object source)
		{
			long sourceDate = ((DateTime)source).Ticks;
			return new _IPreparedComparison_69(sourceDate);
		}

		private sealed class _IPreparedComparison_69 : IPreparedComparison
		{
			public _IPreparedComparison_69(long sourceDate)
			{
				this.sourceDate = sourceDate;
			}

			public int CompareTo(object target)
			{
				if (target == null)
				{
					return 1;
				}
				long targetDate = ((DateTime)target).Ticks;
				return sourceDate == targetDate ? 0 : (sourceDate < targetDate ? -1 : 1);
			}

			private readonly long sourceDate;
		}
	}
}
