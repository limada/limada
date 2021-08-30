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

namespace Db4objects.Db4o.Foundation
{
	public class NoDuplicatesQueue : IQueue4
	{
		private IQueue4 _queue;

		private Hashtable4 _seen;

		public NoDuplicatesQueue(IQueue4 queue)
		{
			_queue = queue;
			_seen = new Hashtable4();
		}

		public virtual void Add(object obj)
		{
			if (_seen.ContainsKey(obj))
			{
				return;
			}
			_queue.Add(obj);
			_seen.Put(obj, obj);
		}

		public virtual bool HasNext()
		{
			return _queue.HasNext();
		}

		public virtual IEnumerator Iterator()
		{
			return _queue.Iterator();
		}

		public virtual object Next()
		{
			return _queue.Next();
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			return _queue.NextMatching(condition);
		}
	}
}
