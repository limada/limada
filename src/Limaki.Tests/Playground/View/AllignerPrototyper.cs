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


using NUnit.Framework;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using System;
using Xwt;
using Limaki.Visuals;
using Limaki.Tests.View;
using Limaki.Common.Linqish;
using Limaki.View;
using Limaki.View.Layout;
using Xwt.Drawing;
using Limaki.View.Visuals;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Rendering;


namespace Limaki.Playground.View {
    public class AlignerPrototyper : Html5DomainTest {
        IGraphScene<IVisual, IVisualEdge> SceneWithTestData (int example) {
            IGraphScene<IVisual, IVisualEdge> scene = null;
            var examples = new SceneExamples();
            var testData = examples.Examples[example];
            testData.Data.Count = 1;
            scene = examples.GetScene(testData.Data);

            return scene;

        }

        GraphSceneContextVisualizer<IVisual, IVisualEdge> SceneWorkerWithTestData (int example, AlignerOptions options) {
            var scene = SceneWithTestData(example);

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge>();
            worker.Compose(scene, new VisualsRenderer());
            worker.StyleSheet.BackColor = Colors.WhiteSmoke;
            worker.Layout.SetOptions(options);
            worker.Folder.ShowAllData();
            worker.Receiver.Execute();
            worker.Receiver.Done();

            var scene2 = SceneWithTestData(example);

            var view = scene.Graph as GraphView<IVisual, IVisualEdge>;
            var graph = view.Two;
            var graph2 = (scene2.Graph as GraphView<IVisual, IVisualEdge>).Two;
            graph2.Where(e => e is Visual<string>).ForEach(e => { e.Data += "1"; });

            var root = graph.FindRoots(null).First();
            var root2 = graph2.FindRoots(null).First();
            graph2.Elements().ForEach(e => graph.Add(e));
            view.Add(root2);
            new Aligner<IVisual, IVisualEdge>(scene, worker.Layout, p => p.OneColumn(new IVisual[] { root, root2 },options));

            scene.Focused = root2;

            worker.Receiver.Execute();
            worker.Receiver.Done();
            return worker;
        }

        void ReportElems (IGraphScene<IVisual,IVisualEdge> scene, IEnumerable<IVisual> elms, ILocator<IVisual> locator, AlignerOptions options) {
            elms
               .OrderBy(e => locator.GetLocation(e), new PointComparer { Delta = options.Distance.Width, Order = options.PointOrder })
               .ForEach(e => ReportDetail("\t{3}{0}\t{1}\t{2}", e.Data, locator.GetLocation(e), locator.GetSize(e), scene.Focused == e ? "*" : ""));

        }
        void ReportOptions (AlignerOptions options) {
            ReportDetail("Options\t.Distance {0}", options.Distance);
        }

        Rectangle ReportExtent(IEnumerable<IVisual> elms,ILocator<IVisual> locator, AlignerOptions options) {
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
                PointOrder = PointOrder.LeftToRight
            };

            var worker = SceneWorkerWithTestData(0,options);
            options.Distance = worker.Layout.Distance;

            var scene = worker.Scene;

            ILocator<IVisual> locator = new GraphSceneItemShapeLocator<IVisual, IVisualEdge> { GraphScene = scene };
            

            Action<IEnumerable<IVisual>> reportElems = elms => ReportElems(scene, elms, locator, options);
            Func<IEnumerable<IVisual>, Rectangle> reportExtent = elms => ReportExtent(elms, locator, options);

            var visibleItems = new HashSet<IVisual>(scene.Elements.Where(e => !(e is IVisualEdge)));
            var visibleBounds = locator.Bounds(visibleItems);
            reportElems(visibleItems);
            reportExtent(visibleItems);


            IEnumerable<IVisual> itemsToPlace = Walker.Create((scene.Graph as GraphView<IVisual, IVisualEdge>).Two)
                .DeepWalk(scene.Focused, 0)
                .Select(l => l.Node)
                .ToArray();
            scene.Focused.Location = Point.Zero+worker.Layout.Distance;
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
            
            ReportPainter.Paint(ctx => worker.Painter.Paint(ctx));
           
            ReportPainter.Paint(ctx => {
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
                Dimension = Dimension.Y,
                PointOrder = PointOrder.LeftToRight,
                Collisions = Limaki.View.Layout.Collisions.NextFree|Collisions.Toggle
            };

            var worker = SceneWorkerWithTestData(0,options);
            options.Distance = worker.Layout.Distance;

            ReportOptions(options);
            var scene = worker.Scene;

            ReportPainter.Paint(ctx => worker.Painter.Paint(ctx));

            ILocator<IVisual> locator = new GraphSceneItemShapeLocator<IVisual, IVisualEdge> { GraphScene = scene };

            Action<IEnumerable<IVisual>> reportElems = elms => ReportElems(scene, elms, locator, options);
            Func<IEnumerable<IVisual>, Rectangle> reportExtent = elms => ReportExtent(elms, locator, options);

            var visibleItems = new HashSet<IVisual>(scene.Elements.Where(e => !(e is IVisualEdge)));
            var visibleBounds = locator.Bounds(visibleItems);
            reportElems(visibleItems);
            reportExtent(visibleItems);


            IEnumerable<IVisual> itemsToPlace = Walker.Create((scene.Graph as GraphView<IVisual, IVisualEdge>).Two)
                .DeepWalk(scene.Focused, 0)
                .Select(l => l.Node)
                .ToArray();
            scene.Focused.Location = Point.Zero + worker.Layout.Distance;
            itemsToPlace.ForEach(e => scene.Graph.Add(e));

            itemsToPlace = itemsToPlace.Where(e => !(e is IVisualEdge));
            var aligner = new Aligner<IVisual, IVisualEdge>(scene, worker.Layout);
            options.Dimension = options.Dimension==Dimension.X?Dimension.Y:Dimension.X;
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

            ReportPainter.Paint(ctx => worker.Painter.Paint(ctx));

            ReportPainter.Paint(ctx => {
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
    }
}