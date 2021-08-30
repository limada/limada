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
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o
{
	/// <summary>the interface to a db4o database, stand-alone or client/server.</summary>
	/// <remarks>
	/// the interface to a db4o database, stand-alone or client/server.
	/// <br /><br />The IObjectContainer interface provides methods
	/// to store, query and delete objects and to commit and rollback
	/// transactions.<br /><br />
	/// An IObjectContainer can either represent a stand-alone database
	/// or a connection to a
	/// <see cref="Db4objects.Db4o.Db4o.OpenServer">db4o server</see>
	/// .
	/// <br /><br />An IObjectContainer also represents a transaction. All work
	/// with db4o always is transactional. Both
	/// <see cref="Db4objects.Db4o.IObjectContainer.Commit">Db4objects.Db4o.IObjectContainer.Commit</see>
	/// and
	/// <see cref="Db4objects.Db4o.IObjectContainer.Rollback">Db4objects.Db4o.IObjectContainer.Rollback</see>
	/// start new transactions immediately. For working
	/// against the same database with multiple transactions, open a db4o server
	/// with
	/// <see cref="Db4objects.Db4o.Db4o.OpenServer">Db4objects.Db4o.Db4o.OpenServer</see>
	/// and
	/// <see cref="Db4objects.Db4o.ObjectServer.OpenClient">connect locally</see>
	/// or
	/// <see cref="Db4objects.Db4o.Db4o.OpenClient">over TCP</see>
	/// .
	/// </remarks>
	/// <seealso cref="Db4objects.Db4o.Ext.IExtObjectContainer">IExtObjectContainer for extended functionality.
	/// 	</seealso>
	public interface IObjectContainer : System.IDisposable, ISodaQueryFactory
	{
		/// <summary>activates all members on a stored object to the specified depth.</summary>
		/// <remarks>
		/// activates all members on a stored object to the specified depth.
		/// <br /><br />
		/// See
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">"Why activation"</see>
		/// for an explanation why activation is necessary.<br /><br />
		/// The activate method activates a graph of persistent objects in memory.
		/// Only deactivated objects in the graph will be touched: their
		/// fields will be loaded from the database.
		/// The activate methods starts from a
		/// root object and traverses all member objects to the depth specified by the
		/// depth parameter. The depth parameter is the distance in "field hops"
		/// (object.field.field) away from the root object. The nodes at 'depth' level
		/// away from the root (for a depth of 3: object.member.member) will be instantiated
		/// but deactivated, their fields will be null.
		/// The activation depth of individual classes can be overruled
		/// with the methods
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth">MaximumActivationDepth()
		/// 	</see>
		/// and
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.MinimumActivationDepth">MinimumActivationDepth()
		/// 	</see>
		/// in the
		/// <see cref="Db4objects.Db4o.Config.IObjectClass">ObjectClass interface</see>
		/// .<br /><br />
		/// A successful call to activate triggers Activating and Activated callback methods,
		/// which can be used for cascaded activation.<br /><br />
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">Why activation?</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <param name="obj">the object to be activated.</param>
		/// <param name="depth">
		/// the member
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">depth</see>
		/// to which activate is to cascade.
		/// </param>
		void Activate(object obj, int depth);

		/// <summary>closes this IObjectContainer.</summary>
		/// <remarks>
		/// closes this IObjectContainer.
		/// <br /><br />A call to Close() automatically performs a
		/// <see cref="Db4objects.Db4o.IObjectContainer.Commit">Commit()</see>
		/// .
		/// <br /><br />Note that every session opened with Db4oFactory.OpenFile() requires one
		/// Close()call, even if the same filename was used multiple times.<br /><br />
		/// Use <code>while(!Close()){}</code> to kill all sessions using this container.<br /><br />
		/// </remarks>
		/// <returns>
		/// success - true denotes that the last used instance of this container
		/// and the database file were closed.
		/// </returns>
		bool Close();

		/// <summary>commits the running transaction.</summary>
		/// <remarks>
		/// commits the running transaction.
		/// <br /><br />Transactions are back-to-back. A call to commit will starts
		/// a new transaction immedidately.
		/// </remarks>
		void Commit();

		/// <summary>deactivates a stored object by setting all members to <code>NULL</code>.
		/// 	</summary>
		/// <remarks>
		/// deactivates a stored object by setting all members to <code>NULL</code>.
		/// <br />Primitive types will be set to their default values.
		/// Calls to this method save memory.
		/// The method has no effect, if the passed object is not stored in the
		/// <code>IObjectContainer</code>.<br /><br />
		/// <code>Deactivate()</code> triggers Deactivating and Deactivated callbacks.
		/// <br /><br />
		/// Be aware that calling this method with a depth parameter greater than
		/// 1 sets members on member objects to null. This may have side effects
		/// in other places of the application.<br /><br />
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">Why activation?</seealso>
		/// <param name="obj">the object to be deactivated.</param>
		/// <param name="depth">
		/// the member
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">depth</see>
		/// 
		/// to which deactivate is to cascade.
		/// </param>
		void Deactivate(object obj, int depth);

		/// <summary>deletes a stored object permanently.</summary>
		/// <remarks>
		/// deletes a stored object permanently.
		/// <br /><br />Note that this method has to be called <b>for every single object
		/// individually</b>. Delete does not recurse to object members. Simple
		/// and array member types are destroyed.
		/// <br /><br />Object members of the passed object remain untouched, unless
		/// cascaded deletes are
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnDelete">configured for the class</see>
		/// or for
		/// <see cref="Db4objects.Db4o.Config.IObjectField.CascadeOnDelete">one of the member fields</see>
		/// .
		/// <br /><br />The method has no effect, if
		/// the passed object is not stored in the <code>IObjectContainer</code>.
		/// <br /><br />A subsequent call to
		/// <code>Store()</code> with the same object newly stores the object
		/// to the <code>IObjectContainer</code>.<br /><br />
		/// <code>Delete()</code> triggers Deleting and Deleted callbacks,
		/// which can be also used for cascaded deletes.<br /><br />
		/// </remarks>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnDelete">Db4objects.Db4o.Config.IObjectClass.CascadeOnDelete
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IObjectField.CascadeOnDelete">Db4objects.Db4o.Config.IObjectField.CascadeOnDelete
		/// 	</seealso>
		/// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
		/// <param name="obj">
		/// the object to be deleted from the
		/// <code>IObjectContainer</code>.<br />
		/// </param>
		void Delete(object obj);

		/// <summary>returns an IObjectContainer with extended functionality.</summary>
		/// <remarks>
		/// returns an IObjectContainer with extended functionality.
		/// <br /><br />Every IObjectContainer that db4o provides can be casted to
		/// an IExtObjectContainer. This method is supplied for your convenience
		/// to work without a cast.
		/// <br /><br />The IObjectContainer functionality is split to two interfaces
		/// to allow newcomers to focus on the essential methods.<br /><br />
		/// </remarks>
		/// <returns>this, casted to IExtObjectContainer</returns>
		Db4objects.Db4o.Ext.IExtObjectContainer Ext();

        /// <summary>Query-By-Example interface to retrieve objects.</summary>
        /// <remarks>
        /// Query-By-Example interface to retrieve objects.
        /// <br /><br /><code>QueryByExample()</code> creates an
        /// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
        /// containing
        /// all objects in the <code>IObjectContainer</code> that match the passed
        /// template object.<br /><br />
        /// Calling <code>QueryByExample(null)</code> returns all objects stored in the
        /// <code>IObjectContainer</code>.<br /><br /><br />
        /// <b>Query Evaluation</b>
        /// <br />All non-null members of the template object are compared against
        /// all stored objects of the same class.
        /// Primitive type members are ignored if they are 0 or false respectively.
        /// <br /><br />Arrays and all supported <code>Collection</code> classes are
        /// evaluated for containment. Differences in <code>Length/Count/Size()</code> are
        /// ignored.
        /// <br /><br />Consult the documentation of the IConfiguration package to
        /// configure class-specific behaviour.<br /><br /><br />
        /// <b>Returned Objects</b><br />
        /// The objects returned in the
        /// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
        /// are instantiated
        /// and activated to the preconfigured depth of 5. The
        /// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">activation depth</see>
        /// may be configured
        /// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">globally</see>
        /// or
        /// <see cref="Db4objects.Db4o.Config.IObjectClass">individually for classes</see>
        /// .
        /// <br /><br />
        /// db4o keeps track of all instantiatied objects. Queries will return
        /// references to these objects instead of instantiating them a second time.
        /// <br /><br />
        /// Objects newly activated by <code>QueryByExample()</code> can respond to the Activating callback
        /// method.
        /// <br /><br />
        /// </remarks>
        /// <param name="template">object to be used as an example to find all matching objects.<br /><br />
        /// 	</param>
        /// <returns>
        /// 
        /// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
        /// containing all found objects.<br /><br />
        /// </returns>
        /// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth">Why activation?</seealso>
        /// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
        Db4objects.Db4o.IObjectSet QueryByExample(object template);


		/// <summary>
		/// creates a new SODA
		/// <see cref="Db4objects.Db4o.Query.IQuery">Query</see>
		/// .
		/// <br /><br />
		/// Linq queries are the recommended main db4o query interface.
		/// <br /><br />
		/// Use
		/// <see cref="Db4objects.Db4o.IObjectContainer.QueryByExample">QueryByExample(Object template)</see>
		/// for simple Query-By-Example.<br /><br />
		/// </summary>
		/// <returns>a new IQuery object</returns>
		Db4objects.Db4o.Query.IQuery Query();

		/// <summary>queries for all instances of a class.</summary>
		/// <remarks>queries for all instances of a class.</remarks>
		/// <param name="clazz">the class to query for.</param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(System.Type clazz);

		/// <summary>Native Query Interface.</summary>
		/// <remarks>
		/// Native Query Interface.
		/// <br /><br />Native Queries allow typesafe, compile-time checked and refactorable
		/// querying, following object-oriented principles. Native Queries expressions
		/// are written as if one or more lines of code would be run against all
		/// instances of a class. A Native Query expression should return true to mark
		/// specific instances as part of the result set.
		/// db4o will  attempt to optimize native query expressions and execute them
		/// against indexes and without instantiating actual objects, where this is
		/// possible.<br /><br />
		/// Example:<br /><br />
		/// <code>
		/// <br />
		/// IList &lt;Cat&gt; cats = db.Query &lt;Cat&gt; (delegate(Cat cat) {<br />
		/// &#160;&#160;&#160;return cat.Name == "Occam";<br />
		/// });<br />
		/// <br />
		/// Summing up the above:<br />
		/// In order to run a Native Query, you can use the delegate notation 
	        /// with a delegate method taking the extend type as a parameter and 
		/// returning bool. True is returned for the objects that are to be included in the result.<br />
		/// <br /><br />
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate);

		/// <summary>Native Query Interface.</summary>
		/// <remarks>
		/// Native Query Interface. Queries as with
		/// <see cref="M:Db4objects.Db4o.IObjectContainer.Query(Db4objects.Db4o.Query.Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)</see>
		/// ,
		/// but will sort the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// according to the given
		/// <see cref="Db4objects.Db4o.Query.IQueryComparator">Db4objects.Db4o.Query.IQueryComparator</see>
		/// .
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <param name="comparator">
		/// the
		/// <see cref="Db4objects.Db4o.Query.IQueryComparator">Db4objects.Db4o.Query.IQueryComparator</see>
		/// specifiying the sort order of the result
		/// </param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate, Db4objects.Db4o.Query.IQueryComparator
			 comparator);

		/// <summary>Native Query Interface.</summary>
		/// <remarks>
		/// Native Query Interface. Queries as with
		/// <see cref="M:Db4objects.Db4o.IObjectContainer.Query(Db4objects.Db4o.Query.Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)</see>
		/// ,
		/// but will sort the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// according to the given
		/// <see cref="System.Collections.IComparer">System.Collections.IComparer</see>
		/// .
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <param name="comparator">
		/// the
		/// <see cref="System.Collections.IComparer">System.Collections.IComparer</see>
		/// specifiying the sort order of the result
		/// </param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		Db4objects.Db4o.IObjectSet Query(Db4objects.Db4o.Query.Predicate predicate, System.Collections.IComparer comparer);

		/// <summary>rolls back the running transaction.</summary>
		/// <remarks>
		/// rolls back the running transaction.
		/// <br /><br />Transactions are back-to-back. A call to rollback will starts
		/// a new transaction immedidately.
		/// <br /><br />rollback will not restore modified objects in memory. They
		/// can be refreshed from the database by calling
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer.Refresh">Db4objects.Db4o.Ext.IExtObjectContainer.Refresh
		/// 	</see>
		/// .
		/// </remarks>
		void Rollback();

        /// <summary>newly stores objects or updates stored objects.</summary>
        /// <remarks>
        /// newly stores objects or updates stored objects.
        /// <br /><br />An object not yet stored in the <code>IObjectContainer</code> will be
        /// stored when it is passed to <code>Store()</code>. An object already stored
        /// in the <code>IObjectContainer</code> will be updated.
        /// <br /><br /><b>Updates</b><br />
        /// - will affect all simple type object members.<br />
        /// - links to object members that are already stored will be updated.<br />
        /// - new object members will be newly stored. The algorithm traverses down
        /// new members, as long as further new members are found.<br />
        /// - object members that are already stored will <b>not</b> be updated
        /// themselves.<br />Every object member needs to be updated individually with a
        /// call to <code>Store()</code> unless a deep
        /// <see cref="Db4objects.Db4o.Config.IConfiguration.UpdateDepth">global</see>
        /// or
        /// <see cref="Db4objects.Db4o.Config.IObjectClass.UpdateDepth">class-specific</see>
        /// update depth was configured or cascaded updates were
        /// <see cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnUpdate">defined in the class</see>
        /// or in
        /// <see cref="Db4objects.Db4o.Config.IObjectField.CascadeOnUpdate">one of the member fields</see>
        /// .
       	/// Depending if the passed object is newly stored or updated, Creating/Created or
       	/// Updaing/Updated callback method is triggered.
       	/// Callbacks
       	/// might also be used for cascaded updates.<br /><br />
        /// </remarks>
        /// <param name="obj">the object to be stored or updated.</param>
        /// <seealso cref="Db4objects.Db4o.Ext.IExtObjectContainer.Store">IExtObjectContainer#Store(object, depth)
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IConfiguration.UpdateDepth">Db4objects.Db4o.Config.IConfiguration.UpdateDepth
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectClass.UpdateDepth">Db4objects.Db4o.Config.IObjectClass.UpdateDepth
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectClass.CascadeOnUpdate">Db4objects.Db4o.Config.IObjectClass.CascadeOnUpdate
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Config.IObjectField.CascadeOnUpdate">Db4objects.Db4o.Config.IObjectField.CascadeOnUpdate
        /// 	</seealso>
        /// <seealso cref="Db4objects.Db4o.Ext.IObjectCallbacks">Using callbacks</seealso>
        void Store(object obj);

        /// <summary>.NET 2.0 Native Query interface.</summary>
        /// <remarks>
        /// Native Query Interface.
        /// <br /><br />Native Queries allow typesafe, compile-time checked and refactorable
        /// querying, following object-oriented principles. Native Queries expressions
        /// are written as if one or more lines of code would be run against all
        /// instances of a class. A Native Query expression should return true to mark
        /// specific instances as part of the result set.
        /// db4o will  attempt to optimize native query expressions and execute them
        /// against indexes and without instantiating actual objects, where this is
        /// possible.<br /><br />
	/// Example:<br /><br />
       	/// <code>
       	/// <br />
       	/// IList &lt;Cat&gt; cats = db.Query &lt;Cat&gt; (delegate(Cat cat) {<br />
       	/// &#160;&#160;&#160;return cat.Name == "Occam";<br />
       	/// });<br />
       	/// <br />
       	/// Summing up the above:<br />
       	/// In order to run a Native Query, you can use the delegate notation 
               /// with a delegate method taking the extend type as a parameter and 
       	/// returning bool. True is returned for the objects that are to be included in the result.<br />
       	/// <br /><br />
        /// </remarks>
        /// <param name="match">
        /// use an anonymous delegate that takes a single paramter and returns
        /// a bool value, see the syntax example above
        /// </param>
        /// <returns>
        /// the
        /// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
        /// returned by the query.
        /// </returns>
        System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match);
        
		/// <summary>Native Query Interface.</summary>
		/// <remarks>
		/// Native Query Interface. Queries as with
		/// <see cref="M:Db4objects.Db4o.IObjectContainer.Query(Db4objects.Db4o.Query.Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)</see>
		/// ,
		/// but will sort the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// according to the given
		/// <see cref="System.Collections.Generic.IComparer">System.Collections.Generic.IComparer</see>
		/// .
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <param name="comparator">
		/// the
		/// <see cref="System.Collections.Generic.IComparer">System.Collections.Generic.IComparer</see>
		/// specifiying the sort order of the result
		/// </param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match, System.Collections.Generic.IComparer<Extent> comparer);

		/// <summary>Native Query Interface.</summary>
		/// <remarks>
		/// Native Query Interface. Queries as with
		/// <see cref="M:Db4objects.Db4o.IObjectContainer.Query(Db4objects.Db4o.Query.Predicate)">Db4objects.Db4o.IObjectContainer.Query(Predicate)</see>
		/// ,
		/// but will sort the resulting
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// according to the given
		/// <see cref="System.Comparison">System.Comparison</see>
		/// .
		/// </remarks>
		/// <param name="predicate">
		/// the
		/// <see cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</see>
		/// containing the native query expression.
		/// </param>
		/// <param name="comparator">
		/// the
		/// <see cref="System.Comparison">System.Comparison</see>
		/// specifiying the sort order of the result
		/// </param>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// returned by the query.
		/// </returns>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Predicate<Extent> match, System.Comparison<Extent> comparison);

		/// <summary>
		/// queries for all instances of the type extent, returning
		/// a IList of ElementType which must be assignable from
		/// extent.
		/// </summary>
		System.Collections.Generic.IList<ElementType> Query<ElementType>(System.Type extent);
		
		/// <summary>
		/// queries for all instances of the type extent.
		/// </summary>
		System.Collections.Generic.IList<Extent> Query<Extent>();

		/// <summary>
		/// queries for all instances of the type extent sorting with the specified comparer.
		/// </summary>
		System.Collections.Generic.IList<Extent> Query<Extent>(System.Collections.Generic.IComparer<Extent> comparer);
	}
}
