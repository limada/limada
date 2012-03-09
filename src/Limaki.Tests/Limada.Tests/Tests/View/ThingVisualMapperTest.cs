/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;
using Limada.Tests.Basic;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Visuals;
using Limada.Model;
using Limaki.UnitTest;
using Limada.Tests.Model;
using NUnit.Framework;

using Id = System.Int64;


namespace Limada.Tests.View {
    public class ThingVisualMapperTest : BasicThingGraphTest {
        
        public override void Setup() {
            base.Setup();

        }

        [Test]
        public virtual void Prove() {
            base.AddData();
            var visualGraph = new VisualGraph();
            var mapper =
                new GraphMapper<IThing, IVisual, ILink, IVisualEdge>(
                (IThingGraph)this.Graph, 
                visualGraph,
                new VisualThingAdapter().ReverseAdapter());

            //mapper.showMarkers = false;
            mapper.ConvertOneTwo();
            Assert.AreEqual(this.Graph.Count,visualGraph.Count);
            this.Graph.Clear();
            mapper.Clear();
            mapper.ConvertTwoOne();
            this.ReportGraph(this.Graph, "Reread");
            Assert.AreEqual(this.Graph.Count, visualGraph.Count);

            ReportSummary();

        }

    }
}
