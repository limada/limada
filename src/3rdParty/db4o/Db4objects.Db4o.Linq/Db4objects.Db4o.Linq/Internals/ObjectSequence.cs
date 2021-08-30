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
using System.Collections.Generic;

using Db4objects.Db4o;

namespace Db4objects.Db4o.Linq.Internals
{
	/// <summary>
	/// A generic wrapper around a not generic IEnumerable,
	/// Faithfully hoping that all items in the enumeration
	/// are of the same kind, otherwise it will throw a
	/// ClassCastException on access.
	/// </summary>
	/// <typeparam name="T">The type of the items</typeparam>
	public class ObjectSequence<T> : IEnumerable<T>
	{
		private IEnumerable _enumerable;

		public ObjectSequence(IEnumerable enumerable)
		{
			_enumerable = enumerable;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ObjectSequenceEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal class ObjectSequenceEnumerator : IEnumerator<T>
		{
			private IEnumerator _enumerator;

			public T Current {
				get { return (T)_enumerator.Current; }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public ObjectSequenceEnumerator(ObjectSequence<T> sequence)
			{
				_enumerator = sequence._enumerable.GetEnumerator();
			}

			public bool MoveNext()
			{
				return _enumerator.MoveNext();
			}

			public void Reset()
			{
				_enumerator.Reset();
			}

			public void Dispose()
			{
				IDisposable enumerator = _enumerator as IDisposable;
				if (enumerator == null) return;

				enumerator.Dispose();
			}
		}
	}
}
