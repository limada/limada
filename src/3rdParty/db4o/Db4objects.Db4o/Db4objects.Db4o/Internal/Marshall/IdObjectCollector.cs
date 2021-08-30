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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class IdObjectCollector
	{
		private TreeInt _ids;

		private List4 _objects;

		public virtual void AddId(int id)
		{
			_ids = (TreeInt)((TreeInt)Tree.Add(_ids, new TreeInt(id)));
		}

		public virtual TreeInt Ids()
		{
			return _ids;
		}

		public virtual void Add(object obj)
		{
			_objects = new List4(_objects, obj);
		}

		public virtual IEnumerator Objects()
		{
			return new Iterator4Impl(_objects);
		}
	}
}
