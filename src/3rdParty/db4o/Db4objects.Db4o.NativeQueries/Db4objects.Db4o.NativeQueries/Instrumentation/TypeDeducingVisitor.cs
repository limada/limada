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
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Instrumentation
{
	internal class TypeDeducingVisitor : IComparisonOperandVisitor
	{
		private ITypeRef _predicateClass;

		private ITypeRef _clazz;

		private IReferenceProvider _referenceProvider;

		public TypeDeducingVisitor(IReferenceProvider provider, ITypeRef predicateClass)
		{
			this._predicateClass = predicateClass;
			this._referenceProvider = provider;
			_clazz = null;
		}

		public virtual void Visit(PredicateFieldRoot root)
		{
			_clazz = _predicateClass;
		}

		public virtual void Visit(CandidateFieldRoot root)
		{
		}

		//		_clazz=_candidateClass;
		public virtual void Visit(StaticFieldRoot root)
		{
			_clazz = root.Type;
		}

		public virtual ITypeRef OperandClass()
		{
			return _clazz;
		}

		public virtual void Visit(ArithmeticExpression operand)
		{
		}

		public virtual void Visit(ConstValue operand)
		{
			_clazz = _referenceProvider.ForType(operand.Value().GetType());
		}

		public virtual void Visit(FieldValue operand)
		{
			_clazz = operand.Field.Type;
		}

		public virtual void Visit(ArrayAccessValue operand)
		{
			operand.Parent().Accept(this);
			_clazz = _clazz.ElementType;
		}

		public virtual void Visit(MethodCallValue operand)
		{
			_clazz = operand.Method.ReturnType;
		}
	}
}
