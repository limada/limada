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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Limaki.Common.Reflections;

namespace Limaki.Common.Linqish {

    public class ExpressionChangerVisit : ExpressionVisitor {

        public ExpressionVisitVisitor Visitor { get; set; }

        public static ExpressionChangerVisit Attach<S, T> (ExpressionVisitVisitor visitor) {
            return new ExpressionChangerVisit (visitor, typeof (S), typeof (T));
        }

        public static Expression Change<S, T> (Expression expression) { return new ExpressionChangerVisit (typeof (S), typeof (T)).Visit (expression); }
        public static Expression Change (Expression expression, Type source, Type sink) { return new ExpressionChangerVisit (source, sink).Visit (expression); }

        public ExpressionChangerVisit (Type source, Type sink):this(new ExpressionVisitVisitor(), source,sink) {}

        public ExpressionChangerVisit (ExpressionVisitVisitor visitor, Type source, Type sink) {
            this.Visitor = visitor;
            this.Source = source;
            this.Sink = sink;
            if (visitor != null) {
                visitor.VisitLambdaFunc += VisitLambdaNG;
                visitor.VisitMemberFunc += VisitMember;
                
                visitor.VisitMemberAssignmentFunc += VisitMemberAssignment;
                visitor.VisitMemberBindingFunc += VisitMemberBinding;
                visitor.VisitMemberMemberBindingFunc += VisitMemberMemberBinding;
                visitor.VisitMemberListBindingFunc += VisitMemberListBinding;
                
                visitor.VisitMemberInitFunc += VisitMemberInit;
                visitor.VisitListInitFunc += VisitListInit;
                visitor.VisitElementInitFunc += VisitElementInit;

                visitor.VisitParameterFunc += VisitParameter;
                visitor.VisitMethodCallFunc += VisitMethodCall;
                visitor.VisitUnaryFunc += VisitUnary;
                
            }
        }

        public Type Source { get; protected set; }
        public Type Sink { get; protected set; }

        public override Expression Visit (Expression node) {
            if (Visitor != null)
                return Visitor.Visit (node);
            else
                return base.Visit (node);
        }

        protected override Expression VisitLambda<T> (Expression<T> node) {
            return VisitLambdaNG (node);
        }

        protected virtual Expression VisitLambdaNG (LambdaExpression node) {

            var source = node.GetType ().GetGenericArguments ().First ();
            var target = ChangeType (source);
            var paras = node.Parameters.Select (p => this.Visit (p) as ParameterExpression);
            var changed = false;
            var body = this.Visit (node.Body);
            var result = Expression.Lambda (target, body, paras);
            return result;

            return node;
        }

        protected override Expression VisitConstant (ConstantExpression node) {
            return base.VisitConstant (node);
        }

        protected override Expression VisitUnary (UnaryExpression node) {
            var op = this.Visit (node.Operand);
            var type = ChangeType (node.Type);
            if (op != node.Operand || node.Type != type)
                return Expression.MakeUnary (node.NodeType, op, type);
            return node.Update (op);
        }

