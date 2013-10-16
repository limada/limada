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
using System.Text;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using Limaki.Tests.Visuals;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.Graph;

namespace Limaki.Tests.View.Visuals {
    public class VisualDataFactory : BasicTestDataFactory<IVisual, IVisualEdge> {
        protected override void CreateItems() {
            var factory = Registry.Pool.TryGetCreate<IVisualFactory> ();
            One = factory.CreateItem("One");
            Two = factory.CreateItem("Source");
            Three = factory.CreateItem("Three");
            Aside = factory.CreateItem("Aside");
            Single = factory.CreateItem("Single");
        }

        protected override IVisualEdge CreateEdge(IVisual root, IVisual leaf) {
            var result = Registry.Pool.TryGetCreate<IVisualFactory>().CreateEdge(root, leaf, "");
            result.Data = GraphExtensions.EdgeString<IVisual, IVisualEdge>(result);
            return result;
        }
    }

    public class BasicVisualGraphTest : BasicGraphTests<IVisual, IVisualEdge> {
        public override BasicTestDataFactory<IVisual, IVisualEdge> GetFactory() {
            return new VisualDataFactory();
        }
    }
}
