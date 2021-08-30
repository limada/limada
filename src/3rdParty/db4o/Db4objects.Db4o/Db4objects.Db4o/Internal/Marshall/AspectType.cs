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

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class AspectType
	{
		public readonly byte _id;

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectType Field = new Db4objects.Db4o.Internal.Marshall.AspectType
			((byte)1);

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectType Translator = 
			new Db4objects.Db4o.Internal.Marshall.AspectType((byte)2);

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectType Typehandler = 
			new Db4objects.Db4o.Internal.Marshall.AspectType((byte)3);

		private AspectType(byte id)
		{
			_id = id;
		}

		public static Db4objects.Db4o.Internal.Marshall.AspectType ForByte(byte b)
		{
			switch (b)
			{
				case 1:
				{
					return Field;
				}

				case 2:
				{
					return Translator;
				}

				case 3:
				{
					return Typehandler;
				}

				default:
				{
					throw new ArgumentException();
				}
			}
		}

		public virtual bool IsFieldMetadata()
		{
			return IsField() || IsTranslator();
		}

		public virtual bool IsTranslator()
		{
			return this == Db4objects.Db4o.Internal.Marshall.AspectType.Translator;
		}

		public virtual bool IsField()
		{
			return this == Db4objects.Db4o.Internal.Marshall.AspectType.Field;
		}
	}
}
