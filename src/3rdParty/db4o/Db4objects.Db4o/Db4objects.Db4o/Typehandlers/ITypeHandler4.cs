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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>
	/// handles reading, writing, deleting, defragmenting and
	/// comparisons for types of objects.<br /><br />
	/// Custom Typehandlers can be implemented to alter the default
	/// behaviour of storing all non-transient fields of an object.<br /><br />
	/// </summary>
	/// <seealso>
	/// 
	/// <see cref="Db4objects.Db4o.Config.IConfiguration.RegisterTypeHandler(ITypeHandlerPredicate, ITypeHandler4)
	/// 	">Db4objects.Db4o.Config.IConfiguration.RegisterTypeHandler(ITypeHandlerPredicate, ITypeHandler4)
	/// 	</see>
	/// 
	/// </seealso>
	public interface ITypeHandler4
	{
		/// <summary>gets called when an object gets deleted.</summary>
		/// <remarks>gets called when an object gets deleted.</remarks>
		/// <param name="context"></param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">Db4objects.Db4o.Ext.Db4oIOException
		/// 	</exception>
		void Delete(IDeleteContext context);

		/// <summary>gets called when an object gets defragmented.</summary>
		/// <remarks>gets called when an object gets defragmented.</remarks>
		/// <param name="context"></param>
		void Defragment(IDefragmentContext context);

		/// <summary>gets called when an object is to be written to the database.</summary>
		/// <remarks>gets called when an object is to be written to the database.</remarks>
		/// <param name="context"></param>
		/// <param name="obj">the object</param>
		void Write(IWriteContext context, object obj);
	}
}
