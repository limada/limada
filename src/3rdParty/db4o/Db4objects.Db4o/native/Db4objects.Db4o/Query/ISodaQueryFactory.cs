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
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
    public interface ISodaQueryFactory
    {
        /// <summary>
        /// creates a new SODA
        /// <see cref="Db4objects.Db4o.Query.IQuery">Query</see>
        /// .
        /// <br /><br />
        /// Linq queries are the recommended main db4o query interface.
        /// <br /><br />
        /// Use
        /// <see cref="Db4objects.Db4o.IObjectContainer.QueryByExample">QueryByExample(Object template)</see>
        /// for simple Query-By-Example.<br /><br />
        /// </summary>
        /// <returns>a new IQuery object</returns>
        IQuery Query();
    }
}