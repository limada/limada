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
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>TypeHandler for objects with members.</summary>
	/// <remarks>TypeHandler for objects with members.</remarks>
	public interface ICascadingTypeHandler : ITypeHandler4
	{
		/// <summary>
		/// will be called during activation if the handled
		/// object is already active
		/// </summary>
		/// <param name="context"></param>
		void CascadeActivation(IActivationContext context);

		/// <summary>
		/// will be called during querying to ask for the handler
		/// to be used to collect children of the handled object
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		ITypeHandler4 ReadCandidateHandler(QueryingReadContext context);

		/// <summary>
		/// will be called during querying to ask for IDs of member
		/// objects of the handled object.
		/// </summary>
		/// <remarks>
		/// will be called during querying to ask for IDs of member
		/// objects of the handled object.
		/// </remarks>
		/// <param name="context"></param>
		void CollectIDs(QueryingReadContext context);
	}
}
