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

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Reflection Field representation.</summary>
	/// <remarks>
	/// Reflection Field representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectField
	{
		object Get(object onObject);

		string GetName();

		/// <summary>
		/// The ReflectClass returned by this method should have been
		/// provided by the parent reflector.
		/// </summary>
		/// <remarks>
		/// The ReflectClass returned by this method should have been
		/// provided by the parent reflector.
		/// </remarks>
		/// <returns>the ReflectClass representing the field type as provided by the parent reflector
		/// 	</returns>
		IReflectClass GetFieldType();

		bool IsPublic();

		bool IsStatic();

		bool IsTransient();

		void Set(object onObject, object value);

		/// <summary>
		/// The ReflectClass returned by this method should have been
		/// provided by the parent reflector.
		/// </summary>
		/// <remarks>
		/// The ReflectClass returned by this method should have been
		/// provided by the parent reflector.
		/// </remarks>
		/// <returns>the ReflectClass representing the index type as provided by the parent reflector
		/// 	</returns>
		IReflectClass IndexType();

		object IndexEntry(object orig);
	}
}
