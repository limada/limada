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
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>root of the reflection implementation API.</summary>
	/// <remarks>
	/// root of the reflection implementation API.
	/// <br/><br/>The open reflection interface is supplied to allow to implement
	/// custom reflection functionality.<br/><br/>
	/// Use
	/// <see cref="IConfiguration.ReflectWith">
	/// Db4o.Configure().ReflectWith(IReflect reflector)
	/// </see>
	/// to register the use of your implementation before opening database
	/// files.
	/// </remarks>
	public interface IReflector : IDeepClone
	{
		void Configuration(IReflectorConfiguration config);

		/// <summary>
		/// returns an ReflectArray object.
		/// </summary>
		/// <remarks>
		/// returns an ReflectArray object.
		/// </remarks>
		IReflectArray Array();

		/// <summary>returns an ReflectClass for a Class</summary>
		IReflectClass ForClass(Type clazz);

		/// <summary>
		/// returns an ReflectClass class reflector for a class name or null
		/// if no such class is found
		/// </summary>
		IReflectClass ForName(string className);

		/// <summary>returns an ReflectClass for an object or null if the passed object is null.
		/// 	</summary>
		/// <remarks>returns an ReflectClass for an object or null if the passed object is null.
		/// 	</remarks>
		IReflectClass ForObject(object obj);

		bool IsCollection(IReflectClass clazz);

		void SetParent(IReflector reflector);
	}
}
