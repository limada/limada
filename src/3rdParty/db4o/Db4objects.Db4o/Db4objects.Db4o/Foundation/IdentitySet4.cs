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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class IdentitySet4 : HashtableBase, IEnumerable
	{
		public IdentitySet4()
		{
		}

		public IdentitySet4(int size) : base(size)
		{
		}

		public virtual bool Contains(object obj)
		{
			return FindWithSameKey(new HashtableIdentityEntry(obj)) != null;
		}

		public virtual void Add(object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			PutEntry(new HashtableIdentityEntry(obj));
		}

		public virtual void Remove(object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			RemoveIntEntry(Runtime.IdentityHashCode(obj));
		}

		public virtual IEnumerator GetEnumerator()
		{
			return ValuesIterator();
		}
	}
}
