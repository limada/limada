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
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Marshall
{
	/// <summary>
	/// this interface is passed to internal class
	/// <see cref="Db4objects.Db4o.Typehandlers.ITypeHandler4">Db4objects.Db4o.Typehandlers.ITypeHandler4
	/// 	</see>
	/// when instantiating objects.
	/// </summary>
	public interface IReadContext : IContext, IReadBuffer
	{
		/// <summary>
		/// Interprets the current position in the context as
		/// an ID and returns the object with this ID.
		/// </summary>
		/// <remarks>
		/// Interprets the current position in the context as
		/// an ID and returns the object with this ID.
		/// </remarks>
		/// <returns>the object</returns>
		object ReadObject();

		/// <summary>
		/// reads sub-objects, in cases where the
		/// <see cref="Db4objects.Db4o.Typehandlers.ITypeHandler4">Db4objects.Db4o.Typehandlers.ITypeHandler4
		/// 	</see>
		/// is known.
		/// </summary>
		object ReadObject(ITypeHandler4 handler);
	}
}
