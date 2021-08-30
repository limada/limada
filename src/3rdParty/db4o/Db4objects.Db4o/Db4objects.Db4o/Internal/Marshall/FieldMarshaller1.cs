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
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class FieldMarshaller1 : FieldMarshaller0
	{
		private bool HasBTreeIndex(FieldMetadata field)
		{
			return !field.IsVirtual();
		}

		public override void Write(Transaction trans, ClassMetadata clazz, ClassAspect aspect
			, ByteArrayBuffer writer)
		{
			base.Write(trans, clazz, aspect, writer);
			if (!(aspect is FieldMetadata))
			{
				return;
			}
			FieldMetadata field = (FieldMetadata)aspect;
			if (!HasBTreeIndex(field))
			{
				return;
			}
			writer.WriteIDOf(trans, field.GetIndex(trans));
		}

		protected override RawFieldSpec ReadSpec(AspectType aspectType, ObjectContainerBase
			 stream, ByteArrayBuffer reader)
		{
			RawFieldSpec spec = base.ReadSpec(aspectType, stream, reader);
			if (spec == null)
			{
				return null;
			}
			if (spec.IsVirtual())
			{
				return spec;
			}
			int indexID = reader.ReadInt();
			spec.IndexID(indexID);
			return spec;
		}

		protected override FieldMetadata FromSpec(RawFieldSpec spec, ObjectContainerBase 
			stream, ClassMetadata containingClass)
		{
			FieldMetadata actualField = base.FromSpec(spec, stream, containingClass);
			if (spec == null)
			{
				return null;
			}
			if (spec.IndexID() != 0)
			{
				actualField.InitIndex(stream.SystemTransaction(), spec.IndexID());
			}
			return actualField;
		}

		public override int MarshalledLength(ObjectContainerBase stream, ClassAspect aspect
			)
		{
			int len = base.MarshalledLength(stream, aspect);
			if (!(aspect is FieldMetadata))
			{
				return len;
			}
			FieldMetadata field = (FieldMetadata)aspect;
			if (!HasBTreeIndex(field))
			{
				return len;
			}
			return len + Const4.IdLength;
		}

		public override void Defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO
			 sio, DefragmentContextImpl context)
		{
			base.Defrag(classMetadata, aspect, sio, context);
			if (!(aspect is FieldMetadata))
			{
				return;
			}
			FieldMetadata field = (FieldMetadata)aspect;
			if (field.IsVirtual())
			{
				return;
			}
			if (field.HasIndex())
			{
				BTree index = field.GetIndex(context.SystemTrans());
				int targetIndexID = context.CopyID();
				if (targetIndexID != 0)
				{
					index.DefragBTree(context.Services());
				}
			}
			else
			{
				context.WriteInt(0);
			}
		}
	}
}
