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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public class ReferenceSystemRegistry
	{
		private readonly Collection4 _referenceSystems = new Collection4();

		public virtual void RemoveId(int id)
		{
			RemoveReference(new _IReferenceSource_17(id));
		}

		private sealed class _IReferenceSource_17 : ReferenceSystemRegistry.IReferenceSource
		{
			public _IReferenceSource_17(int id)
			{
				this.id = id;
			}

			public ObjectReference ReferenceFrom(IReferenceSystem referenceSystem)
			{
				return referenceSystem.ReferenceForId(id);
			}

			private readonly int id;
		}

		public virtual void RemoveObject(object obj)
		{
			RemoveReference(new _IReferenceSource_25(obj));
		}

		private sealed class _IReferenceSource_25 : ReferenceSystemRegistry.IReferenceSource
		{
			public _IReferenceSource_25(object obj)
			{
				this.obj = obj;
			}

			public ObjectReference ReferenceFrom(IReferenceSystem referenceSystem)
			{
				return referenceSystem.ReferenceForObject(obj);
			}

			private readonly object obj;
		}

		public virtual void RemoveReference(ObjectReference reference)
		{
			RemoveReference(new _IReferenceSource_33(reference));
		}

		private sealed class _IReferenceSource_33 : ReferenceSystemRegistry.IReferenceSource
		{
			public _IReferenceSource_33(ObjectReference reference)
			{
				this.reference = reference;
			}

			public ObjectReference ReferenceFrom(IReferenceSystem referenceSystem)
			{
				return reference;
			}

			private readonly ObjectReference reference;
		}

		private void RemoveReference(ReferenceSystemRegistry.IReferenceSource referenceSource
			)
		{
			IEnumerator i = _referenceSystems.GetEnumerator();
			while (i.MoveNext())
			{
				IReferenceSystem referenceSystem = (IReferenceSystem)i.Current;
				ObjectReference reference = referenceSource.ReferenceFrom(referenceSystem);
				if (reference != null)
				{
					referenceSystem.RemoveReference(reference);
				}
			}
		}

		public virtual void AddReferenceSystem(IReferenceSystem referenceSystem)
		{
			_referenceSystems.Add(referenceSystem);
		}

		public virtual bool RemoveReferenceSystem(IReferenceSystem referenceSystem)
		{
			bool res = _referenceSystems.Remove(referenceSystem);
			referenceSystem.Discarded();
			return res;
		}

		private interface IReferenceSource
		{
			ObjectReference ReferenceFrom(IReferenceSystem referenceSystem);
		}
	}
}
