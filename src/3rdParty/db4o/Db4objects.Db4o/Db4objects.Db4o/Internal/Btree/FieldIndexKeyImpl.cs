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
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <summary>
	/// Composite key for field indexes, first compares on the actual
	/// indexed field _value and then on the _parentID (which is a
	/// reference to the containing object).
	/// </summary>
	/// <remarks>
	/// Composite key for field indexes, first compares on the actual
	/// indexed field _value and then on the _parentID (which is a
	/// reference to the containing object).
	/// </remarks>
	/// <exclude></exclude>
	public class FieldIndexKeyImpl : IFieldIndexKey
	{
		private readonly object _value;

		private readonly int _parentID;

		public FieldIndexKeyImpl(int parentID, object value)
		{
			_parentID = parentID;
			_value = value;
		}

		public virtual int ParentID()
		{
			return _parentID;
		}

		public virtual object Value()
		{
			return _value;
		}

		public override string ToString()
		{
			return "FieldIndexKey(" + _parentID + ", " + SafeString(_value) + ")";
		}

		private string SafeString(object value)
		{
			if (null == value)
			{
				return "null";
			}
			return value.ToString();
		}
	}
}
