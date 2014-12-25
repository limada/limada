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

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Common.Linqish {
    
    /// <summary>
    /// Visitor that calculates a hash of an expression
    /// with ideas from 
    /// http://stackoverflow.com/questions/19678074/caching-compiled-expression-tree
    /// </summary>
    /// </summary>
    public class ExpressionHashVisitor : ExpressionVisitor {

        public ExpressionHashVisitor (Expression expression) {
            Visit (expression);
        }

        private const int NullHashCode = 0x61E04917;

        protected void AddHash (object item) {
            var member = item as MemberInfo;
            if (member != null) {
                var h = member.Name.GetHashCode ();

                var declaringType = member.DeclaringType;
                if (declaringType != null && declaringType.AssemblyQualifiedName != null)
                    h = KeyMaker.AddHashCode (h, declaringType.AssemblyQualifiedName.GetHashCode ());
                Hash = KeyMaker.AddHashCode (Hash, h);
                return;
            }

            if (item == null)
                Hash = KeyMaker.AddHashCode (Hash, NullHashCode);
            else
                Hash = KeyMaker.AddHashCode (Hash, KeyMaker.GetHashCode (item));

        }

        protected void AddHash (params object[] items) {
            foreach (var i in items) AddHash (i);
        }

        public int Hash { get; protected set; }

        public override Expression Visit (Expression node) {
            if (null == node)
                return null;

            AddHash (node.NodeType, node.Type);
            return base.Visit (node);
        }

        protected override Expression VisitBinary (BinaryExpression node) {
            AddHash (node.Method, node.IsLifted, node.IsLiftedToNull);

            return base.VisitBinary (node);
        }

        protected override CatchBlock VisitCatchBlock (CatchBlock node) {
            AddHash (node.Test);
            return base.VisitCatchBlock (node);
        }

        protected override Expression VisitConstant (ConstantExpression node) {
            var nodeValues = node.Value as IEnumerable;

            if (nodeValues == null)
                AddHash (node.Value);
            else {
                foreach (var item in nodeValues) {
                    AddHash (item);
                }
            }

            return base.VisitConstant (node);
        }

        protected override Expression VisitDebugInfo (DebugInfoExpression node) {
            AddHash (node.Document,
                node.EndColumn,
                node.EndLine,
                node.IsClear,
                node.StartColumn,
                node.StartLine);

            return base.VisitDebugInfo (node);
        }

        protected override Expression VisitDynamic (DynamicExpression node) {
            AddHash (node.Binder, node.DelegateType);
            return base.VisitDynamic (node);
        }

        protected override ElementInit VisitElementInit (ElementInit node) {
            AddHash (node.AddMethod);
            return base.VisitElementInit (node);
        }

        protected override Expression VisitGoto (GotoExpression node) {
            AddHash (node.Kind);

            return base.VisitGoto (node);
        }

        protected override Expression VisitIndex (IndexExpression node) {
            AddHash (node.Indexer);
            return base.VisitIndex (node);
        }

        protected override LabelTarget VisitLabelTarget (LabelTarget node) {
            AddHash (node.Name, node.Type);
            return base.VisitLabelTarget (node);
        }

        protected override Expression VisitLambda<T> (Expression<T> node) {
            AddHash (node.Name, node.ReturnType, node.TailCall);
            return base.VisitLambda (node);
        }
        
        protected override Expression VisitMember (MemberExpression node) {
            AddHash (node.Member);
            return base.VisitMember (node);
        }
        
        protected override MemberBinding VisitMemberBinding (MemberBinding node) {
            AddHash ((int)node.BindingType, node.Member);
            return base.VisitMemberBinding (node);
        }

        protected override MemberListBinding VisitMemberListBinding (MemberListBinding node) {
            AddHash ((int) node.BindingType, node.Member);
            return base.VisitMemberListBinding (node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding (MemberMemberBinding node) {
            AddHash ((int) node.BindingType, node.Member);
            return base.VisitMemberMemberBinding (node);
        }

        protected override Expression VisitMethodCall (MethodCallExpression node) {
            AddHash (node.Method);
            return base.VisitMethodCall (node);
        }

        protected override Expression VisitNew (NewExpression node) {
            AddHash (node.Constructor);
            return base.VisitNew (node);
        }

        protected override Expression VisitNewArray (NewArrayExpression node) {
            AddHash (node.Type);
            return base.VisitNewArray (node);
        }

        protected override Expression VisitParameter (ParameterExpression node) {
            AddHash (node.IsByRef);
            return base.VisitParameter (node);
        }
        
        protected override Expression VisitSwitch (SwitchExpression node) {
            AddHash (node.Comparison);
            return base.VisitSwitch (node);
        }


        protected override Expression VisitTypeBinary (TypeBinaryExpression node) {
            AddHash (node.TypeOperand);
            return base.VisitTypeBinary (node);
        }
        
        protected override Expression VisitUnary (UnaryExpression node) {
            AddHash (node.IsLifted, node.IsLiftedToNull, node.Method);
            return base.VisitUnary (node);
        }

        
    }
}