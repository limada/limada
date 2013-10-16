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
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Tests;
using Limaki.Tests.View;
using Limaki.View;
using Limaki.View.Layout;
using Limaki.View.Visualizers;
using Limaki.View.Visuals;
using Limaki.View.Visuals.Rendering;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;


namespace Limaki.Playground.View {

    public class AlignerPrototyper : DomainTest {

        IGraphScene<IVisual, IVisualEdge> SceneWithTestData (int exampleNr) {
            IGraphScene<IVisual, IVisualEdge> scene = null;
            var examples = new SceneExamples();
            var testData = examples.Examples[exampleNr];
            testData.Data.Count = 1;
            scene = examples.GetScene(testData.Data);

            return scene;

        }

        GraphSceneContextVisualizer<IVisual, IVisualEdge> SceneWorkerWithTestData (int exampleNr, AlignerOptions options) {
            var scene = SceneWithTestData(exampleNr);

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge>();
            worker.Compose(scene, new VisualsRenderer());
            worker.StyleSheet.BackColor = Colors.WhiteSmoke;
            worker.Layout.SetOptions(options);
            worker.Folder.ShowAllData();
            worker.Receiver.Execute();
            worker.Receiver.Done();

            var scene2 = SceneWithTestData(exampleNr);

            var view = scene.Graph as SubGraph<IVisual, IVisualEdge>;
            var graph = view.Source;
            var graph2 = (scene2.Graph as SubGraph<IVisual, IVisualEdge>).Source;
            graph2.Where(e => e is Visual<string>).ForEach(e => { e.Data += "1"; });

            var root = graph.FindRoots(null).First();
            var root2 = graph2.FindRoots(null).First();
            graph2.Elements().ForEach(e => graph.Add(e));
            view.Add(root2);
            new Aligner<IVisual, IVisualEdge>(scene, worker.Layout, p => p.OneColumn(new IVisual[] { root, root2 }, options));

            scene.Focused = root2;

            worker.Receiver.Execute();
            worker.Receiver.Done();
            return worker;
        }

        void ReportElems (IGraphScene<IVisual, IVisualEdge> scene, IEnumerable<IVisual> elms, ILocator<IVisual> locator, AlignerOptions options) {
            elms
               .OrderBy(e => locator.GetLocation(e), new PointComparer { Delta = options.Distance.Width, Order = options.PointOrder })
               .ForEach(e => ReportDetail("\t{3}{0}\t{1}\t{2}", e.Data, locator.GetLocation(e), locator.GetSize(e), scene.Focused == e ? "*" : ""));

        }
        void ReportOptions (AlignerOptions options) {
            ReportDetail("Options\t.Distance {0}", options.Distance);
        }

        Rectangle ReportExtent (IEnumerable<IVisual> elms, ILocator<IVisual> locator, AlignerOptions options) {
            var measure = new MeasureVisitBuilder<IVisual>(locator);
            Action<IVisual> visit = null;
            var fSizeToFit = measure.SizeToFit(ref visit, options.Distance, options.Dimension);
            var fBounds = measure.Bounds(ref visit);
            var fMinSize = measure.MinSize(ref visit);

            elms.ForEach(e => visit(e));
            ReportDetail("Bounds {1}\tSizeToFit {0}\tMinSize  {2}", fSizeToFit(), fBounds(), fMinSize());
            return fBounds();
        }

        [Test]
        public void TestCollisions0 () {
            var options = new AlignerOptions {
                AlignX = Alignment.Start,
                AlignY = Alignment.Center,
                Dimension = Dimension.X,
                PointOrder = PointOrder.XY
            };

            var worker = SceneWorkerWithTestData(0, options);
            options.Distance = worker.Layout.Distance;

            var scene = worker.Scene;

            ILocator<IVisual> locator = new GraphSceneItemShapeLocator<IVisual, IVisualEdge> { GraphScene = scene };


            Action<IEnumerable<IVisual>> reportElems = elms => ReportElems(scene, elms, locator, options);
            Func<IEnumerable<IVisual>, Rectangle> reportExtent = elms => ReportExtent(elms, locator, options);

            var visibleItems = new HashSet<IVisual>(scene.Elements.Where(e => !(e is IVisualEdge)));
            var visibleBounds = locator.Bounds(visibleItems);
            reportElems(visibleItems);
            reportExtent(visibleItems);


            IEnumerable<IVisual> itemsToPlace = Walker.Create((scene.Graph as SubGraph<IVisual, IVisualEdge>).Source)
                .DeepWalk(scene.Focused, 0)
                .Select(l => l.Node)
                .ToArray();
            scene.Focused.Location = Point.Zero + worker.Layout.Distance;
            itemsToPlace.ForEach(e => scene.Graph.Add(e));

            itemsToPlace = itemsToPlace.Where(e => !(e is IVisualEdge));
            var aligner = new Aligner<IVisual, IVisualEdge>(scene, worker.Layout);

            aligner.Columns(scene.Focused, itemsToPlace, options);
            locator = aligner.Locator;
            reportElems(itemsToPlace);
            var bounds = reportExtent(itemsToPlace);
            var free = aligner.NextFreeSpace(bounds.Location, bounds.Size, itemsToPlace, options.Dimension, options.Distance);
            ReportDetail("Next free space {0}", free);
            var free2 = aligner.NearestNextFreeSpace(bounds.Location, bounds.Size, itemsToPlace, false, options.Dimension, options.Distance);
            ReportDetail("Nearest Next free space {0}", free);

            var dist = new Size(bounds.Location.X - free2.Location.X, bounds.Location.Y - free2.Location.Y);
            itemsToPlace.ForEach(e => aligner.Locator.SetLocation(e, aligner.Locator.GetLocation(e) - dist));

            aligner.Commit();
            worker.Receiver.Execute();
            worker.Receiver.Done();

            ReportPainter.PushPaint(ctx => worker.Painter.Paint(ctx));

            ReportPainter.PushPaint(ctx => {
                var translate = worker.Painter.Viewport.ClipOrigin;
                ctx.Translate(-translate.X, -translate.Y);
                ctx.SetLineWidth(.5);
                ctx.SetColor(Colors.Black);
                ctx.Rectangle(visibleBounds);
                ctx.Stroke();
                ctx.SetColor(Colors.Red);
                ctx.Rectangle(bounds);
                ctx.Stroke();

                ctx.SetColor(Colors.Green);
                ctx.Rectangle(free);
                ctx.Stroke();

                ctx.SetColor(Colors.LawnGreen);
                ctx.Rectangle(free2);
                ctx.Stroke();
                ctx.SetColor(Colors.White);
                ctx.ResetTransform();
            });

            WritePainter();
        }

