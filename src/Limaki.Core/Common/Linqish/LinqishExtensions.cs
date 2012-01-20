using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System;

namespace Limada.Common.Linqish {
    public static class LinqishExtensions {
        public static IEnumerable<T> Yield<T>(this IEnumerable<T> query) {
            foreach (var item in query)
                yield return item;
        }
    }
    public class ExpressionUtils {
        protected static PropertyExtractor Extractor = new PropertyExtractor();
        public static string NameOf<T, TMember>(Expression<Func<T, TMember>> exp) {
            return Extractor.GetPropertyInfo(exp).Name;
        }

        public static Expression Expression<T>(Expression<T> expr) {
            return expr;
        }
    }
    public class PropertyExtractor : LinqKit.ExpressionVisitor {
        private PropertyInfo _member;
        public PropertyInfo GetPropertyInfo<T, TMember>(Expression<Func<T, TMember>> exp) {
            _member = null;
            this.Visit(exp);
            return _member;
        }

        protected override Expression VisitMemberAccess(MemberExpression m) {
            var result = base.VisitMemberAccess(m);
            var exp = result as MemberExpression;
            if (exp != null && exp.Member.MemberType == MemberTypes.Property) {
                _member = exp.Member as PropertyInfo;
            }
            return result;
        }
    }
}