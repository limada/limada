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
using System.IO;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class StringHandler0 : StringHandler
	{
		public override object Read(IReadContext context)
		{
			ByteArrayBuffer buffer = (ByteArrayBuffer)((IInternalReadContext)context).ReadIndirectedBuffer
				();
			if (buffer == null)
			{
				return null;
			}
			return ReadString(context, buffer);
		}

		public override void Delete(IDeleteContext context)
		{
			context.DefragmentRecommended();
		}

		public override void Defragment(IDefragmentContext context)
		{
			int sourceAddress = context.SourceBuffer().ReadInt();
			int length = context.SourceBuffer().ReadInt();
			if (sourceAddress == 0 && length == 0)
			{
				context.TargetBuffer().WriteInt(0);
				context.TargetBuffer().WriteInt(0);
				return;
			}
			int targetAddress = 0;
			try
			{
				targetAddress = context.CopySlotToNewMapped(sourceAddress, length);
			}
			catch (IOException exc)
			{
				throw new Db4oIOException(exc);
			}
			context.TargetBuffer().WriteInt(targetAddress);
			context.TargetBuffer().WriteInt(length);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override object ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer
			 buffer)
		{
			return buffer.Container().ReadWriterByAddress(buffer.Transaction(), buffer.ReadInt
				(), buffer.ReadInt());
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override object ReadIndexEntry(IObjectIdContext context)
		{
			return context.Transaction().Container().ReadWriterByAddress(context.Transaction(
				), context.ReadInt(), context.ReadInt());
		}
	}
}
