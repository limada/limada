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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	public class ObjectReferenceContext : ObjectHeaderContext, IObjectIdContext
	{
		protected readonly Db4objects.Db4o.Internal.ObjectReference _reference;

		public ObjectReferenceContext(Transaction transaction, IReadBuffer buffer, ObjectHeader
			 objectHeader, Db4objects.Db4o.Internal.ObjectReference reference) : base(transaction
			, buffer, objectHeader)
		{
			_reference = reference;
		}

		public virtual int ObjectId()
		{
			return _reference.GetID();
		}

		public override Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = _reference.ClassMetadata();
			if (classMetadata == null)
			{
				throw new InvalidOperationException();
			}
			return classMetadata;
		}

		public virtual Db4objects.Db4o.Internal.ObjectReference ObjectReference()
		{
			return _reference;
		}

		protected virtual Db4objects.Db4o.Internal.ByteArrayBuffer ByteArrayBuffer()
		{
			return (Db4objects.Db4o.Internal.ByteArrayBuffer)Buffer();
		}
	}
}
