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
using Db4objects.Db4o;
using Db4objects.Db4o.Qlin;

namespace Db4objects.Db4o.Qlin
{
	/// <summary>a node in a QLin ("Coolin") query.</summary>
	/// <remarks>
	/// a node in a QLin ("Coolin") query.
	/// QLin is a new experimental query interface.
	/// We would really like to have LINQ for Java instead.
	/// </remarks>
	/// <since>8.0</since>
	public interface IQLin
	{
		/// <summary>adds a where node to this QLin query.</summary>
		/// <remarks>adds a where node to this QLin query.</remarks>
		/// <param name="expression">can be any of the following:</param>
		IQLin Where(object expression);

		/// <summary>
		/// executes the QLin query and returns the result
		/// as an
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// .
		/// Note that ObjectSet extends List and Iterable
		/// on the platforms that support these interfaces.
		/// You may want to use these interfaces instead of
		/// working directly against an ObjectSet.
		/// </summary>
		IObjectSet Select();

		// FIXME: The return value should not be as closely bound to db4o.
		// Collection is mutable, it's not nice.
		// Discuss !!!
		IQLin Equal(object obj);

		IQLin StartsWith(string @string);

		IQLin Limit(int size);

		IQLin Smaller(object obj);

		IQLin Greater(object obj);

		/// <summary>orders the query by the expression.</summary>
		/// <remarks>
		/// orders the query by the expression.
		/// Use the
		/// <see cref="QLinSupport.Ascending()">QLinSupport.Ascending()</see>
		/// and
		/// <see cref="QLinSupport.Descending()">QLinSupport.Descending()</see>
		/// helper methods to set the direction.
		/// </remarks>
		IQLin OrderBy(object expression, QLinOrderByDirection direction);

		object SingleOrDefault(object defaultValue);

		object Single();
	}
}
