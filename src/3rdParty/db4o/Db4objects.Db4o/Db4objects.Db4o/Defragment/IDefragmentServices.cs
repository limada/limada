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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Mapping;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Encapsulates services involving source and target database files during defragmenting.
	/// 	</summary>
	/// <remarks>Encapsulates services involving source and target database files during defragmenting.
	/// 	</remarks>
	/// <exclude></exclude>
	public interface IDefragmentServices : IIDMapping
	{
		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer SourceBufferByAddress(int address, int length);

		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer TargetBufferByAddress(int address, int length);

		ByteArrayBuffer SourceBufferByID(int sourceID);

		Slot AllocateTargetSlot(int targetLength);

		void TargetWriteBytes(ByteArrayBuffer targetPointerReader, int targetAddress);

		Transaction SystemTrans();

		void TargetWriteBytes(DefragmentContextImpl context, int targetAddress);

		void TraverseAllIndexSlots(BTree tree, IVisitor4 visitor4);

		void RegisterBTreeIDs(BTree tree, IDMappingCollector collector);

		ClassMetadata ClassMetadataForId(int id);

		int MappedID(int id);

		void RegisterUnindexed(int id);

		IdSource UnindexedIDs();

		int SourceAddressByID(int sourceID);

		int TargetAddressByID(int sourceID);

		int TargetNewId();

		IIdMapping Mapping();

		void CommitIds();
	}
}
