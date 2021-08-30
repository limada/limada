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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Implements one step in the defragmenting process.</summary>
	/// <remarks>Implements one step in the defragmenting process.</remarks>
	/// <exclude></exclude>
	internal interface IPassCommand
	{
		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessObjectSlot(DefragmentServicesImpl context, ClassMetadata classMetadata
			, int id);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessClass(DefragmentServicesImpl context, ClassMetadata classMetadata, int
			 id, int classIndexID);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessClassCollection(DefragmentServicesImpl context);

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void ProcessBTree(DefragmentServicesImpl context, BTree btree);

		void Flush(DefragmentServicesImpl context);
	}
}
