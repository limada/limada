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
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>
	/// Predicate to be able to select if a specific TypeHandler is
	/// applicable for a specific Type.
	/// </summary>
	/// <remarks>
	/// Predicate to be able to select if a specific TypeHandler is
	/// applicable for a specific Type.
	/// </remarks>
	public interface ITypeHandlerPredicate
	{
		/// <summary>
		/// return true if a TypeHandler is to be used for a specific
		/// Type
		/// </summary>
		/// <param name="classReflector">
		/// the Type passed by db4o that is to
		/// be tested by this predicate.
		/// </param>
		/// <returns>
		/// true if the TypeHandler is to be used for a specific
		/// Type.
		/// </returns>
		bool Match(IReflectClass classReflector);
	}
}
