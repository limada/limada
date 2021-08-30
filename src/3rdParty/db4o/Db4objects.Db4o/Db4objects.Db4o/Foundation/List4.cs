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
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>simplest possible linked list</summary>
	/// <exclude></exclude>
	public sealed class List4 : IUnversioned
	{
		/// <summary>next element in list</summary>
		public Db4objects.Db4o.Foundation.List4 _next;

		/// <summary>carried object</summary>
		public object _element;

		/// <summary>db4o constructor to be able to store objects of this class</summary>
		public List4()
		{
		}

		public List4(object element)
		{
			// TODO: encapsulate field access
			_element = element;
		}

		public List4(Db4objects.Db4o.Foundation.List4 next, object element)
		{
			_next = next;
			_element = element;
		}

		internal bool Holds(object obj)
		{
			if (obj == null)
			{
				return _element == null;
			}
			return ((object)obj).Equals(_element);
		}

		public static int Size(Db4objects.Db4o.Foundation.List4 list)
		{
			int counter = 0;
			Db4objects.Db4o.Foundation.List4 nextList = list;
			while (nextList != null)
			{
				counter++;
				nextList = ((Db4objects.Db4o.Foundation.List4)nextList._next);
			}
			return counter;
		}
	}
}
