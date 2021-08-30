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
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class HardObjectReference
	{
		public static readonly Db4objects.Db4o.Internal.HardObjectReference Invalid = new 
			Db4objects.Db4o.Internal.HardObjectReference(null, null);

		public readonly ObjectReference _reference;

		public readonly object _object;

		public HardObjectReference(ObjectReference @ref, object obj)
		{
			_reference = @ref;
			_object = obj;
		}

		public static Db4objects.Db4o.Internal.HardObjectReference PeekPersisted(Transaction
			 trans, int id, int depth)
		{
			object obj = trans.Container().PeekPersisted(trans, id, ActivationDepthProvider(trans
				).ActivationDepth(depth, ActivationMode.Peek), true);
			if (obj == null)
			{
				return null;
			}
			ObjectReference @ref = trans.ReferenceForId(id);
			return new Db4objects.Db4o.Internal.HardObjectReference(@ref, obj);
		}

		private static IActivationDepthProvider ActivationDepthProvider(Transaction trans
			)
		{
			return trans.Container().ActivationDepthProvider();
		}
	}
}
