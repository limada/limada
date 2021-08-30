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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	internal class WriteUpdateProcessor
	{
		private readonly LocalTransaction _transaction;

		private readonly int _id;

		private readonly ClassMetadata _clazz;

		private readonly ArrayType _typeInfo;

		private int _cascade = 0;

		public WriteUpdateProcessor(LocalTransaction transaction, int id, ClassMetadata clazz
			, ArrayType typeInfo)
		{
			_transaction = transaction;
			_id = id;
			_clazz = clazz;
			_typeInfo = typeInfo;
		}

		public virtual void Run()
		{
			_transaction.CheckSynchronization();
			if (DTrace.enabled)
			{
				DTrace.WriteUpdateAdjustIndexes.Log(_id);
			}
			if (AlreadyHandled())
			{
				return;
			}
			// TODO: Try to get rid of getting the slot here because it 
			//       will invoke reading a pointer from the file system.
			//       It may be possible to figure out the readd case
			//       by asking the IdSystem in a smarter way.
			Slot slot = _transaction.IdSystem().CurrentSlot(_id);
			if (HandledAsReAdd(slot))
			{
				return;
			}
			if (_clazz.CanUpdateFast())
			{
				return;
			}
			StatefulBuffer objectBytes = Container().ReadStatefulBufferBySlot(_transaction, _id
				, slot);
			DeleteMembers(objectBytes);
		}

		private LocalObjectContainer Container()
		{
			return _transaction.LocalContainer();
		}

		private void DeleteMembers(StatefulBuffer objectBytes)
		{
			ObjectHeader oh = new ObjectHeader(_clazz, objectBytes);
			DeleteInfo info = (DeleteInfo)TreeInt.Find(_transaction._delete, _id);
			if (info != null)
			{
				if (info._cascade > _cascade)
				{
					_cascade = info._cascade;
				}
			}
			objectBytes.SetCascadeDeletes(_cascade);
			DeleteContextImpl context = new DeleteContextImpl(objectBytes, oh, _clazz.ClassReflector
				(), null);
			_clazz.DeleteMembers(context, _typeInfo, true);
		}

		private bool HandledAsReAdd(Slot slot)
		{
			if (!Slot.IsNull(slot))
			{
				return false;
			}
			_clazz.AddToIndex(_transaction, _id);
			return true;
		}

		private bool AlreadyHandled()
		{
			TreeInt newNode = new TreeInt(_id);
			_transaction._writtenUpdateAdjustedIndexes = Tree.Add(_transaction._writtenUpdateAdjustedIndexes
				, newNode);
			return !newNode.WasAddedToTree();
		}
	}
}
