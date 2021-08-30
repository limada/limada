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
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Query
{
	/// <summary>handle to a node in a S.O.D.A.</summary>
	/// <remarks>
	/// handle to a node in a S.O.D.A. query graph.
	/// <br/><br/>
	/// A node in the query graph can represent multiple
	/// classes, one class or an attribute of a class.<br/><br/>The graph
	/// is automatically extended with attributes of added constraints
	/// (see
	/// <see cref="Constrain(object)">Constrain(object)</see>
	/// ) and upon calls to
	/// <see cref="Descend(string)">Descend(string)</see>
	/// that request nodes that do not yet exist.
	/// <br/><br/>
	/// References to joined nodes in the query graph can be obtained
	/// by "walking" along the nodes of the graph with the method
	/// <see cref="Descend(string)">Descend(string)</see>
	/// .
	/// <br/><br/>
	/// <see cref="Execute()">Execute()</see>
	/// evaluates the entire graph against all persistent objects.
	/// <br/><br/>
	/// <see cref="Execute()">Execute()</see>
	/// can be called from any
	/// <see cref="IQuery">IQuery</see>
	/// node
	/// of the graph. It will return an
	/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
	/// filled with
	/// objects of the class/classes that the node, it was called from,
	/// represents.<br/><br/>
	/// <b>Note:<br/>
	/// <see cref="Predicate">Native queries</see>
	/// are the recommended main query
	/// interface of db4o.</b>
	/// </remarks>
	public interface IQuery
	{
		/// <summary>adds a constraint to this node.</summary>
		/// <remarks>
		/// adds a constraint to this node.
		/// <br/><br/>
		/// If the constraint contains attributes that are not yet
		/// present in the query graph, the query graph is extended
		/// accordingly.
		/// <br/><br/>
		/// Special behaviour for:
		/// <ul>
		/// <li> class
		/// <see cref="System.Type{T}">System.Type&lt;T&gt;</see>
		/// : confine the result to objects of one
		/// class or to objects implementing an interface.</li>
		/// <li> interface
		/// <see cref="IEvaluation">IEvaluation</see>
		/// : run
		/// evaluation callbacks against all candidates.</li>
		/// </ul>
		/// </remarks>
		/// <param name="constraint">the constraint to be added to this Query.</param>
		/// <returns>
		/// 
		/// <see cref="IConstraint">IConstraint</see>
		/// a new
		/// <see cref="IConstraint">IConstraint</see>
		/// for this
		/// query node or <code>null</code> for objects implementing the
		/// <see cref="IEvaluation">IEvaluation</see>
		/// interface.
		/// </returns>
		IConstraint Constrain(object constraint);

		/// <summary>
		/// returns a
		/// <see cref="IConstraints">IConstraints</see>
		/// object that holds an array of all constraints on this node.
		/// </summary>
		/// <returns>
		/// 
		/// <see cref="IConstraints">IConstraints</see>
		/// on this query node.
		/// </returns>
		IConstraints Constraints();

		/// <summary>returns a reference to a descendant node in the query graph.</summary>
		/// <remarks>
		/// returns a reference to a descendant node in the query graph.
		/// <br/><br/>If the node does not exist, it will be created.
		/// <br/><br/>
		/// All classes represented in the query node are tested, whether
		/// they contain a field with the specified field name. The
		/// descendant Query node will be created from all possible candidate
		/// classes.
		/// </remarks>
		/// <param name="fieldName">path to the descendant.</param>
		/// <returns>
		/// descendant
		/// <see cref="IQuery">IQuery</see>
		/// node
		/// </returns>
		IQuery Descend(string fieldName);

		/// <summary>
		/// executes the
		/// <see cref="IQuery">IQuery</see>
		/// .
		/// </summary>
		/// <returns>
		/// 
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// - the result of the
		/// <see cref="IQuery">IQuery</see>
		/// .
		/// </returns>
		IObjectSet Execute();

		/// <summary>
		/// adds an ascending ordering criteria to this node of
		/// the query graph.
		/// </summary>
		/// <remarks>
		/// adds an ascending ordering criteria to this node of
		/// the query graph.
		/// <p>
		/// If multiple ordering criteria are applied, the chronological
		/// order of method calls is relevant: criteria created by 'earlier' calls are
		/// considered more significant, i.e. 'later' criteria only have an effect
		/// for elements that are considered equal by all 'earlier' criteria.
		/// </p>
		/// <p>
		/// As an example, consider a type with two int fields, and an instance set
		/// {(a:1,b:3),(a:2,b:2),(a:1,b:2),(a:2,b:3)}. The call sequence [orderAscending(a),
		/// orderDescending(b)] will result in [(<b>a:1</b>,b:3),(<b>a:1</b>,b:2),(<b>a:2</b>,b:3),(<b>a:2</b>,b:2)].
		/// </p>
		/// </remarks>
		/// <returns>
		/// this
		/// <see cref="IQuery">IQuery</see>
		/// object to allow the chaining of method calls.
		/// </returns>
		IQuery OrderAscending();

		/// <summary>
		/// adds a descending order criteria to this node of
		/// the query graph.
		/// </summary>
		/// <remarks>
		/// adds a descending order criteria to this node of
		/// the query graph.
		/// <br/><br/>
		/// For semantics of multiple calls setting ordering criteria, see
		/// <see cref="OrderAscending()">OrderAscending()</see>
		/// .
		/// </remarks>
		/// <returns>
		/// this
		/// <see cref="IQuery">IQuery</see>
		/// object to allow the chaining of method calls.
		/// </returns>
		IQuery OrderDescending();

		/// <summary>Sort the resulting ObjectSet by the given comparator.</summary>
		/// <remarks>Sort the resulting ObjectSet by the given comparator.</remarks>
		/// <param name="comparator">The comparator to apply.</param>
		/// <returns>
		/// this
		/// <see cref="IQuery">IQuery</see>
		/// object to allow the chaining of method calls.
		/// </returns>
		IQuery SortBy(IQueryComparator comparator);
	}
}
