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
using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class ClientTransactionHandle
	{
		private readonly ClientTransactionPool _transactionPool;

		private Db4objects.Db4o.Internal.Transaction _mainTransaction;

		private Db4objects.Db4o.Internal.Transaction _transaction;

		private bool _rollbackOnClose;

		public ClientTransactionHandle(ClientTransactionPool transactionPool)
		{
			_transactionPool = transactionPool;
			_mainTransaction = _transactionPool.AcquireMain();
			_rollbackOnClose = true;
		}

		public virtual void AcquireTransactionForFile(string fileName)
		{
			_transaction = _transactionPool.Acquire(fileName);
		}

		public virtual void ReleaseTransaction(ShutdownMode mode)
		{
			if (_transaction != null)
			{
				_transactionPool.Release(mode, _transaction, _rollbackOnClose);
				_transaction = null;
			}
		}

		public virtual bool IsClosed()
		{
			return _transactionPool.IsClosed();
		}

		public virtual void Close(ShutdownMode mode)
		{
			if ((!_transactionPool.IsClosed()) && (_mainTransaction != null))
			{
				_transactionPool.Release(mode, _mainTransaction, _rollbackOnClose);
			}
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			if (_transaction != null)
			{
				return _transaction;
			}
			return _mainTransaction;
		}

		public virtual void Transaction(Db4objects.Db4o.Internal.Transaction transaction)
		{
			if (_transaction != null)
			{
				_transaction = transaction;
			}
			else
			{
				_mainTransaction = transaction;
			}
			_rollbackOnClose = false;
		}
	}
}
