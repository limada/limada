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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeIterator : IEnumerator
	{
		private readonly Transaction _transaction;

		private readonly BTree _bTree;

		private BTreePointer _currentPointer;

		private bool _beyondEnd;

		public BTreeIterator(Transaction trans, BTree bTree)
		{
			_transaction = trans;
			_bTree = bTree;
		}

		public virtual object Current
		{
			get
			{
				if (_currentPointer == null)
				{
					throw new InvalidOperationException();
				}
				return _currentPointer.Key();
			}
		}

		public virtual bool MoveNext()
		{
			if (_beyondEnd)
			{
				return false;
			}
			if (BeforeFirst())
			{
				_currentPointer = _bTree.FirstPointer(_transaction);
			}
			else
			{
				_currentPointer = _currentPointer.Next();
			}
			_beyondEnd = (_currentPointer == null);
			return !_beyondEnd;
		}

		private bool BeforeFirst()
		{
			return _currentPointer == null;
		}

		public virtual void Reset()
		{
			throw new NotSupportedException();
		}
	}
}
