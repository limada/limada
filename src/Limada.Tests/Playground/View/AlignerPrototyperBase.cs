/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Tests;
using Limaki.Tests.View;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;
using Xwt.Html5.Backend;

namespace Limaki.Playground.View {

    public class AlignerPrototyperBase : DomainTest {

        Toolkit _savedToolkit = null;
        public override void Setup () {
            base.Setup ();
            _savedToolkit = Toolkit.CurrentEngine;
            Toolkit.Engine<Html5Engine> ().SetActive ();
            RegisterHtml5 (Registry.ConcreteContext);

        }

        public override void TearDown () {
            base.TearDown ();
            if (_savedToolkit != null)
                _savedToolkit.SetActive ();
        }

        protected IGraphScene<IVisual, IVisualEdge> SceneWithTestData (int exampleNr, int count = 1) {
            IGraphScene<IVisual, IVisualEdge> scene = null;
            var examples = new SceneExamples ();
            var testData = examples.Examples [exampleNr];
            testData.Data.Count = count;
            scene = examples.GetScene (testData.Data);

            return scene;

        }

        protected GraphSceneContextVisualizer<IVisual, IVisualEdge> SceneWorkerWithTestData (int exampleNr, AlignerOptions options, int count = 1) {

            var scene = SceneWithTestData (exampleNr, count);

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge> ();
            worker.Compose (scene, new VisualsRenderer ());
            worker.StyleSheet.BackColor = Colors.WhiteSmoke;
            worker.Layout.SetOptions (options);
            worker.Folder.ShowAllData ();
            worker.Modeller.Perform ();
            worker.Modeller.Finish ();

            scene.Focused = scene.Graph.FindRoots (null).First ();
            return worker;
        }

        protected GraphSceneContextVisualizer<IVisual, IVisualEdge> SceneWorkerWithTestData2 (int exampleNr, AlignerOptions options) {
            var worker = SceneWorkerWithTestData (exampleNr, options);
            var scene = worker.Scene;

            var scene2 = SceneWithTestData (exampleNr);

            var view = scene.Graph as SubGraph<IVisual, IVisualEdge>;
            var graph = view.Source;
            var graph2 = (scene2.Graph as SubGraph<IVisual, IVisualEdge>).Source;
            graph2.Where (e => e is Visual<string>).ForEach (e => { e.Data += "1"; });

            var root = scene.Focused;

            var root2 = graph2.FindRoots (null).First ();
            graph2.Elements ().ForEach (e => graph.Add (e));
            view.Add (root2);
            new Aligner<IVisual, IVisualEdge> (scene, worker.Layout, p => p.OneColumn (new IVisual [] { root, root2 }, options));

            scene.Focused = root2;

            worker.Modeller.Perform ();
            worker.Modeller.Finish ();
            return worker;
        }

        protected void ReportElems (IGraphScene<IVisual, IVisualEdge> scene, IEnumerable<IVisual> elms, ILocator<IVisual> locator, AlignerOptions options) {
            elms
               .OrderBy (e => locator.GetLocation (e), new PointComparer { Delta = options.Distance.Width, Order = options.PointOrder })
               .ForEach (e => ReportDetail ("\t{3}{0}\t{1}\t{2}", e.Data, locator.GetLocation (e), locator.GetSize (e), scene.Focused == e ? "*" : ""));

        }
        protected void ReportOptions (AlignerOptions options) {
            ReportDetail ("Options\t.Distance {0}", options.Distance);
        }

        protected Rectangle ReportExtent (IEnumerable<IVisual> elms, ILocator<IVisual> locator, AlignerOptions options) {
            var measure = new MeasureVisitBuilder<IVisual> (locator);
            Action<IVisual> visit = null;
            var fSizeToFit = measure.SizeToFit (ref visit, options.Distance, options.Dimension);
            var fBounds = measure.Bounds (ref visit);
            var fMinSize = measure.MinSize (ref visit);

            elms.ForEach (e => visit (e));
            ReportDetail ("Bounds {1}\tSizeToFit {0}\tMinSize  {2}", fSizeToFit (), fBounds (), fMinSize ());
            return fBounds ();
        }
    }

}