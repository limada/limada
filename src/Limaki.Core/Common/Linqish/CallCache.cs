using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Limaki.Common.Linqish {

    public class CallCache {
        
        public class Entity { }

        public CallCache (Expression call) { this.Call = call; }

        public Expression Call { get; protected set; }

        private Dictionary<Type, Delegate> _cache = new Dictionary<Type, Delegate> ();
        
        public Delegate Getter (Type cType) {
            Delegate getter = null;
            if (!_cache.TryGetValue (cType, out getter)) {
                var changer = new ExpressionChangerVisit (typeof (Entity), cType);
                var expr = changer.Visit (Call);
                getter = (expr as LambdaExpression).Compile ();

                _cache.Add (cType, getter);
            }
            return getter;
        }
    }
}