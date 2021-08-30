/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Limaki.Common.Linqish;

namespace Limaki.Data {

    public static class DomainQuoreExtensions {

        public static void CopyTo<T> (this T source, T sink) where T : IDomainQuore {

            var upsertCall = new CallCache (ExpressionUtils.Lambda<Action<T, IEnumerable<CallCache.Entity>>> ((q, e) => q.Quore.Upsert (e)));

            var sourceIori = source.Quore.Gateway.Iori;
            var sinkIori = sink.Quore.Gateway.Iori;

            Trace.WriteLine ($"{nameof (CopyTo)} source = {sourceIori.ToFileName ()} sink = {sinkIori.ToFileName ()}");

            foreach (var queryProp in typeof (T).GetProperties ().Where (p => typeof (IQueryable).IsAssignableFrom (p.PropertyType))) {

                using (var trans = sink.Quore.BeginTransaction ()) {
                    var s = queryProp.GetValue (source);
                    var type = queryProp.PropertyType.GenericTypeArguments.First ();
                    var meth = upsertCall.Getter (type);
                    Trace.WriteLine ($"{nameof (CopyTo)} {nameof (IQueryable)}<{type.Name}>");
                    meth.DynamicInvoke (sink, s);

                    trans.Commit ();
                }
            }

        }
    }
}
