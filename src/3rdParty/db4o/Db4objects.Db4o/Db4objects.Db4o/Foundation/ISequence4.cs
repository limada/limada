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

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public interface ISequence4 : IEnumerable
	{
		bool Add(object element);

		void AddAll(IEnumerable iterable);

		bool IsEmpty();

		object Get(int index);

		int Size();

		void Clear();

		bool Remove(object obj);

		bool Contains(object obj);

		bool ContainsAll(IEnumerable iter);

		object[] ToArray();

		object[] ToArray(object[] array);
	}
}
