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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	public interface IDefragmentContext : IBufferContext, IMarshallingInfo, IHandlerVersionContext
	{
		ITypeHandler4 TypeHandlerForId(int id);

		int CopyID();

		int CopyIDReturnOriginalID();

		int CopySlotlessID();

		int CopyUnindexedID();

		void Defragment(ITypeHandler4 handler);

		int HandlerVersion();

		void IncrementOffset(int length);

		bool IsLegacyHandlerVersion();

		int MappedID(int origID);

		ByteArrayBuffer SourceBuffer();

		ByteArrayBuffer TargetBuffer();

		Slot AllocateTargetSlot(int length);

		Slot AllocateMappedTargetSlot(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		int CopySlotToNewMapped(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer SourceBufferByAddress(int sourceAddress, int length);

		/// <exception cref="System.IO.IOException"></exception>
		ByteArrayBuffer SourceBufferById(int sourceId);

		void TargetWriteBytes(int address, ByteArrayBuffer buffer);

		IDefragmentServices Services();

		ObjectContainerBase Container();
	}
}
