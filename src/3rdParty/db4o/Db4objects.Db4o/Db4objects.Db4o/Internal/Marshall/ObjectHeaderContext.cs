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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class ObjectHeaderContext : AbstractReadContext, IMarshallingInfo, IHandlerVersionContext
	{
		protected ObjectHeader _objectHeader;

		private int _declaredAspectCount;

		public ObjectHeaderContext(Transaction transaction, IReadBuffer buffer, ObjectHeader
			 objectHeader) : base(transaction, buffer)
		{
			_objectHeader = objectHeader;
		}

		public ObjectHeaderAttributes HeaderAttributes()
		{
			return _objectHeader._headerAttributes;
		}

		public bool IsNull(int fieldIndex)
		{
			return HeaderAttributes().IsNull(fieldIndex);
		}

		public override int HandlerVersion()
		{
			return _objectHeader.HandlerVersion();
		}

		public virtual void BeginSlot()
		{
		}

		// do nothing
		public virtual ContextState SaveState()
		{
			return new ContextState(Offset());
		}

		public virtual void RestoreState(ContextState state)
		{
			Seek(state._offset);
		}

		public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _objectHeader.ClassMetadata();
		}

		public virtual int DeclaredAspectCount()
		{
			return _declaredAspectCount;
		}

		public virtual void DeclaredAspectCount(int count)
		{
			_declaredAspectCount = count;
		}
	}
}
