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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Db4objects.Db4o.Linq.Expressions
{
	class Set<T>
	{
#if SILVERLIGHT
		Dictionary<T, T> items = new Dictionary<T, T>();
#else
		HashSet<T> items = new HashSet<T>();
#endif
		public bool Contains(T item)
		{
#if SILVERLIGHT
			return items.ContainsKey(item);
#else
			return items.Contains(item);
#endif
		}

		public void Add(T item)
		{
#if SILVERLIGHT
			items.Add (item, item);
#else
			items.Add(item);
#endif
		}
	}

	public class SubtreeEvaluator : ExpressionTransformer
	{
		private Set<Expression> _candidates;

		private SubtreeEvaluator(Set<Expression> candidates)
		{
			_candidates = candidates;
		}

		public static Expression Evaluate(Expression expression)
		{
			var nominator = new Nominator(expression, exp => exp.NodeType != ExpressionType.Parameter);

			return new SubtreeEvaluator(nominator.Candidates).Visit(expression);
		}

		protected override Expression Visit(Expression expression)
		{
			if (expression == null) return null;
			if (_candidates.Contains(expression)) return EvaluateCandidate(expression);

			return base.Visit(expression);
		}

		private Expression EvaluateCandidate(Expression expression)
		{
			if (expression.NodeType == ExpressionType.Constant) return expression;

			var evaluator = Expression.Lambda(expression).Compile();
			return Expression.Constant(
#if !CF_3_5
				evaluator.DynamicInvoke(null),
#else
				evaluator.Method.Invoke(evaluator.Target, new object[0]),
#endif
				expression.Type);
		}

		class Nominator : ExpressionTransformer
		{
			readonly Func<Expression, bool> _predicate;
			readonly Set<Expression> _candidates = new Set<Expression>();
			bool cannotBeEvaluated;

			public Set<Expression> Candidates
			{
				get { return _candidates; }
			}

			public Nominator(Expression expression, Func<Expression, bool> predicate)
			{
				_predicate = predicate;

				Visit(expression);
			}

			private void AddCandidate(Expression expression)
			{
				_candidates.Add(expression);
			}

			// TODO: refactor
			protected override Expression Visit(Expression expression)
			{
				if (expression == null) return null;

				bool saveCannotBeEvaluated = cannotBeEvaluated;
				cannotBeEvaluated = false;

				base.Visit(expression);

				if (cannotBeEvaluated) return expression;

				if (_predicate(expression))
				{
					AddCandidate(expression);
				}
				else
				{
					cannotBeEvaluated = true;
				}

				cannotBeEvaluated |= saveCannotBeEvaluated;

				return expression;
			}
		}
	}
}
