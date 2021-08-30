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
namespace Db4objects.Db4o.Qlin
{
	/// <summary>
	/// Internal implementation class, access should not be necessary,
	/// except for implementors.
	/// </summary>
	/// <remarks>
	/// Internal implementation class, access should not be necessary,
	/// except for implementors.
	/// Use the static methods in
	/// <see cref="QLinSupport">QLinSupport</see>
	/// 
	/// <see cref="QLinSupport.Ascending()">QLinSupport.Ascending()</see>
	/// and
	/// <see cref="QLinSupport.Descending()">QLinSupport.Descending()</see>
	/// </remarks>
	/// <exclude></exclude>
	public class QLinOrderByDirection
	{
		private readonly string _direction;

		private readonly bool _ascending;

		private QLinOrderByDirection(string direction, bool ascending)
		{
			_direction = direction;
			_ascending = ascending;
		}

		internal static readonly Db4objects.Db4o.Qlin.QLinOrderByDirection Ascending = new 
			Db4objects.Db4o.Qlin.QLinOrderByDirection("ascending", true);

		internal static readonly Db4objects.Db4o.Qlin.QLinOrderByDirection Descending = new 
			Db4objects.Db4o.Qlin.QLinOrderByDirection("descending", false);

		public virtual bool IsAscending()
		{
			return _ascending;
		}

		public virtual bool IsDescending()
		{
			return !_ascending;
		}

		public override string ToString()
		{
			return _direction;
		}
	}
}
