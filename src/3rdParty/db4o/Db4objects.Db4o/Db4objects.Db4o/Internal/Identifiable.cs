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
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class Identifiable
	{
		protected int _id;

		protected int _state = 2;

		// DIRTY and ACTIVE
		public bool BeginProcessing()
		{
			if (BitIsTrue(Const4.Processing))
			{
				return false;
			}
			BitTrue(Const4.Processing);
			return true;
		}

		internal void BitFalse(int bitPos)
		{
			_state &= ~(1 << bitPos);
		}

		internal bool BitIsFalse(int bitPos)
		{
			return (_state | (1 << bitPos)) != _state;
		}

		internal bool BitIsTrue(int bitPos)
		{
			return (_state | (1 << bitPos)) == _state;
		}

		internal void BitTrue(int bitPos)
		{
			_state |= (1 << bitPos);
		}

		public virtual void EndProcessing()
		{
			BitFalse(Const4.Processing);
		}

		public virtual int GetID()
		{
			return _id;
		}

		public bool IsActive()
		{
			return BitIsTrue(Const4.Active);
		}

		public virtual bool IsDirty()
		{
			return BitIsTrue(Const4.Active) && (!BitIsTrue(Const4.Clean));
		}

		public bool IsNew()
		{
			return GetID() == 0;
		}

		public virtual void SetID(int id)
		{
			if (DTrace.enabled)
			{
				DTrace.PersistentbaseSetId.Log(id);
			}
			_id = id;
		}

		public void SetStateClean()
		{
			BitTrue(Const4.Active);
			BitTrue(Const4.Clean);
		}

		public void SetStateDeactivated()
		{
			BitFalse(Const4.Active);
		}

		public virtual void SetStateDirty()
		{
			BitTrue(Const4.Active);
			BitFalse(Const4.Clean);
		}

		public override int GetHashCode()
		{
			if (IsNew())
			{
				throw new InvalidOperationException();
			}
			return GetID();
		}
	}
}
