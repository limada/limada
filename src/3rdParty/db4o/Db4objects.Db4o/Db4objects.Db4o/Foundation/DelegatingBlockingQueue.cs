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
	public class DelegatingBlockingQueue : IBlockingQueue4
	{
		private IBlockingQueue4 queue;

		/// <exception cref="Db4objects.Db4o.Foundation.BlockingQueueStoppedException"></exception>
		public virtual object Next(long timeout)
		{
			return queue.Next(timeout);
		}

		public virtual object Next()
		{
			return queue.Next();
		}

		public virtual void Add(object obj)
		{
			queue.Add(obj);
		}

		public virtual bool HasNext()
		{
			return queue.HasNext();
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			return queue.NextMatching(condition);
		}

		public virtual IEnumerator Iterator()
		{
			return queue.Iterator();
		}

		public DelegatingBlockingQueue(IBlockingQueue4 queue)
		{
			this.queue = queue;
		}

		public virtual void Stop()
		{
			queue.Stop();
		}

		public virtual int DrainTo(Collection4 list)
		{
			return queue.DrainTo(list);
		}
	}
}
