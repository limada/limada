/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using Limaki.Common.Collections;
using Limaki.Graphs;
using NUnit.Framework;

namespace Limada.Tests.Model {

    public class MapperTester<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public bool TargetContainsSource (
            IGraph<TItemOne, TEdgeOne> source,
            IGraph<TItemTwo, TEdgeTwo> target,
            Func<TItemOne, TItemTwo> getItem,
            ICollection<TItemOne> notContained) {
            bool result = true;

            if (notContained == null) {
                notContained = new EmptyCollection<TItemOne> ();
            }


            foreach (var item in source) {
                if (!(item is TEdgeOne)) {
                    bool contains = getItem (item) != null;
                    if (!contains) {
                        notContained.Add (item);
                    }
                    result = result && contains;
                }
            }


            foreach (var edge in source.Edges ()) {
                bool contains = getItem (edge) != null;
                if (!contains) {
                    notContained.Add (edge);
                }
                result = result && contains;
            }
            return result;
        }

        public void ProveConversion (
            IGraph<TItemOne, TEdgeOne> source,
            IGraph<TItemTwo, TEdgeTwo> target,
            Func<TItemOne, TItemTwo> getItem
            ) {

            var failedItems = new Set<TItemOne> ();

            bool contains =
                TargetContainsSource (source, target, getItem, failedItems);

            if (!contains) {
                var message = new StringWriter ();
                foreach (var item in failedItems) {
                    message.WriteLine (item.ToString () + "\tnot in " + target.GetType ().Name);
                }
                Assert.Fail (message.ToString ());
            }
        }

    }
}