        [Test]
        public void TestCollisions () {
            var options = new AlignerOptions {
                AlignX = Alignment.Center,
                AlignY = Alignment.Center,
                Dimension = Dimension.X,
                PointOrder = PointOrder.XY,
                Collisions = Limaki.View.Layout.Collisions.NextFree | Collisions.Toggle
            };

            var worker = SceneWorkerWithTestData(0, options);
            options.Distance = worker.Layout.Distance;

            ReportOptions(options);
            var scene = worker.Scene;

            ReportPainter.PushPaint(ctx => worker.Painter.Paint(ctx));

            ILocator<IVisual> locator = new GraphSceneItemShapeLocator<IVisual, IVisualEdge> { GraphScene = scene };

            Action<IEnumerable<IVisual>> reportElems = elms => ReportElems(scene, elms, locator, options);
            Func<IEnumerable<IVisual>, Rectangle> reportExtent = elms => ReportExtent(elms, locator, options);

            var visibleItems = new HashSet<IVisual>(scene.Elements.Where(e => !(e is IVisualEdge)));
            var visibleBounds = locator.Bounds(visibleItems);
            reportElems(visibleItems);
            reportExtent(visibleItems);


            IEnumerable<IVisual> itemsToPlace = Walker.Create((scene.Graph as SubGraph<IVisual, IVisualEdge>).Source)
                .DeepWalk(scene.Focused, 0)
                .Select(l => l.Node)
                .ToArray();
            scene.Focused.Location = Point.Zero + worker.Layout.Distance;
            itemsToPlace.ForEach(e => scene.Graph.Add(e));

            itemsToPlace = itemsToPlace.Where(e => !(e is IVisualEdge));
            var aligner = new Aligner<IVisual, IVisualEdge>(scene, worker.Layout);
            options.Dimension = options.Dimension == Dimension.X ? Dimension.Y : Dimension.X;
            aligner.Columns(scene.Focused, itemsToPlace, options);
            locator = aligner.Locator;
            reportElems(itemsToPlace);
            var bounds = reportExtent(itemsToPlace);
            var free = aligner.NextFreeSpace(bounds.Location, bounds.Size, itemsToPlace, options.Dimension, options.Distance);
            ReportDetail("Next free space {0}", free);
            var free2 = aligner.NearestNextFreeSpace(bounds.Location, bounds.Size, itemsToPlace,
                options.Collisions.HasFlag(Collisions.Toggle), options.Dimension, options.Distance);

            ReportDetail("Nearest Next free space {0}", free);

            var selected = new Walker<IVisual, IVisualEdge>(scene.Graph).DeepWalk(scene.Focused, 0).Select(l => l.Node);
            selected = itemsToPlace;
            aligner.Columns(scene.Focused, selected, options);

            aligner.Commit();
            worker.Receiver.Execute();
            worker.Receiver.Done();

            ReportPainter.PushPaint(ctx => worker.Painter.Paint(ctx));

            ReportPainter.PushPaint(ctx => {
                var translate = worker.Painter.Viewport.ClipOrigin;
                ctx.Translate(-translate.X, -translate.Y);
                ctx.SetLineWidth(.5);
                ctx.SetColor(Colors.Black);
                ctx.Rectangle(visibleBounds);
                ctx.Stroke();
                ctx.SetColor(Colors.Red);
                ctx.Rectangle(bounds);
                ctx.Stroke();

                ctx.SetColor(Colors.Green);
                ctx.Rectangle(free);
                ctx.Stroke();

                ctx.SetColor(Colors.LawnGreen);
                ctx.Rectangle(free2);
                ctx.Stroke();
                ctx.SetColor(Colors.White);
                ctx.ResetTransform();
            });



            WritePainter();
        }

