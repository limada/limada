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
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public class TransactionalReferenceSystem : TransactionalReferenceSystemBase, IReferenceSystem
	{
		public override void Commit()
		{
			TraverseNewReferences(new _IVisitor4_16(this));
			CreateNewReferences();
		}

		private sealed class _IVisitor4_16 : IVisitor4
		{
			public _IVisitor4_16(TransactionalReferenceSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				ObjectReference oref = (ObjectReference)obj;
				if (oref.GetObject() != null)
				{
					this._enclosing._committedReferences.AddExistingReference(oref);
				}
			}

			private readonly TransactionalReferenceSystem _enclosing;
		}

		public override void AddExistingReference(ObjectReference @ref)
		{
			_committedReferences.AddExistingReference(@ref);
		}

		public override void AddNewReference(ObjectReference @ref)
		{
			_newReferences.AddNewReference(@ref);
		}

		public override void RemoveReference(ObjectReference @ref)
		{
			_newReferences.RemoveReference(@ref);
			_committedReferences.RemoveReference(@ref);
		}

		public override void Rollback()
		{
			CreateNewReferences();
		}

		public virtual void Discarded()
		{
		}
		// do nothing;
	}
}
