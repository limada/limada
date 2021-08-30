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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.TA
{
	/// <summary>
	/// An
	/// <see cref="Db4objects.Db4o.Activation.IActivator">Db4objects.Db4o.Activation.IActivator
	/// 	</see>
	/// implementation that activates an object on a specific
	/// transaction.
	/// </summary>
	/// <exclude></exclude>
	internal sealed class TransactionalActivator : IActivator
	{
		private readonly Transaction _transaction;

		private readonly ObjectReference _objectReference;

		public TransactionalActivator(Transaction transaction, ObjectReference objectReference
			)
		{
			_objectReference = objectReference;
			_transaction = transaction;
		}

		public void Activate(ActivationPurpose purpose)
		{
			_objectReference.ActivateOn(_transaction, purpose);
		}
	}
}