        [Test]
        public void TestShowAllData () {
            var options = new AlignerOptions {
                AlignX = Alignment.Center,
                AlignY = Alignment.Start,
                Dimension = Dimension.X,
                PointOrder = PointOrder.XY,
                //Collisions = Collisions.NextFree
            };

            var scene = SceneWithTestData(0);
            var graphView = scene.Graph as SubGraph<IVisual, IVisualEdge>;
            var graph = graphView.Source;
            for (int i = 1; i < 6; i++)
                (SceneWithTestData(i).Graph as SubGraph<IVisual, IVisualEdge>)
                    .Source.ForEach(item => graph.Add(item));

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge>();
            worker.Compose(scene, new VisualsRenderer());
            worker.StyleSheet.BackColor = Colors.WhiteSmoke;
            worker.Layout.SetOptions(options);
            options.Distance = worker.Layout.Distance;

            Action allData = () =>
            {
                var roots = new Queue<IVisual>(graph.FindRoots(null));

                var walker = new Walker<IVisual, IVisualEdge>(graph);

                roots.ForEach(root => walker.DeepWalk(root, 0).ForEach(item => graphView.Sink.Add(item.Node)));
                walker = new Walker<IVisual, IVisualEdge>(graphView);
                var aligner = new Aligner<IVisual, IVisualEdge>(scene, worker.Layout);
                //var bounds = new Rectangle(worker.Layout.Border.Width, worker.Layout.Border.Height, 0, 0);
                var pos = new Point(worker.Layout.Border.Width, worker.Layout.Border.Height);
                roots.ForEach(root => {
                    var walk = walker.DeepWalk(root, 1).Where(l => !(l.Node is IVisualEdge)).ToArray();
                    var bounds = new Rectangle(pos, Size.Zero);
                    var cols = aligner.MeasureWalk(walk, ref bounds, options);
                    //bounds = aligner.NextFreeSpace(bounds.Location, bounds.Size, walk.Select(t => t.Node), Dimension.Y, options.Distance);
                    pos = new Point(pos.X, pos.Y+bounds.Size.Height+options.Distance.Height);
                    aligner.LocateColumns(cols, ref bounds, options);
                    //bounds.Top = bounds.Bottom + options.Distance.Height;
                   
                   
                });

                aligner.Commit();
                worker.Receiver.Execute();
                worker.Receiver.Done();
            };
            allData();
            allData();

            ReportPainter.PushPaint(ctx => worker.Painter.Paint(ctx));

            WritePainter();
        }


        [Test]
        public void TestExpand () {
            var options = new AlignerOptions {
                                                 AlignX = Alignment.Start,
                                                 AlignY = Alignment.Center,
                                                 Dimension = Dimension.X,
                                                 PointOrder = PointOrder.XY,
                                                
            };

            var worker = SceneWorkerWithTestData(3, options);
            options.Distance = worker.Layout.Distance;

            var roots = new IVisual[] {worker.Scene.Focused};
            var aligner = new Aligner<IVisual, IVisualEdge>(worker.Scene, worker.Layout);
            var deep = false;
            var graphView = worker.Scene.Graph as SubGraph<IVisual, IVisualEdge>;

            var affected = new SubGraphWorker<IVisual, IVisualEdge>
                    (graphView).Expand(roots, deep);

           

            var walker = new Walker<IVisual, IVisualEdge>(graphView);

            roots.ForEach(root => {
                var walk = (deep ? walker.DeepWalk(root, 1) : walker.ExpandWalk(root, 1))
                          .Where(l => !(l.Node is IVisualEdge)).ToArray();
                var bounds = new Rectangle(aligner.Locator.GetLocation(root), aligner.Locator.GetSize(root));
                options.Collisions = Collisions.None;
                var cols = aligner.MeasureWalk(walk, ref bounds, options);
                var removeCol = cols.Dequeue();

                if (options.Dimension == Dimension.X) {
                    var adjust = aligner.AlignDelta(bounds.Height, removeCol.Item2.Height, options.AlignY);
                    bounds.Location = new Point(bounds.X + removeCol.Item2.Width + options.Distance.Width, bounds.Y - adjust);
                } else {
                    var adjust = aligner.AlignDelta(bounds.Width, removeCol.Item2.Width, options.AlignX);
                    bounds.Location = new Point(bounds.X + adjust, bounds.Y + removeCol.Item2.Height + options.Distance.Height);
                }
                options.Collisions = Collisions.NextFree | Collisions.PerColumn | Collisions.Toggle;
                aligner.LocateColumns(cols, ref bounds, options);
            });

            aligner.Commit();
            worker.Receiver.Execute();
            worker.Receiver.Done();

            ReportPainter.PushPaint(ctx => worker.Painter.Paint(ctx));

            WritePainter();
        }
    }
}