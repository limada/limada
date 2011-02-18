/*
 * Limada 
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
using Limada.Tests.Basic;
using Limaki.Graphs;
using Limaki.Widgets;
using Limada.Model;
using Limaki.UnitTest;
using Limada.Tests.Model;
using Limada.View;
using NUnit.Framework;

using Id = System.Int64;


namespace Limada.Tests.View {
    public class ThingWidgetMapperTest : BasicThingGraphTest {
        
        public override void Setup() {
            base.Setup();

        }

        [Test]
        public virtual void Prove() {
            base.AddData();
            IGraph<IWidget, IEdgeWidget> widgetGraph = new WidgetGraph();
            GraphMapper<IThing,IWidget,ILink,IEdgeWidget> mapper =
                new GraphMapper<IThing, IWidget, ILink, IEdgeWidget>(
                (ThingGraph)this.Graph, 
                widgetGraph,
                new WidgetThingAdapter().ReverseAdapter());

            //mapper.showMarkers = false;
            mapper.ConvertOneTwo();
            Assert.AreEqual(this.Graph.Count,widgetGraph.Count);
            this.Graph.Clear();
            mapper.Clear();
            mapper.ConvertTwoOne();
            this.ReportGraph(this.Graph, "Reread");
            Assert.AreEqual(this.Graph.Count, widgetGraph.Count);

            ReportSummary();

        }

    }
}
