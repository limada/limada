/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011 Lytico
 *
* http://www.limada.org
 * 
 */

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Limaki.Common.Linqish {

    public class PropertyExtractor : ExpressionVisitor {

        private PropertyInfo _member;
        public PropertyInfo GetPropertyInfo<T, TMember>(Expression<Func<T, TMember>> exp) {
            _member = null;
            this.Visit(exp);
            return _member;
        }
       
        protected override Expression VisitMember(MemberExpression m) {
            var result = base.VisitMember(m);
            var exp = result as MemberExpression;
            if (exp != null && exp.Member.MemberType == MemberTypes.Property) {
                _member = exp.Member as PropertyInfo;
            }
            return result;
        }
    }
}