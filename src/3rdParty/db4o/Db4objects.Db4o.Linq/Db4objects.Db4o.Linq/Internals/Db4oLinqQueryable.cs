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
#if !CF_3_5

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Db4objects.Db4o.Linq.Expressions;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class Db4oQueryable<T> : IDb4oLinqQueryable<T>, IQueryProvider
	{
		Expression _expression;
		IDb4oLinqQuery<T> _query;

		public Type ElementType
		{
			get { return typeof(T); }
		}

		public Expression Expression
		{
			get { return _expression; }
		}

		public IQueryProvider Provider
		{
			get { return this; }
		}

		public Db4oQueryable(Expression expression)
		{
			_expression = expression;
		}

		public Db4oQueryable(IDb4oLinqQuery<T> query)
		{
			_expression = Expression.Constant(this);
			_query = query;
		}

		public IDb4oLinqQuery GetQuery()
		{
			return _query;
		}

		public IEnumerator<T> GetEnumerator ()
		{
			return Execute<IEnumerable<T>>(_expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
 			return new Db4oQueryable<TElement>(expression);
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return (IQueryable)Activator.CreateInstance(
				MakeGenericDb4oQueryable(expression.Type),
				expression);
		}

		private static Type MakeGenericDb4oQueryable(Type type)
		{
			return typeof(Db4oQueryable<>).MakeGenericType(type.GetFirstGenericArgument());
		}

		private static Expression TransformQuery(Expression expression)
		{
			var result = QueryableTransformer.Transform(expression);
			return result;
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return Expression.Lambda<Func<TResult>>(TransformQuery(expression)).Compile().Invoke();
		}

		public object Execute(Expression expression)
		{
 			return Expression.Lambda(TransformQuery(expression)).Compile().DynamicInvoke();
		}
	}
}

#endif