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
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	internal class PendingClassInits
	{
		private readonly Transaction _systemTransaction;

		private Collection4 _pending = new Collection4();

		private IQueue4 _members = new NonblockingQueue();

		private IQueue4 _statics = new NonblockingQueue();

		private IQueue4 _writes = new NonblockingQueue();

		private IQueue4 _inits = new NonblockingQueue();

		private bool _running = false;

		internal PendingClassInits(Transaction systemTransaction)
		{
			_systemTransaction = systemTransaction;
		}

		internal virtual void Process(ClassMetadata newClassMetadata)
		{
			if (_pending.Contains(newClassMetadata))
			{
				return;
			}
			ClassMetadata ancestor = newClassMetadata.GetAncestor();
			if (ancestor != null)
			{
				Process(ancestor);
			}
			_pending.Add(newClassMetadata);
			_members.Add(newClassMetadata);
			if (_running)
			{
				return;
			}
			_running = true;
			try
			{
				CheckInits();
				_pending = new Collection4();
			}
			finally
			{
				_running = false;
			}
		}

		private void InitializeAspects()
		{
			while (_members.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_members.Next());
				classMetadata.InitializeAspects();
				_statics.Add(classMetadata);
			}
		}

		private void CheckStatics()
		{
			InitializeAspects();
			while (_statics.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_statics.Next());
				classMetadata.StoreStaticFieldValues(_systemTransaction, true);
				_writes.Add(classMetadata);
				InitializeAspects();
			}
		}

		private void CheckWrites()
		{
			CheckStatics();
			while (_writes.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_writes.Next());
				classMetadata.SetStateDirty();
				classMetadata.Write(_systemTransaction);
				_inits.Add(classMetadata);
				CheckStatics();
			}
		}

		private void CheckInits()
		{
			CheckWrites();
			while (_inits.HasNext())
			{
				ClassMetadata classMetadata = ((ClassMetadata)_inits.Next());
				classMetadata.InitConfigOnUp(_systemTransaction);
				CheckWrites();
			}
		}
	}
}
