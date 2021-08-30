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
namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class RawClassSpec
	{
		private readonly string _name;

		private readonly int _superClassID;

		private readonly int _numFields;

		public RawClassSpec(string name, int superClassID, int numFields)
		{
			_name = name;
			_superClassID = superClassID;
			_numFields = numFields;
		}

		public virtual string Name()
		{
			return _name;
		}

		public virtual int SuperClassID()
		{
			return _superClassID;
		}

		public virtual int NumFields()
		{
			return _numFields;
		}
	}
}
