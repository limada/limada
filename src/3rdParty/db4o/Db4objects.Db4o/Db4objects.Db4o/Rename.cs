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
using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <summary>
	/// Renaming actions are stored to the database file to make
	/// sure that they are only performed once.
	/// </summary>
	/// <remarks>
	/// Renaming actions are stored to the database file to make
	/// sure that they are only performed once.
	/// </remarks>
	/// <exclude></exclude>
	/// <persistent></persistent>
	public sealed class Rename : IInternal4
	{
		public string rClass;

		public string rFrom;

		public string rTo;

		public Rename()
		{
		}

		public Rename(string aClass, string aFrom, string aTo)
		{
			rClass = aClass;
			rFrom = aFrom;
			rTo = aTo;
		}

		public bool IsField()
		{
			return rClass.Length != 0;
		}
	}
}
