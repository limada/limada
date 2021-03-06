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
#if !SILVERLIGHT

using System;
using System.Collections;

namespace Db4objects.Db4o.Config
{
	/// <exclude />
	public class TQueue : IObjectTranslator
	{
		public void OnActivate(IObjectContainer objectContainer, object obj, object members)
		{
			Queue queue = (Queue) obj;
			queue.Clear();
			if (members != null)
			{
				object[] elements = (object[])members;
				for (int i = 0; i < elements.Length; i++)
				{
					queue.Enqueue(elements[i]);
				}
			}
		}

		public Object OnStore(IObjectContainer objectContainer, object obj)
		{
			Queue queue = (Queue)obj;
			int count = queue.Count;
			object[] elements = new object[count];
			IEnumerator e = queue.GetEnumerator();
			e.Reset();
			for (int i = 0; i < count; i++)
			{
				e.MoveNext();
				elements[i] = e.Current;
			}
			return elements;
		}

		public System.Type StoredClass()
		{
			return typeof(object[]);
		}
	}
}

#endif