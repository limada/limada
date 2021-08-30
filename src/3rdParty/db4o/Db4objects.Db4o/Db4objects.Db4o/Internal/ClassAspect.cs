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
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class ClassAspect
	{
		protected int _handle;

		private int _disabledFromAspectCountVersion = AspectVersionContextImpl.AlwaysEnabled
			.DeclaredAspectCount();

		// used for identification when sending in C/S mode 
		public abstract Db4objects.Db4o.Internal.Marshall.AspectType AspectType();

		public abstract string GetName();

		public abstract void CascadeActivation(IActivationContext context);

		public abstract int LinkLength();

		public void IncrementOffset(IReadBuffer buffer)
		{
			buffer.Seek(buffer.Offset() + LinkLength());
		}

		public abstract void DefragAspect(IDefragmentContext context);

		public abstract void Marshall(MarshallingContext context, object child);

		public abstract void CollectIDs(CollectIdContext context);

		public virtual void SetHandle(int handle)
		{
			_handle = handle;
		}

		public abstract void Activate(UnmarshallingContext context);

		public abstract void Delete(DeleteContextImpl context, bool isUpdate);

		public abstract bool CanBeDisabled();

		protected virtual bool CheckEnabled(IAspectVersionContext context)
		{
			if (!IsEnabledOn(context))
			{
				IncrementOffset((IReadBuffer)context);
				return false;
			}
			return true;
		}

		public virtual void DisableFromAspectCountVersion(int aspectCount)
		{
			if (!CanBeDisabled())
			{
				return;
			}
			if (aspectCount < _disabledFromAspectCountVersion)
			{
				_disabledFromAspectCountVersion = aspectCount;
			}
		}

		public bool IsEnabledOn(IAspectVersionContext context)
		{
			return _disabledFromAspectCountVersion > context.DeclaredAspectCount();
		}

		public abstract void Deactivate(IActivationContext context);
	}
}
