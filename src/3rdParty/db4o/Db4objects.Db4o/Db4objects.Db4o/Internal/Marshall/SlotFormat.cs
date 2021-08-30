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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public abstract class SlotFormat
	{
		private static readonly Hashtable4 _versions = new Hashtable4();

		private static readonly Db4objects.Db4o.Internal.Marshall.SlotFormat CurrentSlotFormat
			 = new SlotFormatCurrent();

		static SlotFormat()
		{
			new SlotFormat0();
			new SlotFormat2();
		}

		protected SlotFormat()
		{
			_versions.Put(HandlerVersion(), this);
		}

		public static Db4objects.Db4o.Internal.Marshall.SlotFormat ForHandlerVersion(int 
			handlerVersion)
		{
			if (handlerVersion == HandlerRegistry.HandlerVersion)
			{
				return CurrentSlotFormat;
			}
			if (handlerVersion < 0 || handlerVersion > CurrentSlotFormat.HandlerVersion())
			{
				throw new ArgumentException();
			}
			Db4objects.Db4o.Internal.Marshall.SlotFormat slotFormat = (Db4objects.Db4o.Internal.Marshall.SlotFormat
				)_versions.Get(handlerVersion);
			if (slotFormat != null)
			{
				return slotFormat;
			}
			return ForHandlerVersion(handlerVersion + 1);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Db4o.Internal.Marshall.SlotFormat))
			{
				return false;
			}
			return HandlerVersion() == ((Db4objects.Db4o.Internal.Marshall.SlotFormat)obj).HandlerVersion
				();
		}

		public override int GetHashCode()
		{
			return HandlerVersion();
		}

		protected abstract int HandlerVersion();

		public abstract bool IsIndirectedWithinSlot(ITypeHandler4 handler);

		public static Db4objects.Db4o.Internal.Marshall.SlotFormat Current()
		{
			return CurrentSlotFormat;
		}

		public virtual object DoWithSlotIndirection(IReadBuffer buffer, ITypeHandler4 typeHandler
			, IClosure4 closure)
		{
			if (!IsIndirectedWithinSlot(typeHandler))
			{
				return closure.Run();
			}
			return DoWithSlotIndirection(buffer, closure);
		}

		public virtual object DoWithSlotIndirection(IReadBuffer buffer, IClosure4 closure
			)
		{
			int payLoadOffset = buffer.ReadInt();
			buffer.ReadInt();
			// length, not used
			int savedOffset = buffer.Offset();
			object res = null;
			if (payLoadOffset != 0)
			{
				buffer.Seek(payLoadOffset);
				res = closure.Run();
			}
			buffer.Seek(savedOffset);
			return res;
		}

		public virtual void WriteObjectClassID(ByteArrayBuffer buffer, int id)
		{
			buffer.WriteInt(-id);
		}

		public virtual void SkipMarshallerInfo(ByteArrayBuffer reader)
		{
			reader.IncrementOffset(1);
		}

		public virtual ObjectHeaderAttributes ReadHeaderAttributes(ByteArrayBuffer reader
			)
		{
			return new ObjectHeaderAttributes(reader);
		}
	}
}
