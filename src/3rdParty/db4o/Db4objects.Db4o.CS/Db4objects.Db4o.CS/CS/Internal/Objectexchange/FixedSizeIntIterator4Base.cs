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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public abstract class FixedSizeIntIterator4Base : IFixedSizeIntIterator4
	{
		private readonly int _size;

		private int _current;

		private int _available;

		public FixedSizeIntIterator4Base(int size)
		{
			this._size = size;
			_available = size;
		}

		public virtual int Size()
		{
			return _size;
		}

		public virtual int CurrentInt()
		{
			return _current;
		}

		public virtual object Current
		{
			get
			{
				return _current;
			}
		}

		public virtual bool MoveNext()
		{
			if (_available > 0)
			{
				--_available;
				_current = NextInt();
				return true;
			}
			return false;
		}

		protected abstract int NextInt();

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
