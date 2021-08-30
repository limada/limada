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

namespace Db4objects.Db4o.Internal.Classindex
{
	/// <exclude></exclude>
	public interface IClassIndexStrategy
	{
		void Initialize(ObjectContainerBase stream);

		void Read(ObjectContainerBase stream, int indexID);

		int Write(Transaction transaction);

		void Add(Transaction transaction, int id);

		void Remove(Transaction transaction, int id);

		int EntryCount(Transaction transaction);

		int OwnLength();

		void Purge();

		/// <summary>Traverses all index entries (java.lang.Integer references).</summary>
		/// <remarks>Traverses all index entries (java.lang.Integer references).</remarks>
		void TraverseAll(Transaction transaction, IVisitor4 command);

		void DontDelete(Transaction transaction, int id);

		IEnumerator AllSlotIDs(Transaction trans);

		// FIXME: Why is this never called?
		void DefragReference(ClassMetadata classMetadata, DefragmentContextImpl context, 
			int classIndexID);

		int Id();

		// FIXME: Why is this never called?
		void DefragIndex(DefragmentContextImpl context);
	}
}
