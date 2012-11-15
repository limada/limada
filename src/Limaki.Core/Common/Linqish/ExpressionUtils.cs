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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Limaki.Common.Linqish {

    public class ExpressionUtils {

        public static MemberInfo MemberInfo<T, TMember>(Expression<Func<T, TMember>> exp) {
            var member = exp.Body as MemberExpression;
            if (member != null)
                return member.Member;
            throw new ArgumentException(string.Format("{0} is not a MemberExpression", exp.ToString()));
        }

        public static string MemberName<T, TMember>(Expression<Func<T, TMember>> exp) {
            return MemberInfo(exp).Name;
        }

        public static Expression<T> ToLamda<T>(Expression<T> expr) {
            return expr;
        }

      
    }

   
}