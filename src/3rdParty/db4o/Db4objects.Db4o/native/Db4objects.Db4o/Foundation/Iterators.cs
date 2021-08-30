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
using System.Collections.Generic;

namespace Db4objects.Db4o.Foundation
{
	public delegate B Function<A, B>(A a);

	public struct Tuple<A, B>
	{
		public A a;
		public B b;

		public Tuple(A a, B b)
		{
			this.a = a;
			this.b = b;
		}
	}

	public partial class Iterators
	{
		public static IEnumerator Map(System.Array array, IFunction4 function)
		{
			return Map(array.GetEnumerator(), function);
		}

		public static IEnumerable<T> Cast<T>(IEnumerable source)
		{
			foreach (object o in source) yield return (T) o;
		}

		public static IEnumerable<Tuple<object, object>> Zip(IEnumerable @as, IEnumerable bs)
		{
			return Zip(Cast<object>(@as), Cast<object>(bs));
		}

		public static IEnumerable<Tuple<A, B>> Zip<A, B>(IEnumerable<A> @as, IEnumerable<B> bs)
		{
			IEnumerator<B> bsEnumerator = bs.GetEnumerator();
			foreach (A a in @as)
			{
				if (!bsEnumerator.MoveNext())
				{
					yield break;
				}

				yield return new Tuple<A, B>(a, bsEnumerator.Current);
			}
		}

		public static IEnumerable Unique(IEnumerable enumerable)
		{
			Hashtable seen = new Hashtable();
			foreach (object item in enumerable)
			{
				if (seen.ContainsKey(item)) continue;
				seen.Add(item, item);
				yield return item;
			}
		}
	}
}
