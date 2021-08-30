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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal
{
    /// <exclude></exclude>
    public partial class ObjectContainerSession
    {
        void System.IDisposable.Dispose()
        {
            Close();
        }

        public IObjectSet Query(Db4objects.Db4o.Query.Predicate match, System.Collections.IComparer comparer)
        {
            return _server.Query(_transaction, match, new ComparerAdaptor(comparer));
        }

        public System.Collections.Generic.IList<Extent> Query<Extent>(Predicate<Extent> match)
        {
            return _server.Query(_transaction, match);
        }

        public System.Collections.Generic.IList<Extent> Query<Extent>(Predicate<Extent> match, System.Collections.Generic.IComparer<Extent> comparer)
        {
            return _server.Query(_transaction, match, comparer);
        }

        public System.Collections.Generic.IList<Extent> Query<Extent>(Predicate<Extent> match, System.Comparison<Extent> comparison)
        {
            return _server.Query(_transaction, match, comparison);
        }

        public System.Collections.Generic.IList<ElementType> Query<ElementType>(System.Type extent)
        {
            return _server.Query<ElementType>(_transaction, extent, null);
        }

        public System.Collections.Generic.IList<ElementType> Query<ElementType>(System.Type extent, System.Collections.Generic.IComparer<ElementType> comparer)
        {
            return _server.Query(_transaction, extent, comparer);
        }

        public System.Collections.Generic.IList<Extent> Query<Extent>()
        {
            return Query<Extent>(typeof(Extent));
        }

        public System.Collections.Generic.IList<Extent> Query<Extent>(System.Collections.Generic.IComparer<Extent> comparer)
        {
            return Query<Extent>(typeof(Extent), comparer);
        }

		public void WithEnvironment(Action4 action)
		{
			_server.WithEnvironment(new RunnableAction(action));
		}
    }
}
