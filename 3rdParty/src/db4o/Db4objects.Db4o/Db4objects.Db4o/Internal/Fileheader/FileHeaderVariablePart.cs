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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Fileheader
{
	/// <exclude></exclude>
	public abstract class FileHeaderVariablePart
	{
		protected readonly LocalObjectContainer _container;

		public abstract IRunnable Commit(bool shuttingDown);

		public abstract void Read(int variablePartAddress, int variablePartLength);

		protected FileHeaderVariablePart(LocalObjectContainer container)
		{
			_container = container;
		}

		public byte GetIdentifier()
		{
			return Const4.Header;
		}

		protected Db4objects.Db4o.Internal.SystemData SystemData()
		{
			return _container.SystemData();
		}

		protected Slot AllocateSlot(int length)
		{
			Slot reusedSlot = _container.FreespaceManager().AllocateSafeSlot(length);
			if (reusedSlot != null)
			{
				return reusedSlot;
			}
			return _container.AppendBytes(length);
		}

		public virtual void ReadIdentity(LocalTransaction trans)
		{
			LocalObjectContainer file = trans.LocalContainer();
			Db4oDatabase identity = Debug4.staticIdentity ? 
				Db4oDatabase.StaticIdentity : 
				(Db4oDatabase)file.GetByID(trans, SystemData().IdentityId());
			if (null != identity)
			{
				file.Activate(trans, identity, new FixedActivationDepth(2));
				SystemData().Identity(identity);
			}
		}

		// TODO: What now?
		// Apparently we get this state after defragment
		// and defragment then sets the identity.
		// If we blindly generate a new identity here,
		// ObjectUpdateFileSizeTestCase reports trouble.
		public abstract int MarshalledLength();
	}
}