        protected virtual MemberBinding VisitBinding (MemberBinding binding) {
            switch (binding.BindingType) {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment ((MemberAssignment) binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding ((MemberMemberBinding) binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding ((MemberListBinding) binding);
                default:
                    throw new Exception (string.Format ("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected override MemberBinding VisitMemberBinding (MemberBinding node) {
            return VisitBinding (node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding (MemberMemberBinding node) {
            var member = VisitMember (node.Member, Source, Sink);
            var bindings = ExpressionVisitor.Visit (node.Bindings, VisitBinding);
            if (bindings != node.Bindings || member != node.Member) {
                return Expression.MemberBind (node.Member, bindings);
            }
            return node;
        }

        protected override Expression VisitMemberInit (MemberInitExpression node) {
            // TODO:
            var bindings = ExpressionVisitor.Visit (node.Bindings, VisitBinding);
            var exp = VisitNew (node.NewExpression) as NewExpression;
            if (bindings != node.Bindings || exp != node.NewExpression)
                return Expression.MemberInit (exp, bindings);
            return node;
        }
        
        protected override Expression VisitNew (NewExpression node) {
            // TODO:
            return node;
        }

        protected override MemberAssignment VisitMemberAssignment (MemberAssignment node) {
            var member = VisitMember (node.Member, Source, Sink);
            var expr = this.Visit (node.Expression);
            if (expr != node.Expression || member!=node.Member) {
                return Expression.Bind (member, expr);
            }
            return node;
        }

        protected override MemberListBinding VisitMemberListBinding (MemberListBinding node) {
            var member = VisitMember (node.Member, Source, Sink);
            var initializers = ExpressionVisitor.Visit(node.Initializers,this.VisitElementInit);
            if (initializers != node.Initializers || member != node.Member) {
                return Expression.ListBind (member, initializers);
            }
            return node;
        }

        protected override ElementInit VisitElementInit (ElementInit node) {
            // TODO:
            return node;
        }

        protected override Expression VisitListInit (ListInitExpression node) {
            // TODO:
            var n = this.VisitNew (node.NewExpression) as NewExpression;
            var initializers = ExpressionVisitor.Visit (node.Initializers, this.VisitElementInit);
            if (n != node.NewExpression || initializers != node.Initializers) {
                return Expression.ListInit (n, initializers);
            }
            return node;
        }

        protected virtual Expression VisitMember (MemberExpression node) {
            var source = node.Member.ReflectedType;
            var target = ChangeType (source);
            return VisitMember (node, source, target);
        }

        protected virtual Expression VisitParameter (ParameterExpression node) {
            var source = node.Type;
            if (source == Source && source != Sink) {
                return VisitParameter (Expression.Parameter (Sink, node.Name));
            }
            var target = ChangeType (source);
            return VisitParameter (node, source, target);

        }

        protected virtual Expression VisitMethodCall (MethodCallExpression node) {
            var instance = default(Expression);
            if (!node.Method.IsStatic) {
                instance = this.Visit (node.Object);
            }
            if (node.Method.IsGenericMethod) {
                var genericArguments = node.Method.GetGenericArguments ();
                var changed = false;
                for (int ia = 0; ia < genericArguments.Length; ia++) {
                    if (genericArguments[ia] == Source) {
                        genericArguments[ia] = Sink;
                        changed = true;
                    }
                }

                if (changed) {
                    var method = node.Method.GetGenericMethodDefinition ().MakeGenericMethod (genericArguments);
                    var margs = ExpressionVisitor.Visit (node.Arguments, this.Visit);
                    var exp = default(MethodCallExpression);
                    if (node.Method.IsStatic)
                        exp = Expression.Call (method, margs);
                    else
                        exp = Expression.Call (instance, method, margs);
                    return base.VisitMethodCall (exp);
                }
            }
            var args = ExpressionVisitor.Visit (node.Arguments, this.Visit);
            if (node.Method.IsStatic)
                return Expression.Call (node.Method, args);
            else
                return Expression.Call (instance, node.Method, args);
        }

        protected virtual MemberInfo VisitMember (MemberInfo member, Type source, Type sink) {

            //System.Reflection.FieldInfo
            //System.Reflection.MethodBase
            //System.Reflection.PropertyInfo
            // Use the Field, Property or PropertyOrField factory methods to create a MemberExpression

            if (member is PropertyInfo) {
                var info = member as PropertyInfo;
                var propertyType = ChangeType (info.PropertyType);
                return GetPropertyInfo (sink, propertyType, info.Name);
            }
            if (member is FieldInfo) {
                var info = member as FieldInfo;
                var propertyType = ChangeType (info.FieldType);
                return sink.GetField (info.Name); //GetPropertyInfo (target, propertyType, info.Name);
            }
            return member;
        }
       
        protected virtual Expression VisitMember (MemberExpression node, Type source, Type sink) {

            if (node.Member.ReflectedType == source) {
                var param = node.Expression as ParameterExpression;
                var ex = node.Expression;
                if (param != null && param.Type != source) {
                    ex = this.Visit (node.Expression);
                    return Expression.MakeMemberAccess (ex, node.Member);
                }
                
                var info = VisitMember (node.Member, source, sink);
                if (info != node.Member) {
                    ex = this.Visit (node.Expression);
                    return Expression.MakeMemberAccess (ex, info);
                } else
                    return node;
            }

            return this.Visit (node);
        }

        protected virtual Expression VisitParameter (ParameterExpression node, Type source, Type target) {
            if (node.Type == source)
                return GetParameter (target, node.Name);
            return this.Visit (node);
        }

        protected IDictionary<int, ParameterExpression> _params = new Dictionary<int, ParameterExpression> ();
        protected virtual ParameterExpression GetParameter (Type type, string name) {
            var key = type.GetHashCode () ^ name.GetHashCode ();
            ParameterExpression result = null;
            if (!_params.TryGetValue (key, out result)) {
                result = Expression.Parameter (type, name);
                _params[key] = result;
            }
            return result;
        }

        protected Type ChangeType (Type type, Type source, Type target) {
            if (type == source)
                return target;

            if (type.IsGenericType) {
                var argsSource = type.GetGenericArguments ();
                //if (!Array.Exists (argsSource, t => t == source))
                //    return generic;

                var args = argsSource.Select (t => ChangeType(t,source,target)).ToArray ();
                var gen = type.GetGenericTypeDefinition ();

                var result = gen.MakeGenericType (args);
                return result;
            }

            if (type.IsArray && type.GetElementType () == source) {
                return target.MakeArrayType ();
            }
   
            return type;
        }

        protected virtual Type ChangeType (Type type) {
            return ChangeType (type, Source, Sink);
        }
        
        protected MemberReflectionCache _memberReflectionCache = new MemberReflectionCache ();
        protected PropertyInfo GetPropertyInfo (Type target, Type propertyType, string memberName) {
            if (!_memberReflectionCache.ValidMember (target, propertyType, memberName))
                _memberReflectionCache.AddType (target);

            return _memberReflectionCache.GetPropertyInfo (target, propertyType, memberName);
        }
 
    }
}