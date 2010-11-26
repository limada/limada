/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
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
using Limaki.Widgets;
using Limaki.Tests.Widget;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.Graph;

namespace Limaki.Tests.View.Widget {
    public class WidgetDataFactory : BasicTestDataFactory<IWidget, IEdgeWidget> {
        protected override void CreateItems() {
            var factory = Registry.Pool.TryGetCreate<IWidgetFactory> ();
            One = factory.CreateWidget("One");
            Two = factory.CreateWidget("Two");
            Three = factory.CreateWidget("Three");
            Aside = factory.CreateWidget("Aside");
            Single = factory.CreateWidget("Single");
        }

        protected override IEdgeWidget CreateEdge(IWidget root, IWidget leaf) {
            var result = Registry.Pool.TryGetCreate<IWidgetFactory>().CreateEdgeWidget("", root, leaf);
            result.Data = GraphUtils.EdgeString<IWidget, IEdgeWidget>(result);
            return result;
        }
    }

    public class BasicWidgetGraphTest : BasicGraphTests<IWidget, IEdgeWidget> {
        public override BasicTestDataFactory<IWidget, IEdgeWidget> GetFactory() {
            return new WidgetDataFactory();
        }
    }
}
