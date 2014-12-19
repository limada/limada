/*
 * Limada 
 *
 * This file is a simplified composition of 
 * 
 * Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation.EvaluatableTreeFindingExpressionTreeVisitor 
 * Remotion.Linq.Parsing.ExpressionTreeVisitors.PartialEvaluatingExpressionTreeVisitor
 * 
 * by
 * 
 * re-motion Relinq (https://github.com/re-motion/Relinq)
 * 
 * Author of compositoin: Lytico
 * http://www.limada.org
 * 
 * original license:
 * 
 */

/*
   Copyright (c) rubicon IT GmbH, www.rubicon.eu
   
   See the NOTICE file distributed with this work for additional information
   regarding copyright ownership.  rubicon licenses this file to you under 
   the Apache License, Version 2.0 (the "License"); you may not use this 
   file except in compliance with the License.  You may obtain a copy of the 
   License at
   
     http://www.apache.org/licenses/LICENSE-2.0
   
   Unless required by applicable law or agreed to in writing, software 
   distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
   WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
   License for the specific language governing permissions and limitations
   under the License.
   
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Limaki.Common.Linqish {

    /// <summary>
    /// Takes an expression tree and first analyzes it for evaluatable subtrees (using <see cref="FindEvaluatableExpressionVisitor"/>), i.e.
    /// subtrees that can be pre-evaluated before actually generating the query. Examples for evaluatable subtrees are operations on constant
    /// values (constant folding), access to closure variables (variables used by the LINQ query that are defined in an outer scope), or method
    /// calls on known objects or their members. In a second step, it replaces all of the evaluatable subtrees (top-down and non-recursive) by 
    /// their evaluated counterparts.
    /// </summary>
    /// <remarks>
    /// This visitor visits each tree node at most twice: once via the <see cref="FindEvaluatableExpressionVisitor"/> for analysis and once
    /// again to replace nodes if possible (unless the parent node has already been replaced).
    /// </remarks>
    public class EvaluatingExpressionVisitor : ExpressionVisitor {
        /// <summary>
        /// Takes an expression tree and finds and evaluates all its evaluatable subtrees.
        /// </summary>
        public static Expression Evaluate (Expression expressionTree) {
            var evaluatableExpressions = FindEvaluatableExpressionVisitor.Analyze (expressionTree);

            var visitor = new EvaluatingExpressionVisitor (expressionTree, evaluatableExpressions);
            return visitor.Visit (expressionTree);
        }

        public class EvaluationExceptionExpression : Expression {
            private Exception ex;
            private Expression baseVisitedExpression;

            public EvaluationExceptionExpression (Exception ex, Expression baseVisitedExpression) {
                // TODO: Complete member initialization
                this.ex = ex;
                this.baseVisitedExpression = baseVisitedExpression;
            }
        }

        // _evaluatableExpressions contains a list of the expressions that are safe to be evaluated.
        private readonly ICollection<Expression> _evaluatableExpressions;

        private EvaluatingExpressionVisitor (Expression treeRoot, ICollection<Expression> evaluatableExpressions) {
            _evaluatableExpressions = evaluatableExpressions;
        }

        public override Expression Visit (Expression expression) {
            // Only evaluate expressions which do not use any of the surrounding parameter expressions. Don't evaluate
            // lambda expressions (even if you could), we want to analyze those later on.
            if (expression == null)
                return null;

            if (expression.NodeType == ExpressionType.Lambda || !_evaluatableExpressions.Contains (expression))
                return base.Visit (expression);

            Expression evaluatedExpression;
            try {
                evaluatedExpression = EvaluateSubtree (expression);
            } catch (Exception ex) {
                // Evaluation caused an exception. Skip evaluation of this expression and proceed as if it weren't evaluable.
                var baseVisitedExpression = base.Visit (expression);
                // Then wrap the result to capture the exception for the back-end.
                return new EvaluationExceptionExpression (ex, baseVisitedExpression);
            }

            if (evaluatedExpression != expression)
                return Evaluate (evaluatedExpression);

            return evaluatedExpression;
        }

        /// <summary>
        /// Evaluates an evaluatable <see cref="Expression"/> subtree, i.e. an independent expression tree that is compilable and executable
        /// without any data being passed in. The result of the evaluation is returned as a <see cref="ConstantExpression"/>; if the subtree
        /// is already a <see cref="ConstantExpression"/>, no evaluation is performed.
        /// </summary>
        /// <param name="subtree">The subtree to be evaluated.</param>
        /// <returns>A <see cref="ConstantExpression"/> holding the result of the evaluation.</returns>
        protected Expression EvaluateSubtree (Expression subtree) {
            if (subtree.NodeType == ExpressionType.Constant) {
                var constantExpression = (ConstantExpression) subtree;
                var valueAsIQueryable = constantExpression.Value as IQueryable;
                if (valueAsIQueryable != null && valueAsIQueryable.Expression != constantExpression)
                    return valueAsIQueryable.Expression;

                return constantExpression;
            } else {
                Expression<Func<object>> lambdaWithoutParameters = Expression.Lambda<Func<object>> (Expression.Convert (subtree, typeof (object)));
                var compiledLambda = lambdaWithoutParameters.Compile ();

                object value = compiledLambda ();
                return Expression.Constant (value, subtree.Type);
            }
        }
    }

    /// <summary>
    /// Analyzes an expression tree by visiting each of its nodes, finding those subtrees that can be evaluated without modifying the meaning of
    /// the tree.
    /// </summary>
    /// <remarks>
    /// An expression node/subtree is evaluatable if:
    /// <list type="bullet">
    /// <item>it is not a <see cref="ParameterExpression"/> or any non-standard expression, </item>
    /// <item>it is not a <see cref="MethodCallExpression"/> that involves an <see cref="IQueryable"/>, and</item>
    /// <item>it does not have any of those non-evaluatable expressions as its children.</item>
    /// </list>
    /// <see cref="ParameterExpression"/> nodes are not evaluatable because they usually identify the flow of
    /// some information from one query node to the next. 
    /// <see cref="MethodCallExpression"/> nodes that involve <see cref="IQueryable"/> parameters or object instances are not evaluatable because they 
    /// should usually be translated into the target query syntax.
    /// Non-standard expressions are not evaluatable because they cannot be compiled and evaluated by LINQ.
    /// </remarks>
    public class FindEvaluatableExpressionVisitor : ExpressionVisitor {

        public static ICollection<Expression> Analyze (Expression expressionTree) {

            var visitor = new FindEvaluatableExpressionVisitor ();
            visitor.Visit (expressionTree);
            return visitor._evaluatableExpressions;
        }

        private readonly ICollection<Expression> _evaluatableExpressions = new HashSet<Expression> ();
        private bool _isCurrentSubtreeEvaluatable;

        public override Expression Visit (Expression expression) {
            if (expression == null)
                return base.Visit (expression);

            // An expression node/subtree is evaluatable iff:
            // - by itself it would be evaluatable, and
            // - it does not contain any non-evaluatable expressions.

            // To find these nodes, first assume that the current subtree is evaluatable iff it is one of the standard nodes. Store the evaluatability 
            // of the parent node for later.
            bool isParentNodeEvaluatable = _isCurrentSubtreeEvaluatable;
            _isCurrentSubtreeEvaluatable = Enum.IsDefined (typeof (ExpressionType), expression.NodeType);

            // Then call the specific Visit... method for this expression. This will determine if this node by itself is not evaluatable by setting 
            // _isCurrentSubtreeEvaluatable to false if it isn't. It will also investigate the evaluatability info of the child nodes and set 
            // _isCurrentSubtreeEvaluatable accordingly.
            var visitedExpression = base.Visit (expression);

            // If the current subtree is still marked to be evaluatable, put it into the result list.
            if (_isCurrentSubtreeEvaluatable)
                _evaluatableExpressions.Add (expression);

            // Before returning to the parent node, set the evaluatability of the parent node.
            // The parent node can be evaluatable only if (among other things):
            //   - it was evaluatable before, and
            //   - the current subtree (i.e. the child of the parent node) is evaluatable.
            _isCurrentSubtreeEvaluatable &= isParentNodeEvaluatable; // the _isCurrentSubtreeEvaluatable flag now relates to the parent node again
            return visitedExpression;
        }

        protected override Expression VisitParameter (ParameterExpression expression) {
            // Parameters are not evaluatable.
            _isCurrentSubtreeEvaluatable = false;
            return base.VisitParameter (expression);
        }

        protected override Expression VisitMethodCall (MethodCallExpression expression) {
            // Method calls are only evaluatable if they do not involve IQueryable objects.

            if (IsQueryableExpression (expression.Object))
                _isCurrentSubtreeEvaluatable = false;

            for (int i = 0; i < expression.Arguments.Count && _isCurrentSubtreeEvaluatable; i++) {
                if (IsQueryableExpression (expression.Arguments[i]))
                    _isCurrentSubtreeEvaluatable = false;
            }

            return base.VisitMethodCall (expression);
        }

        protected override Expression VisitMember (MemberExpression expression) {
            // MemberExpressions are only evaluatable if they do not involve IQueryable objects.

            if (IsQueryableExpression (expression.Expression))
                _isCurrentSubtreeEvaluatable = false;

            return base.VisitMember (expression);
        }

        protected override Expression VisitMemberInit (MemberInitExpression expression) {
            Visit (expression.Bindings, VisitMemberBinding);

            // Visit the NewExpression only if the List initializers is evaluatable. It makes no sense to evaluate the ListExpression if the initializers
            // cannot be evaluated.

            if (!_isCurrentSubtreeEvaluatable)
                return expression;

            Visit (expression.NewExpression);
            return expression;
        }

        protected override Expression VisitListInit (ListInitExpression expression) {
            Visit (expression.Initializers, VisitElementInit);

            // Visit the NewExpression only if the List initializers is evaluatable. It makes no sense to evaluate the NewExpression if the initializers
            // cannot be evaluated.

            if (!_isCurrentSubtreeEvaluatable)
                return expression;

            Visit (expression.NewExpression);
            return expression;
        }

        private bool IsQueryableExpression (Expression expression) {
            return expression != null && typeof (IQueryable).IsAssignableFrom (expression.Type);
        }

        public Expression VisitPartialEvaluationException (EvaluatingExpressionVisitor.EvaluationExceptionExpression expression) {
            // PartialEvaluationExceptionExpression is not evaluable, and its children aren't either (so we don't visit them).
            _isCurrentSubtreeEvaluatable = false;
            return expression;
        }
    }
}
