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
	public class EnumerateIterator : MappingIterator
	{
		public sealed class Tuple
		{
			public readonly int index;

			public readonly object value;

			public Tuple(int index_, object value_)
			{
				index = index_;
				value = value_;
			}
		}

		private int _index;

		public EnumerateIterator(IEnumerator iterator) : base(iterator)
		{
			_index = 0;
		}

		public override bool MoveNext()
		{
			if (base.MoveNext())
			{
				++_index;
				return true;
			}
			return false;
		}

		protected override object Map(object current)
		{
			return new EnumerateIterator.Tuple(_index, current);
		}
	}
}
