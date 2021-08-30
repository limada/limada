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
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Internal.Metadata
{
	/// <exclude></exclude>
	public abstract class MarshallingInfoTraverseAspectCommand : ITraverseAspectCommand
	{
		private bool _cancelled = false;

		protected readonly IMarshallingInfo _marshallingInfo;

		public MarshallingInfoTraverseAspectCommand(IMarshallingInfo marshallingInfo)
		{
			_marshallingInfo = marshallingInfo;
		}

		public int DeclaredAspectCount(ClassMetadata classMetadata)
		{
			int aspectCount = InternalDeclaredAspectCount(classMetadata);
			_marshallingInfo.DeclaredAspectCount(aspectCount);
			return aspectCount;
		}

		protected virtual int InternalDeclaredAspectCount(ClassMetadata classMetadata)
		{
			return classMetadata.ReadAspectCount(_marshallingInfo.Buffer());
		}

		public virtual bool Cancelled()
		{
			return _cancelled;
		}

		protected virtual void Cancel()
		{
			_cancelled = true;
		}

		public virtual bool Accept(ClassAspect aspect)
		{
			return true;
		}

		public virtual void ProcessAspectOnMissingClass(ClassAspect aspect, int currentSlot
			)
		{
			if (_marshallingInfo.IsNull(currentSlot))
			{
				return;
			}
			aspect.IncrementOffset(_marshallingInfo.Buffer());
		}

		public virtual void ProcessAspect(ClassAspect aspect, int currentSlot)
		{
			if (Accept(aspect))
			{
				ProcessAspect(aspect, currentSlot, _marshallingInfo.IsNull(currentSlot));
			}
			_marshallingInfo.BeginSlot();
		}

		protected abstract void ProcessAspect(ClassAspect aspect, int currentSlot, bool isNull
			);
	}
}
