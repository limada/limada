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
	public class FieldMarshaller2 : FieldMarshaller1
	{
		private const int AspectTypeTagLength = 1;

		public override int MarshalledLength(ObjectContainerBase stream, ClassAspect aspect
			)
		{
			return base.MarshalledLength(stream, aspect) + AspectTypeTagLength;
		}

		protected override RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader)
		{
			return base.ReadSpec(AspectType.ForByte(reader.ReadByte()), stream, reader);
		}

		public override void Write(Transaction trans, ClassMetadata clazz, ClassAspect aspect
			, ByteArrayBuffer writer)
		{
			writer.WriteByte(aspect.AspectType()._id);
			base.Write(trans, clazz, aspect, writer);
		}

		public override void Defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO
			 sio, DefragmentContextImpl context)
		{
			context.ReadByte();
			base.Defrag(classMetadata, aspect, sio, context);
		}
	}
}
