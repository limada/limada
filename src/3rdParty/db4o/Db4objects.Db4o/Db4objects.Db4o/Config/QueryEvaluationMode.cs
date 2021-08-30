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

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// This class provides static constants for the query evaluation
	/// modes that db4o supports.
	/// </summary>
	/// <remarks>
	/// This class provides static constants for the query evaluation
	/// modes that db4o supports.
	/// <br /><br /><b>For detailed documentation please see
	/// <see cref="IQueryConfiguration.EvaluationMode(QueryEvaluationMode)">IQueryConfiguration.EvaluationMode(QueryEvaluationMode)
	/// 	</see>
	/// </b>
	/// </remarks>
	public class QueryEvaluationMode
	{
		private readonly string _id;

		private QueryEvaluationMode(string id)
		{
			_id = id;
		}

		/// <summary>Constant for immediate query evaluation.</summary>
		/// <remarks>
		/// Constant for immediate query evaluation. The query is executed fully
		/// when
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// is called.
		/// <br /><br /><b>For detailed documentation please see
		/// <see cref="IQueryConfiguration.EvaluationMode(QueryEvaluationMode)">IQueryConfiguration.EvaluationMode(QueryEvaluationMode)
		/// 	</see>
		/// </b>
		/// </remarks>
		public static readonly Db4objects.Db4o.Config.QueryEvaluationMode Immediate = new 
			Db4objects.Db4o.Config.QueryEvaluationMode("IMMEDIATE");

		/// <summary>Constant for snapshot query evaluation.</summary>
		/// <remarks>
		/// Constant for snapshot query evaluation. When
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// is called,
		/// the query processor chooses the best indexes, does all index processing
		/// and creates a snapshot of the index at this point in time. Non-indexed
		/// constraints are evaluated lazily when the application iterates through
		/// the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// resultset of the query.
		/// <br /><br /><b>For detailed documentation please see
		/// <see cref="IQueryConfiguration.EvaluationMode(QueryEvaluationMode)">IQueryConfiguration.EvaluationMode(QueryEvaluationMode)
		/// 	</see>
		/// </b>
		/// </remarks>
		public static readonly Db4objects.Db4o.Config.QueryEvaluationMode Snapshot = new 
			Db4objects.Db4o.Config.QueryEvaluationMode("SNAPSHOT");

		/// <summary>Constant for lazy query evaluation.</summary>
		/// <remarks>
		/// Constant for lazy query evaluation. When
		/// <see cref="Db4objects.Db4o.Query.IQuery.Execute()">Db4objects.Db4o.Query.IQuery.Execute()
		/// 	</see>
		/// is called, the
		/// query processor only chooses the best index and creates an iterator on
		/// this index. Indexes and constraints are evaluated lazily when the
		/// application iterates through the
		/// <see cref="Db4objects.Db4o.IObjectSet">Db4objects.Db4o.IObjectSet</see>
		/// resultset of the query.
		/// <br /><br /><b>For detailed documentation please see
		/// <see cref="IQueryConfiguration.EvaluationMode(QueryEvaluationMode)">IQueryConfiguration.EvaluationMode(QueryEvaluationMode)
		/// 	</see>
		/// </b>
		/// </remarks>
		public static readonly Db4objects.Db4o.Config.QueryEvaluationMode Lazy = new Db4objects.Db4o.Config.QueryEvaluationMode
			("LAZY");

		private static readonly Db4objects.Db4o.Config.QueryEvaluationMode[] Modes = new 
			Db4objects.Db4o.Config.QueryEvaluationMode[] { Db4objects.Db4o.Config.QueryEvaluationMode
			.Immediate, Db4objects.Db4o.Config.QueryEvaluationMode.Snapshot, Db4objects.Db4o.Config.QueryEvaluationMode
			.Lazy };

		/// <summary>internal method, ignore please.</summary>
		/// <remarks>internal method, ignore please.</remarks>
		public virtual int AsInt()
		{
			for (int i = 0; i < Modes.Length; i++)
			{
				if (Modes[i] == this)
				{
					return i;
				}
			}
			throw new InvalidOperationException();
		}

		/// <summary>internal method, ignore please.</summary>
		/// <remarks>internal method, ignore please.</remarks>
		public static Db4objects.Db4o.Config.QueryEvaluationMode FromInt(int i)
		{
			return Modes[i];
		}

		public override string ToString()
		{
			return _id;
		}
	}
}
