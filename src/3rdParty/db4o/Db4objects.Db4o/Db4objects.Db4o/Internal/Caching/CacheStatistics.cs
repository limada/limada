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
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public class CacheStatistics : ICache4
	{
		private readonly ICache4 _delegate;

		private int _calls;

		private int _misses;

		public CacheStatistics(ICache4 delegate_)
		{
			_delegate = delegate_;
		}

		public virtual object Produce(object key, IFunction4 producer, IProcedure4 onDiscard
			)
		{
			_calls++;
			IFunction4 delegateProducer = new _IFunction4_26(this, producer);
			return _delegate.Produce(key, delegateProducer, onDiscard);
		}

		private sealed class _IFunction4_26 : IFunction4
		{
			public _IFunction4_26(CacheStatistics _enclosing, IFunction4 producer)
			{
				this._enclosing = _enclosing;
				this.producer = producer;
			}

			public object Apply(object arg)
			{
				this._enclosing._misses++;
				return producer.Apply(arg);
			}

			private readonly CacheStatistics _enclosing;

			private readonly IFunction4 producer;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _delegate.GetEnumerator();
		}

		public virtual int Calls()
		{
			return _calls;
		}

		public virtual int Misses()
		{
			return _misses;
		}

		public override string ToString()
		{
			return "Cache statistics  Calls:" + _calls + " Misses:" + _misses;
		}
	}
}
