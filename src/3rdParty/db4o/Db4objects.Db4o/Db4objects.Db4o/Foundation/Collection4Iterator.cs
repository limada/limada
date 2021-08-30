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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Collection4Iterator : Iterator4Impl
	{
		private readonly Collection4 _collection;

		private readonly int _initialVersion;

		public Collection4Iterator(Collection4 collection, List4 first) : base(first)
		{
			_collection = collection;
			_initialVersion = CurrentVersion();
		}

		public override bool MoveNext()
		{
			Validate();
			return base.MoveNext();
		}

		public override object Current
		{
			get
			{
				Validate();
				return base.Current;
			}
		}

		private void Validate()
		{
			if (_initialVersion != CurrentVersion())
			{
				// FIXME: change to ConcurrentModificationException
				throw new InvalidIteratorException();
			}
		}

		private int CurrentVersion()
		{
			return _collection.Version();
		}
	}
}
