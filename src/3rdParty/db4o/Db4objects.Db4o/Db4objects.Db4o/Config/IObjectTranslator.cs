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
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <summary>translator interface to translate objects on storage and activation.</summary>
	/// <remarks>
	/// translator interface to translate objects on storage and activation.
	/// <br/><br/>
	/// By writing classes that implement this interface, it is possible to
	/// define how application classes are to be converted to be stored more efficiently.
	/// <br/><br/>
	/// Before starting a db4o session, translator classes need to be registered. An example:<br/>
	/// <code>
	/// IObjectClass oc = config.ObjectClass("Namespace.ClassName");<br/>
	/// oc.Translate(new FooTranslator());
	/// </code><br/><br/>
	/// </remarks>
	public interface IObjectTranslator
	{
		/// <summary>db4o calls this method during storage and query evaluation.</summary>
		/// <remarks>db4o calls this method during storage and query evaluation.</remarks>
		/// <param name="container">the ObjectContainer used</param>
		/// <param name="applicationObject">the Object to be translated</param>
		/// <returns>
		/// return the object to store.<br />It needs to be of the class
		/// <see cref="StoredClass()">StoredClass()</see>
		/// .
		/// </returns>
		object OnStore(IObjectContainer container, object applicationObject);

		/// <summary>db4o calls this method during activation.</summary>
		/// <remarks>db4o calls this method during activation.</remarks>
		/// <param name="container">the ObjectContainer used</param>
		/// <param name="applicationObject">the object to set the members on</param>
		/// <param name="storedObject">the object that was stored</param>
		void OnActivate(IObjectContainer container, object applicationObject, object storedObject
			);

		/// <summary>return the Class you are converting to.</summary>
		/// <remarks>return the Class you are converting to.</remarks>
		/// <returns>
		/// the Class of the object you are returning with the method
		/// <see cref="OnStore(Db4objects.Db4o.IObjectContainer, object)">OnStore(Db4objects.Db4o.IObjectContainer, object)
		/// 	</see>
		/// </returns>
		Type StoredClass();
	}
}
