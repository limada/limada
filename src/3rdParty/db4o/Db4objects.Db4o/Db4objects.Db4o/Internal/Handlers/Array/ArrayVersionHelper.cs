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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayVersionHelper
	{
		public virtual int ClassIDFromInfo(ObjectContainerBase container, ArrayInfo info)
		{
			ClassMetadata classMetadata = container.ProduceClassMetadata(info.ReflectClass());
			if (classMetadata == null)
			{
				return 0;
			}
			return classMetadata.GetID();
		}

		public virtual int ClassIdToMarshalledClassId(int classID, bool primitive)
		{
			return classID;
		}

		public virtual IReflectClass ClassReflector(IReflector reflector, ClassMetadata classMetadata
			, bool isPrimitive)
		{
			return isPrimitive ? Handlers4.PrimitiveClassReflector(classMetadata, reflector) : 
				classMetadata.ClassReflector();
		}

		public virtual bool UseJavaHandling()
		{
			return true;
		}

		public virtual bool HasNullBitmap(ArrayInfo info)
		{
			if (info.Nullable())
			{
				return true;
			}
			return !info.Primitive();
		}

		public virtual bool IsPreVersion0Format(int elementCount)
		{
			return false;
		}

		public virtual bool IsPrimitive(IReflector reflector, IReflectClass claxx, ClassMetadata
			 classMetadata)
		{
			return claxx.IsPrimitive();
		}

		public virtual IReflectClass ReflectClassFromElementsEntry(ObjectContainerBase container
			, ArrayInfo info, int classID)
		{
			if (classID == 0)
			{
				return null;
			}
			ClassMetadata classMetadata = container.ClassMetadataForID(classID);
			if (classMetadata == null)
			{
				return null;
			}
			return ClassReflector(container.Reflector(), classMetadata, info.Primitive());
		}

		public virtual void WriteTypeInfo(IWriteContext context, ArrayInfo info)
		{
			BitMap4 typeInfoBitmap = new BitMap4(2);
			typeInfoBitmap.Set(0, info.Primitive());
			typeInfoBitmap.Set(1, info.Nullable());
			context.WriteByte(typeInfoBitmap.GetByte(0));
		}

		public virtual void ReadTypeInfo(Transaction trans, IReadBuffer buffer, ArrayInfo
			 info, int classID)
		{
			BitMap4 typeInfoBitmap = new BitMap4(buffer.ReadByte());
			info.Primitive(typeInfoBitmap.IsTrue(0));
			info.Nullable(typeInfoBitmap.IsTrue(1));
		}
	}
}
