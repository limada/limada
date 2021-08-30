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
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayHandler0 : ArrayHandler1
	{
		protected override ArrayVersionHelper CreateVersionHelper()
		{
			return new ArrayVersionHelper0();
		}

		protected override void WithContent(AbstractBufferContext context, IRunnable runnable
			)
		{
			int address = context.ReadInt();
			int length = context.ReadInt();
			if (address == 0)
			{
				return;
			}
			IReadBuffer temp = context.Buffer();
			ByteArrayBuffer indirectedBuffer = Container(context).DecryptedBufferByAddress(address
				, length);
			context.Buffer(indirectedBuffer);
			runnable.Run();
			context.Buffer(temp);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Delete(IDeleteContext context)
		{
			context.ReadSlot();
			context.DefragmentRecommended();
		}

		public override object Read(IReadContext readContext)
		{
			IInternalReadContext context = (IInternalReadContext)readContext;
			ByteArrayBuffer buffer = (ByteArrayBuffer)context.ReadIndirectedBuffer();
			if (buffer == null)
			{
				return null;
			}
			// With the following line we ask the context to work with 
			// a different buffer. Should this logic ever be needed by
			// a user handler, it should be implemented by using a Queue
			// in the UnmarshallingContext.
			// The buffer has to be set back from the outside!  See below
			IReadBuffer contextBuffer = context.Buffer(buffer);
			object array = base.Read(context);
			// The context buffer has to be set back.
			context.Buffer(contextBuffer);
			return array;
		}

		public static void Defragment(IDefragmentContext context, ArrayHandler handler)
		{
			int sourceAddress = context.SourceBuffer().ReadInt();
			int length = context.SourceBuffer().ReadInt();
			if (sourceAddress == 0 && length == 0)
			{
				context.TargetBuffer().WriteInt(0);
				context.TargetBuffer().WriteInt(0);
				return;
			}
			Slot slot = context.AllocateMappedTargetSlot(sourceAddress, length);
			ByteArrayBuffer sourceBuffer = null;
			try
			{
				sourceBuffer = context.SourceBufferByAddress(sourceAddress, length);
			}
			catch (IOException exc)
			{
				throw new Db4oIOException(exc);
			}
			DefragmentContextImpl payloadContext = new DefragmentContextImpl(sourceBuffer, (DefragmentContextImpl
				)context);
			handler.DefragmentSlot(payloadContext);
			payloadContext.WriteToTarget(slot.Address());
			context.TargetBuffer().WriteInt(slot.Address());
			context.TargetBuffer().WriteInt(length);
		}

		public override void Defragment(IDefragmentContext context)
		{
			Defragment(context, this);
		}
	}
}
