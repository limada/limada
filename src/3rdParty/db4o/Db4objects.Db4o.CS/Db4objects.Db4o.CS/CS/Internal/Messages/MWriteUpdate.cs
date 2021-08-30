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
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MWriteUpdate : MsgObject, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			int classMetadataID = _payLoad.ReadInt();
			int arrayTypeValue = _payLoad.ReadInt();
			ArrayType arrayType = ArrayType.ForValue(arrayTypeValue);
			Unmarshall(_payLoad._offset);
			lock (ContainerLock())
			{
				ClassMetadata classMetadata = LocalContainer().ClassMetadataForID(classMetadataID
					);
				int id = _payLoad.GetID();
				Transaction().DontDelete(id);
				Slot clientSlot = _payLoad.Slot();
				Slot newSlot = null;
				if (clientSlot.IsUpdate())
				{
					Transaction().WriteUpdateAdjustIndexes(id, classMetadata, arrayType);
					newSlot = LocalContainer().AllocateSlotForUserObjectUpdate(_payLoad.Transaction()
						, _payLoad.GetID(), _payLoad.Length());
				}
				else
				{
					if (clientSlot.IsNew())
					{
						// Just one known usecase for this one: For updating plain objects from old versions, since
						// they didnt't have own slots that could be freed.
						// Logic that got us here in OpenTypeHandler7#addReference()#writeUpdate()
						newSlot = LocalContainer().AllocateSlotForNewUserObject(_payLoad.Transaction(), _payLoad
							.GetID(), _payLoad.Length());
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				_payLoad.Address(newSlot.Address());
				classMetadata.AddFieldIndices(_payLoad);
				_payLoad.WriteEncrypt();
				DeactivateCacheFor(id);
			}
		}

		private void DeactivateCacheFor(int id)
		{
			ObjectReference reference = Transaction().ReferenceForId(id);
			if (null == reference)
			{
				return;
			}
			reference.Deactivate(Transaction(), new FixedActivationDepth(1));
		}
	}
}
