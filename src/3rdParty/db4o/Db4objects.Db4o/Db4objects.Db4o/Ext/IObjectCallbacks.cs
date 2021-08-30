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
using Db4objects.Db4o;

namespace Db4objects.Db4o.Ext
{
	/// <summary>callback methods.</summary>
	/// <remarks>
	/// callback methods.
	/// <br /><br />
	/// This interface only serves as a list of all available callback methods.
	/// Every method is called individually, independantly of implementing this interface.<br /><br />
	/// <b>Using callbacks</b><br />
	/// Simply implement one or more of the listed methods in your application classes to
	/// do tasks before activation, deactivation, delete, new or update, to cancel the
	/// action about to be performed and to respond to the performed task.
	/// <br /><br />Callback methods are typically used for:
	/// <br />- cascaded delete
	/// <br />- cascaded update
	/// <br />- cascaded activation
	/// <br />- restoring transient members on instantiation
	/// <br /><br />Callback methods follow regular calling conventions. Methods in superclasses
	/// need to be called explicitely.
	/// <br /><br />All method calls are implemented to occur only once, upon one event.
	/// </remarks>
	public interface IObjectCallbacks
	{
		/// <summary>called before an Object is activated.</summary>
		/// <remarks>called before an Object is activated.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		/// <returns>false to prevent activation.</returns>
		bool ObjectCanActivate(IObjectContainer container);

		/// <summary>called before an Object is deactivated.</summary>
		/// <remarks>called before an Object is deactivated.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		/// <returns>false to prevent deactivation.</returns>
		bool ObjectCanDeactivate(IObjectContainer container);

		/// <summary>called before an Object is deleted.</summary>
		/// <remarks>
		/// called before an Object is deleted.
		/// <br /><br />In a client/server setup this callback method will be executed on
		/// the server.
		/// </remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		/// <returns>false to prevent the object from being deleted.</returns>
		bool ObjectCanDelete(IObjectContainer container);

		/// <summary>called before an Object is stored the first time.</summary>
		/// <remarks>called before an Object is stored the first time.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> is about to be stored to.
		/// 	</param>
		/// <returns>false to prevent the object from being stored.</returns>
		bool ObjectCanNew(IObjectContainer container);

		/// <summary>called before a persisted Object is updated.</summary>
		/// <remarks>called before a persisted Object is updated.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		/// <returns>false to prevent the object from being updated.</returns>
		bool ObjectCanUpdate(IObjectContainer container);

		/// <summary>called upon activation of an object.</summary>
		/// <remarks>called upon activation of an object.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		void ObjectOnActivate(IObjectContainer container);

		/// <summary>called upon deactivation of an object.</summary>
		/// <remarks>called upon deactivation of an object.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		void ObjectOnDeactivate(IObjectContainer container);

		/// <summary>called after an object was deleted.</summary>
		/// <remarks>
		/// called after an object was deleted.
		/// <br /><br />In a client/server setup this callback method will be executed on
		/// the server.
		/// </remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object was stored in.
		/// 	</param>
		void ObjectOnDelete(IObjectContainer container);

		/// <summary>called after a new object was stored.</summary>
		/// <remarks>called after a new object was stored.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored to.
		/// 	</param>
		void ObjectOnNew(IObjectContainer container);

		/// <summary>called after an object was updated.</summary>
		/// <remarks>called after an object was updated.</remarks>
		/// <param name="container">the <code>ObjectContainer</code> the object is stored in.
		/// 	</param>
		void ObjectOnUpdate(IObjectContainer container);
	}
}
