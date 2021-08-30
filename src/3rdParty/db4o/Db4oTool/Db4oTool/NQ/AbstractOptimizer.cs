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
using Db4objects.Db4o.Instrumentation.Cecil;
using Db4objects.Db4o.NativeQueries;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Instrumentation;
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.NQ
{
	public abstract class AbstractOptimizer : AbstractAssemblyInstrumentation
	{
		public void OptimizePredicate(TypeDefinition type, MethodDefinition match, IExpression e)
		{
			TraceInfo("Optimizing '{0}' ({1})", type, e);

			new SODAMethodBuilder(new CecilTypeEditor(type)).InjectOptimization(e);
		}

		public IExpression GetExpression(MethodDefinition match)
		{
			try
			{
				return QueryExpressionBuilder.FromMethodDefinition(match);
			}
			catch (Exception x)
			{
				TraceWarning("WARNING: Predicate '{0}' could not be optimized. {1}", match, x.Message);
				TraceVerbose("{0}", x);
			}
			return null;
		}

		protected override void BeforeAssemblyProcessing()
		{
			_processedCount = 0;
		}

		protected override void AfterAssemblyProcessing()
		{
			TraceInfo("{0} {1} processed.", _processedCount, TargetName(_processedCount));
		}

		protected abstract string TargetName(int count);

		protected int _processedCount;
	}
}