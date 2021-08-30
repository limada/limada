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

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class ArrayType
	{
		public static readonly Db4objects.Db4o.Internal.ArrayType None = new Db4objects.Db4o.Internal.ArrayType
			(0);

		public static readonly Db4objects.Db4o.Internal.ArrayType PlainArray = new Db4objects.Db4o.Internal.ArrayType
			(3);

		public static readonly Db4objects.Db4o.Internal.ArrayType MultidimensionalArray = 
			new Db4objects.Db4o.Internal.ArrayType(4);

		private ArrayType(int value)
		{
			_value = value;
		}

		private readonly int _value;

		public virtual int Value()
		{
			return _value;
		}

		public static Db4objects.Db4o.Internal.ArrayType ForValue(int value)
		{
			switch (value)
			{
				case 0:
				{
					return None;
				}

				case 3:
				{
					return PlainArray;
				}

				case 4:
				{
					return MultidimensionalArray;
				}

				default:
				{
					throw new ArgumentException();
				}
			}
		}
	}
}
