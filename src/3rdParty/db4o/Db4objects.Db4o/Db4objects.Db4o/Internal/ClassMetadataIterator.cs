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

namespace Db4objects.Db4o.Internal
{
	/// <exclude>TODO: remove this class or make it private to ClassMetadataRepository</exclude>
	public class ClassMetadataIterator : MappingIterator
	{
		private readonly ClassMetadataRepository i_collection;

		internal ClassMetadataIterator(ClassMetadataRepository a_collection, IEnumerator 
			iterator) : base(iterator)
		{
			i_collection = a_collection;
		}

		public virtual ClassMetadata CurrentClass()
		{
			return (ClassMetadata)Current;
		}

		protected override object Map(object current)
		{
			return i_collection.ReadClassMetadata((ClassMetadata)current, null);
		}
	}
}
