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
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayVersionHelper3 : ArrayVersionHelper5
	{
		public override int ClassIDFromInfo(ObjectContainerBase container, ArrayInfo info
			)
		{
			ClassMetadata classMetadata = container.ProduceClassMetadata(info.ReflectClass());
			if (classMetadata == null)
			{
				// TODO: This one is a terrible low-frequency blunder !!!
				// If YapClass-ID == 99999 then we will get IGNORE back.
				// Discovered on adding the primitives
				return Const4.IgnoreId;
			}
			return classMetadata.GetID();
		}

		public override int ClassIdToMarshalledClassId(int classID, bool primitive)
		{
			if (primitive)
			{
				classID -= Const4.Primitive;
			}
			return -classID;
		}

		public override IReflectClass ClassReflector(IReflector reflector, ClassMetadata 
			classMetadata, bool isPrimitive)
		{
			IReflectClass primitiveClaxx = Handlers4.PrimitiveClassReflector(classMetadata, reflector
				);
			if (primitiveClaxx != null)
			{
				return primitiveClaxx;
			}
			return base.ClassReflector(reflector, classMetadata, isPrimitive);
		}

		public override bool HasNullBitmap(ArrayInfo info)
		{
			return false;
		}

		public override bool IsPrimitive(IReflector reflector, IReflectClass claxx, ClassMetadata
			 classMetadata)
		{
			return Handlers4.PrimitiveClassReflector(classMetadata, reflector) != null;
			return claxx.IsPrimitive();
		}

		public override IReflectClass ReflectClassFromElementsEntry(ObjectContainerBase container
			, ArrayInfo info, int classID)
		{
			if (classID == Const4.IgnoreId)
			{
				// TODO: Here is a low-frequency mistake, extremely unlikely.
				// If classID == 99999 by accident then we will get ignore.
				return null;
			}
			info.Primitive(false);
			if (UseJavaHandling())
			{
				if (classID < Const4.Primitive)
				{
					info.Primitive(true);
					classID -= Const4.Primitive;
				}
			}
			classID = -classID;
			ClassMetadata classMetadata = container.ClassMetadataForID(classID);
			if (classMetadata != null)
			{
				return ClassReflector(container.Reflector(), classMetadata, info.Primitive());
			}
			return null;
		}

		public sealed override bool UseJavaHandling()
		{
			return !Deploy.csharp;
		}

		public override void WriteTypeInfo(IWriteContext context, ArrayInfo info)
		{
		}

		// do nothing, the byte for additional type information was added after format 3
		public override void ReadTypeInfo(Transaction trans, IReadBuffer buffer, ArrayInfo
			 info, int classID)
		{
		}
		// do nothing, the byte for additional type information was added after format 3
	}
}
