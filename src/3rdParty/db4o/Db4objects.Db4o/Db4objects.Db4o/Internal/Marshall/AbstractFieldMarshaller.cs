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
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public abstract class AbstractFieldMarshaller : IFieldMarshaller
	{
		protected abstract RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader);

		public virtual RawFieldSpec ReadSpec(ObjectContainerBase stream, ByteArrayBuffer 
			reader)
		{
			return ReadSpec(AspectType.Field, stream, reader);
		}

		public abstract void Defrag(ClassMetadata arg1, ClassAspect arg2, LatinStringIO arg3
			, DefragmentContextImpl arg4);

		public abstract int MarshalledLength(ObjectContainerBase arg1, ClassAspect arg2);

		public abstract FieldMetadata Read(ObjectContainerBase arg1, ClassMetadata arg2, 
			ByteArrayBuffer arg3);

		public abstract void Write(Transaction arg1, ClassMetadata arg2, ClassAspect arg3
			, ByteArrayBuffer arg4);
	}
}
