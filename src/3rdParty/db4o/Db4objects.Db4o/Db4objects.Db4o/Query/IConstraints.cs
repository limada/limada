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
	/// <summary>
	/// set of
	/// <see cref="IConstraint">IConstraint</see>
	/// objects.
	/// <br /><br />This extension of the
	/// <see cref="IConstraint">IConstraint</see>
	/// interface allows
	/// setting the evaluation mode of all contained
	/// <see cref="IConstraint">IConstraint</see>
	/// objects with single calls.
	/// <br /><br />
	/// See also
	/// <see cref="IQuery.Constraints()">IQuery.Constraints()</see>
	/// .
	/// </summary>
	public interface IConstraints : IConstraint
	{
		/// <summary>
		/// returns an array of the contained
		/// <see cref="IConstraint">IConstraint</see>
		/// objects.
		/// </summary>
		/// <returns>
		/// an array of the contained
		/// <see cref="IConstraint">IConstraint</see>
		/// objects.
		/// </returns>
		IConstraint[] ToArray();
	}
}
