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

using Limaki.Common;
using Limaki.Graphs;
using Limaki.Tests.Graph.Basic;
using Limaki.View.Visuals;

namespace Limaki.Tests.View.Visuals {

    public class VisualGraphTestDataFactory : BasicGraphTestDataFactory<IVisual, IVisualEdge> {
        protected override void CreateItems() {
            var factory = Registry.Pooled<IVisualFactory> ();
            One = factory.CreateItem("One");
            Two = factory.CreateItem("Source");
            Three = factory.CreateItem("Three");
            Aside = factory.CreateItem("Aside");
            Single = factory.CreateItem("Single");
        }

        protected override IVisualEdge CreateEdge(IVisual root, IVisual leaf) {
            var result = Registry.Pooled<IVisualFactory>().CreateEdge(root, leaf, "");
            result.Data = GraphExtensions.EdgeString<IVisual, IVisualEdge>(result);
            return result;
        }
    }

    public class BasicVisualGraphTest : BasicGraphTests<IVisual, IVisualEdge> {
        public override BasicGraphTestDataFactory<IVisual, IVisualEdge> GetFactory() {
            return new VisualGraphTestDataFactory();
        }
    }
}
