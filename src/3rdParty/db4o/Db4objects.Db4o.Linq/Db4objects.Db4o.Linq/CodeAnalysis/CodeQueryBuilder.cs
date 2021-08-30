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
using Cecil.FlowAnalysis.CodeStructure;
using Mono.Cecil;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class CodeQueryBuilder : AbstractCodeStructureVisitor
	{
		private readonly QueryBuilderRecorder _recorder;

		public CodeQueryBuilder(QueryBuilderRecorder recorder)
		{
			_recorder = recorder;
		}

		public override void Visit(ArgumentReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(AssignExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(BinaryExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(CastExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(FieldReferenceExpression node)
		{
            Type descendingEnumType = ResolveDescendingEnumType(node);
            _recorder.Add(
                ctx =>
                    {
                        ctx.Descend(node.Field.Name);
                        ctx.PushDescendigFieldEnumType(descendingEnumType);
                    });
		}

		public override void Visit(LiteralExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(MethodInvocationExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(MethodReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(PropertyReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(ThisReferenceExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(UnaryExpression node)
		{
			CannotOptimize(node);
		}

		public override void Visit(VariableReferenceExpression node)
		{
			CannotOptimize(node);
		}

		private static void CannotOptimize(Expression expression)
		{
			throw new QueryOptimizationException(ExpressionPrinter.ToString(expression));
		}

        private static Type ResolveDescendingEnumType(FieldReferenceExpression node)
        {
			var type = ResolveType(node.Field.FieldType);
			if (type == null) return null;
			if (!type.IsEnum) return null;

			return type;
        }

		private static Type ResolveType(TypeReference type)
		{
			var assemblyName = type.Module.Assembly.Name.FullName;
			var assembly = System.Reflection.Assembly.Load(assemblyName);
			if (assembly == null) return null;

			return assembly.GetType(NormalizeTypeName(type));
		}

		private static string NormalizeTypeName(TypeReference type)
		{
			return type.FullName.Replace('/', '+');
		}
	}
}

#endif
