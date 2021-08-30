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

namespace Db4objects.Db4o.Foundation
{
	public class Coercion4
	{
		public static object ToByte(object obj)
		{
			if (obj is byte) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToByte(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToSByte(object obj)
		{
			if (obj is sbyte) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToSByte(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToShort(object obj)
		{
			if (obj is short) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToInt16(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToInt(object obj)
		{
			if (obj is int) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToInt32(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToLong(object obj)
		{
			if (obj is long) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToInt64(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToFloat(object obj)
		{
			if (obj is float) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToSingle(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}

		public static object ToDouble(object obj)
		{
			if (obj is double) return obj;

			IConvertible convertible = obj as IConvertible;
			if (null != convertible) return convertible.ToDouble(null);
			return Db4objects.Db4o.Foundation.No4.Instance;
		}
	}
}

