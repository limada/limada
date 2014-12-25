/*
 * Limada 
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
using System.Text;

namespace Limaki.Common.Linqish {

    public class ExpressionCache {

        public int Hits { get; protected set; }
        public int Fails { get; protected set; }

        private IDictionary<int, Expression> _cache = new Dictionary<int, Expression> ();
        public bool TryGet (ref Expression expr) {
            var hash = ExpressionHashVisitor.GetHash (expr);
            Expression cExpr = null;
            if (_cache.TryGetValue (hash, out cExpr)) {
                expr = cExpr;
                Hits++;
                return true;
            }
            Fails++;
            return false;
        }

        public void Add (Expression expr) {
            var hash = ExpressionHashVisitor.GetHash (expr);
            _cache[hash] = expr;
        }
    }
}
