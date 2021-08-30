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
#if CF || SILVERLIGHT

using System;
using System.Reflection;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;
using Cecil.FlowAnalysis;
using Cecil.FlowAnalysis.ActionFlow;
using Cecil.FlowAnalysis.CodeStructure;

using Mono.Cecil;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class CecilMethodAnalyser : IMethodAnalyser
	{
		private static ICache4<MethodDefinition, ActionFlowGraph> _graphCache =
			CacheFactory<MethodDefinition, ActionFlowGraph>.For(CacheFactory.New2QXCache(5));

		private readonly Expression _queryExpression;

		private CecilMethodAnalyser(ActionFlowGraph graph)
		{
			if (graph == null) throw new ArgumentNullException("graph");

			_queryExpression = QueryExpressionFinder.FindIn(graph);
		}

		public void Run(QueryBuilderRecorder recorder)
		{
			if (_queryExpression == null) throw new QueryOptimizationException("No query expression");

			_queryExpression.Accept(new CodeQueryBuilder(recorder));
		}

		public static IMethodAnalyser FromMethod(MethodInfo info)
		{
			return GetAnalyserFor(ResolveMethod(info));
		}

		private static MethodDefinition ResolveMethod(MethodInfo info)
		{
			if (info == null) throw new ArgumentNullException("info");

			var method = MetadataResolver.Instance.ResolveMethod(info);

			if (method == null) throw new QueryOptimizationException(
				string.Format("Cannot resolve method {0}", info));

			return method;
		}

		private static IMethodAnalyser GetAnalyserFor(MethodDefinition method)
		{
			var graph = _graphCache.Produce(method, CreateActionFlowGraph);
			return new CecilMethodAnalyser(graph);
		}

		private static ActionFlowGraph CreateActionFlowGraph(MethodDefinition method)
		{
			return FlowGraphFactory.CreateActionFlowGraph(FlowGraphFactory.CreateControlFlowGraph(method));
		}
	}
}

#endif
