/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Linq.Expressions;

namespace Limaki.Common.Linqish {

    public class ExpressionVisitVisitor : ExpressionVisitor {

        #region Non-Expression overrides

        public Func<ElementInit, ElementInit> VisitElementInitFunc { get; set; }
        protected override ElementInit VisitElementInit (ElementInit node) {
            return EvalFuncT (VisitElementInitFunc, node, base.VisitElementInit);
        }

        public Func<MemberAssignment, MemberAssignment> VisitMemberAssignmentFunc { get; set; }
        public Func<MemberListBinding, MemberListBinding> VisitMemberListBindingFunc { get; set; }
        public Func<MemberMemberBinding, MemberMemberBinding> VisitMemberMemberBindingFunc { get; set; }
        public Func<MemberBinding, MemberBinding> VisitMemberBindingFunc { get; set; }

        protected override MemberAssignment VisitMemberAssignment (MemberAssignment node) {
            return EvalFuncT (VisitMemberAssignmentFunc, node, base.VisitMemberAssignment);
        }


        protected override MemberBinding VisitMemberBinding (MemberBinding node) {
            return EvalFuncT (VisitMemberBindingFunc, node, base.VisitMemberBinding);
        }

        protected override MemberListBinding VisitMemberListBinding (MemberListBinding node) {
            return EvalFuncT (VisitMemberListBindingFunc, node, base.VisitMemberListBinding);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding (MemberMemberBinding node) {
            return EvalFuncT (VisitMemberMemberBindingFunc, node, base.VisitMemberMemberBinding);
        }

        #endregion
        
        protected Expression EvalFunc<T> (Func<T, Expression> visit, T node, Func<T, Expression> super) where T : Expression {

            if (visit != null) {
                T result = node;
                foreach (var v in visit.GetInvocationList ()) {
                    var exp = ((Func<T, Expression>) v) (result);
                    result = exp as T;
                    // break if expression type has changed:
                    if (result == null)
                        return exp;

                }
                return result;
            } else
                return super (node);

        }

        protected T EvalFuncT<T> (Func<T, T> visit, T node, Func<T, T> super) where T : class {

            if (visit != null) {
                foreach (var v in visit.GetInvocationList ()) {
                    node = ((Func<T, T>) v) (node) as T;
                }
                return node;
            } else
                return super (node);

        }

        public Func<Expression, Expression> VisitFunc { get; set; }
        public override Expression Visit (Expression node) {
            return EvalFunc (VisitFunc, node, base.Visit);
        }

        public Func<LambdaExpression, Expression> VisitLambdaFunc { get; set; }
        protected override Expression VisitLambda<T> (Expression<T> node) {
            return EvalFunc<LambdaExpression> (VisitLambdaFunc, node, e => base.VisitLambda<T> (e as Expression<T>));
        }

        		public Func<BinaryExpression, Expression> VisitBinaryFunc { get; set; }
		protected override Expression VisitBinary (BinaryExpression node) {
			return EvalFunc (VisitBinaryFunc,node,base.VisitBinary);
		}
		public Func<BlockExpression, Expression> VisitBlockFunc { get; set; }
		protected override Expression VisitBlock (BlockExpression node) {
			return EvalFunc (VisitBlockFunc,node,base.VisitBlock);
		}
		public Func<ConditionalExpression, Expression> VisitConditionalFunc { get; set; }
		protected override Expression VisitConditional (ConditionalExpression node) {
			return EvalFunc (VisitConditionalFunc,node,base.VisitConditional);
		}
		public Func<ConstantExpression, Expression> VisitConstantFunc { get; set; }
		protected override Expression VisitConstant (ConstantExpression node) {
			return EvalFunc (VisitConstantFunc,node,base.VisitConstant);
		}
		public Func<DebugInfoExpression, Expression> VisitDebugInfoFunc { get; set; }
		protected override Expression VisitDebugInfo (DebugInfoExpression node) {
			return EvalFunc (VisitDebugInfoFunc,node,base.VisitDebugInfo);
		}
		public Func<DynamicExpression, Expression> VisitDynamicFunc { get; set; }
		protected override Expression VisitDynamic (DynamicExpression node) {
			return EvalFunc (VisitDynamicFunc,node,base.VisitDynamic);
		}
		public Func<DefaultExpression, Expression> VisitDefaultFunc { get; set; }
		protected override Expression VisitDefault (DefaultExpression node) {
			return EvalFunc (VisitDefaultFunc,node,base.VisitDefault);
		}
		public Func<Expression, Expression> VisitExtensionFunc { get; set; }
		protected override Expression VisitExtension (Expression node) {
			return EvalFunc (VisitExtensionFunc,node,base.VisitExtension);
		}
		public Func<GotoExpression, Expression> VisitGotoFunc { get; set; }
		protected override Expression VisitGoto (GotoExpression node) {
			return EvalFunc (VisitGotoFunc,node,base.VisitGoto);
		}
		public Func<InvocationExpression, Expression> VisitInvocationFunc { get; set; }
		protected override Expression VisitInvocation (InvocationExpression node) {
			return EvalFunc (VisitInvocationFunc,node,base.VisitInvocation);
		}
		public Func<LabelExpression, Expression> VisitLabelFunc { get; set; }
		protected override Expression VisitLabel (LabelExpression node) {
			return EvalFunc (VisitLabelFunc,node,base.VisitLabel);
		}

		public Func<LoopExpression, Expression> VisitLoopFunc { get; set; }
		protected override Expression VisitLoop (LoopExpression node) {
			return EvalFunc (VisitLoopFunc,node,base.VisitLoop);
		}
		public Func<MemberExpression, Expression> VisitMemberFunc { get; set; }
		protected override Expression VisitMember (MemberExpression node) {
			return EvalFunc (VisitMemberFunc,node,base.VisitMember);
		}
		public Func<IndexExpression, Expression> VisitIndexFunc { get; set; }
		protected override Expression VisitIndex (IndexExpression node) {
			return EvalFunc (VisitIndexFunc,node,base.VisitIndex);
		}
		public Func<MethodCallExpression, Expression> VisitMethodCallFunc { get; set; }
		protected override Expression VisitMethodCall (MethodCallExpression node) {
			return EvalFunc (VisitMethodCallFunc,node,base.VisitMethodCall);
		}
		public Func<NewArrayExpression, Expression> VisitNewArrayFunc { get; set; }
		protected override Expression VisitNewArray (NewArrayExpression node) {
			return EvalFunc (VisitNewArrayFunc,node,base.VisitNewArray);
		}
		public Func<NewExpression, Expression> VisitNewFunc { get; set; }
		protected override Expression VisitNew (NewExpression node) {
			return EvalFunc (VisitNewFunc,node,base.VisitNew);
		}
		public Func<ParameterExpression, Expression> VisitParameterFunc { get; set; }
		protected override Expression VisitParameter (ParameterExpression node) {
			return EvalFunc (VisitParameterFunc,node,base.VisitParameter);
		}
		public Func<RuntimeVariablesExpression, Expression> VisitRuntimeVariablesFunc { get; set; }
		protected override Expression VisitRuntimeVariables (RuntimeVariablesExpression node) {
			return EvalFunc (VisitRuntimeVariablesFunc,node,base.VisitRuntimeVariables);
		}
		public Func<SwitchExpression, Expression> VisitSwitchFunc { get; set; }
		protected override Expression VisitSwitch (SwitchExpression node) {
			return EvalFunc (VisitSwitchFunc,node,base.VisitSwitch);
		}
		public Func<TryExpression, Expression> VisitTryFunc { get; set; }
		protected override Expression VisitTry (TryExpression node) {
			return EvalFunc (VisitTryFunc,node,base.VisitTry);
		}
		public Func<TypeBinaryExpression, Expression> VisitTypeBinaryFunc { get; set; }
		protected override Expression VisitTypeBinary (TypeBinaryExpression node) {
			return EvalFunc (VisitTypeBinaryFunc,node,base.VisitTypeBinary);
		}
		public Func<UnaryExpression, Expression> VisitUnaryFunc { get; set; }
		protected override Expression VisitUnary (UnaryExpression node) {
			return EvalFunc (VisitUnaryFunc,node,base.VisitUnary);
		}
		public Func<MemberInitExpression, Expression> VisitMemberInitFunc { get; set; }
		protected override Expression VisitMemberInit (MemberInitExpression node) {
			return EvalFunc (VisitMemberInitFunc,node,base.VisitMemberInit);
		}
		public Func<ListInitExpression, Expression> VisitListInitFunc { get; set; }
		protected override Expression VisitListInit (ListInitExpression node) {
			return EvalFunc (VisitListInitFunc,node,base.VisitListInit);
		}

    }
}