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
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <summary>Interface for comparison support in queries.</summary>
	/// <remarks>Interface for comparison support in queries.</remarks>
	public interface IComparable4
	{
		/// <summary>
		/// creates a prepared comparison to compare multiple objects
		/// against one single object.
		/// </summary>
		/// <remarks>
		/// creates a prepared comparison to compare multiple objects
		/// against one single object.
		/// </remarks>
		/// <param name="context">the context of the comparison</param>
		/// <param name="obj">
		/// the object that is to be compared
		/// against multiple other objects
		/// </param>
		/// <returns>the prepared comparison</returns>
		IPreparedComparison PrepareComparison(IContext context, object obj);
	}
}
