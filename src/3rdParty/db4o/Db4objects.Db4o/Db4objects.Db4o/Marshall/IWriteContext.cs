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
	/// during marshaling
	/// and provides methods to marshal objects.
	/// </summary>
	public interface IWriteContext : IContext, IWriteBuffer
	{
		/// <summary>
		/// makes sure the object is stored and writes the ID of
		/// the object to the context.
		/// </summary>
		/// <remarks>
		/// makes sure the object is stored and writes the ID of
		/// the object to the context.
		/// Use this method for first class objects only (objects that
		/// have an identity in the database). If the object can potentially
		/// be a primitive type, do not use this method but use
		/// a matching
		/// <see cref="IWriteBuffer">IWriteBuffer</see>
		/// method instead.
		/// </remarks>
		/// <param name="obj">the object to write.</param>
		void WriteObject(object obj);

		/// <summary>
		/// writes sub-objects, in cases where the
		/// <see cref="Db4objects.Db4o.Typehandlers.ITypeHandler4">Db4objects.Db4o.Typehandlers.ITypeHandler4
		/// 	</see>
		/// is known.
		/// </summary>
		/// <param name="handler">typehandler to be used to write the object.</param>
		/// <param name="obj">the object to write</param>
		void WriteObject(ITypeHandler4 handler, object obj);

		/// <summary>
		/// reserves a buffer with a specific length at the current
		/// position, to be written in a later step.
		/// </summary>
		/// <remarks>
		/// reserves a buffer with a specific length at the current
		/// position, to be written in a later step.
		/// </remarks>
		/// <param name="length">the length to be reserved.</param>
		/// <returns>the ReservedBuffer</returns>
		IReservedBuffer Reserve(int length);
	}
}
