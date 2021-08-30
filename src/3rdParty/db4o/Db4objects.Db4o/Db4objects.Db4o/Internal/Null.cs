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
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class Null : IIndexable4, IPreparedComparison
	{
		public static readonly Db4objects.Db4o.Internal.Null Instance = new Db4objects.Db4o.Internal.Null
			();

		private Null()
		{
		}

		public virtual int CompareTo(object a_obj)
		{
			if (a_obj == null)
			{
				return 0;
			}
			return -1;
		}

		public virtual int LinkLength()
		{
			return 0;
		}

		public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer a_reader)
		{
			return null;
		}

		public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer a_writer, object
			 a_object)
		{
		}

		// do nothing
		public virtual void DefragIndexEntry(DefragmentContextImpl context)
		{
		}

		// do nothing
		public virtual IPreparedComparison PrepareComparison(IContext context, object obj_
			)
		{
			return new _IPreparedComparison_43();
		}

		private sealed class _IPreparedComparison_43 : IPreparedComparison
		{
			public _IPreparedComparison_43()
			{
			}

			public int CompareTo(object obj)
			{
				if (obj == null)
				{
					return 0;
				}
				if (obj is Db4objects.Db4o.Internal.Null)
				{
					return 0;
				}
				return -1;
			}
		}
	}
}
