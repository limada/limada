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

namespace Db4objects.Db4o.Foundation
{
	public class Collections4
	{
		public static ISequence4 UnmodifiableList(ISequence4 orig)
		{
			return new Collections4.UnmodifiableSequence4(orig);
		}

		private class UnmodifiableSequence4 : ISequence4
		{
			private ISequence4 _sequence;

			public UnmodifiableSequence4(ISequence4 sequence)
			{
				_sequence = sequence;
			}

			public virtual bool Add(object element)
			{
				throw new NotSupportedException();
			}

			public virtual void AddAll(IEnumerable iterable)
			{
				throw new NotSupportedException();
			}

			public virtual bool IsEmpty()
			{
				return _sequence.IsEmpty();
			}

			public virtual IEnumerator GetEnumerator()
			{
				return _sequence.GetEnumerator();
			}

			public virtual object Get(int index)
			{
				return _sequence.Get(index);
			}

			public virtual int Size()
			{
				return _sequence.Size();
			}

			public virtual void Clear()
			{
				throw new NotSupportedException();
			}

			public virtual bool Remove(object obj)
			{
				throw new NotSupportedException();
			}

			public virtual bool Contains(object obj)
			{
				return _sequence.Contains(obj);
			}

			public virtual bool ContainsAll(IEnumerable iter)
			{
				return _sequence.ContainsAll(iter);
			}

			public virtual object[] ToArray()
			{
				return _sequence.ToArray();
			}

			public virtual object[] ToArray(object[] array)
			{
				return _sequence.ToArray(array);
			}
		}
	}
}
