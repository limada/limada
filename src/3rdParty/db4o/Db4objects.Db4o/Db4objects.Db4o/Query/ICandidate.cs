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

namespace Db4objects.Db4o.Query
{
	/// <summary>
	/// candidate for
	/// <see cref="IEvaluation">IEvaluation</see>
	/// callbacks.
	/// <br /><br />
	/// During
	/// <see cref="IQuery.Execute()">query execution</see>
	/// all registered
	/// <see cref="IEvaluation">IEvaluation</see>
	/// callback
	/// handlers are called with
	/// <see cref="ICandidate">ICandidate</see>
	/// proxies that represent the persistent objects that
	/// meet all other
	/// <see cref="IQuery">IQuery</see>
	/// criteria.
	/// <br /><br />
	/// A
	/// <see cref="ICandidate">ICandidate</see>
	/// provides access to the persistent object it
	/// represents and allows to specify, whether it is to be included in the
	/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
	/// resultset.
	/// </summary>
	public interface ICandidate
	{
		/// <summary>
		/// returns the persistent object that is represented by this query
		/// <see cref="ICandidate">ICandidate</see>
		/// .
		/// </summary>
		/// <returns>Object the persistent object.</returns>
		object GetObject();

		/// <summary>
		/// specify whether the Candidate is to be included in the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// resultset.
		/// <br /><br />
		/// This method may be called multiple times. The last call prevails.
		/// </summary>
		/// <param name="flag">inclusion.</param>
		void Include(bool flag);

		/// <summary>
		/// returns the
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// the Candidate object is stored in.
		/// </summary>
		/// <returns>
		/// the
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// </returns>
		IObjectContainer ObjectContainer();
	}
}
