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

using System.Collections.Generic;
using Cecil.FlowAnalysis.ActionFlow;
using Cecil.FlowAnalysis.CodeStructure;

using Mono.Cecil;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class QueryExpressionFinder
	{
		private ActionFlowGraph _graph;
		private Expression _queryExpression;
		private Dictionary<int, Expression> _variables;

		private Expression QueryExpression
		{
			get { return _queryExpression; }
		}

		private QueryExpressionFinder(ActionFlowGraph graph)
		{
			_graph = graph;
			_variables = new Dictionary<int, Expression>();
			FindQueryExpression();
		}

		public static Expression FindIn(ActionFlowGraph graph)
		{
			var finder = new QueryExpressionFinder(graph);
			return finder.QueryExpression;
		}

		private static ActionBlock GetFirstBlock(ActionFlowGraph graph)
		{
			if (graph.Blocks.Count < 1) return null;
			return graph.Blocks[0];
		}

		private void FindQueryExpression()
		{
			var block = GetFirstBlock(_graph);
			while (block != null)
			{
				switch (block.ActionType)
				{
					case ActionType.Invoke:
						block = OnInvokeAction((InvokeActionBlock)block);
						break;
					case ActionType.ConditionalBranch:
						block = OnConditionalBranch((ConditionalBranchActionBlock)block);
						break;
					case ActionType.Branch:
						block = OnBranch((BranchActionBlock)block);
						break;
					case ActionType.Assign:
						block = OnAssign((AssignActionBlock)block);
						break;
					case ActionType.Return:
						block = OnReturn((ReturnActionBlock)block);
						break;
				}
			}
		}

		private ActionBlock OnInvokeAction(InvokeActionBlock block)
		{
			MethodInvocationExpression invocation = block.Expression;
			if (!IsActivateInvocation(invocation)) CannotOptimize(invocation);

			return block.Next;
		}

		private ActionBlock OnConditionalBranch(ConditionalBranchActionBlock block)
		{
			throw new QueryOptimizationException();
		}

		private ActionBlock OnBranch(BranchActionBlock block)
		{
			return block.Target;
		}

		private ActionBlock OnAssign(AssignActionBlock block)
		{
			var assign = block.AssignExpression;
			var variable = assign.Target as VariableReferenceExpression;

			if (variable == null) CannotOptimize(assign);
			if (HasBeenAlreadyAssignedTo(variable)) CannotOptimize(assign.Expression);

			_variables.Add(GetVariableIndex(variable), assign.Expression);

			return block.Next;
		}

		private bool HasBeenAlreadyAssignedTo(VariableReferenceExpression variable)
		{
			return _variables.ContainsKey(GetVariableIndex(variable));
		}

		private ActionBlock OnReturn(ReturnActionBlock block)
		{
			Expression expression = block.Expression;
			VariableReferenceExpression variable = expression as VariableReferenceExpression;

			_queryExpression =
				variable != null
					? _variables[GetVariableIndex(variable)]
					: expression;

			return null;
		}

		private static bool IsActivateInvocation(MethodInvocationExpression invocation)
		{
			MethodReferenceExpression methodRef = invocation.Target as MethodReferenceExpression;
			if (null == methodRef) return false;
			return IsActivateMethod(methodRef.Method);
		}

		private static bool IsActivateMethod(MethodReference method)
		{
			return method.Name == "Activate";
		}

		private static int GetVariableIndex(VariableReferenceExpression variable)
		{
			return variable.Variable.Index;
		}

		private static void CannotOptimize(Expression expression)
		{
			throw new QueryOptimizationException(ExpressionPrinter.ToString(expression));
		}
	}
}

#endif
