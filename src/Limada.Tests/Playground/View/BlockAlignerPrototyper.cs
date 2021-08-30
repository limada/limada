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
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Playground.View {


    public class BlockAlignerPrototyper : AlignerPrototyperBase {


        protected IEnumerable<Rectangle> TrackBounds = null;

        public virtual Rectangle AlignByPath<TItem, TEdge> (IEnumerable<LevelItem<TItem>> walk, AlignerOptions options, Aligner<TItem, TEdge> aligner, Point startPoint)
        where TEdge : IEdge<TItem>, TItem {

            var bounds = new Rectangle (startPoint, Size.Zero);

            // contains levelitems with backtracked path
            var trackedSteps = new Dictionary<TItem, LevelItem<TItem>> ();

            // used to backtrack path
            var steps = new Dictionary<TItem, LevelItem<TItem>> ();

            var trackBounds = new Dictionary<TItem, Rectangle> ();
            var trackBoundsToken = new HashSet<TItem> ();

            Func<TItem, TEdge> asEdge = node => node is TEdge ? (TEdge)node : default (TEdge);

            Func<TItem, bool> isVisibleNode = node => !(node is TEdge);

            Func<LevelItem<TItem>, TItem> findPath = step => {

                var visibleNode = isVisibleNode (step.Node);

                while (step.Path != null) {

                    if (isVisibleNode (step.Path))
                        return step.Path;
                    
                    if ((step.Node as IEdge<TItem>).IsEdgeOfEdges ())
                        return step.Path;

                    if ((step.Path as IEdge<TItem>).IsEdgeOfEdges ())
                        return step.Path;
                    

                    if (step.StepIsEdgeToEdge () && visibleNode)
                        return step.Node;
                    
                    if (trackBounds.ContainsKey (step.Path))
                        return step.Path;
                    
                    step = steps [step.Path];

                }

                TrackBounds = trackBounds.Values;
                return step.Path;
            };

            // steps up the paths and sums the size of trackBounds
            Action<LevelItem<TItem>, Rectangle> sumTrackBounds = (trackedStep, trackBound) => {
                var adjDimension = options.Dimension.Adjacent ();
                while (trackedStep.Path != null) {
                    var trackStep = trackedSteps [trackedStep.Path];

                    if (trackStep.Path != null) {
                        var backTrackBound = trackBounds [trackStep.Path].MaxExtend (trackBound, adjDimension);
                        trackBounds [trackStep.Path] = backTrackBound;
                        trackBound = backTrackBound;
                    }
                    trackedStep = trackStep;
                }

            };

            // gets the trackBounds of the trackedStep.Path; ensures that the path has trackBounds
            Func<LevelItem<TItem>, Rectangle, Rectangle> getTrackBounds = (trackedStep, trackBound) => {

                while (trackedStep.Path != null) {
                    var gb = new Rectangle ();
                    if (trackBounds.TryGetValue (trackedStep.Path, out gb)) {
                        return gb;
                    } else {
                        // TODO: ensure that all paths have trackBounds, not only the first uppath
                        var trackStep = trackedSteps [trackedStep.Path];
                        if (trackStep.Path != null) {
                            var backTrackBound = trackBounds [trackStep.Path];
                            trackBound.X = backTrackBound.Right;
                            trackBound.Y = backTrackBound.Bottom;
                        }
                        trackBounds [trackedStep.Path] = trackBound;
                        break;
                    }

                }
                return trackBound;
            };


            foreach (var step in walk) {

                steps.Add (step.Node, step);

                var visibleNode = isVisibleNode (step.Node);
                var nonAdjacent = !step.StepIsAdjacent ();

                var isStepEdgeToEdge = step.StepIsEdgeToEdge ();
                var isNodeEdgeOfEdges = (step.Node as IEdge<TItem>).IsEdgeOfEdges ();

                var trackedStep = new LevelItem<TItem> (step.Node, findPath (step), step.Level);
                trackedSteps [step.Node] = trackedStep;
                ReportDetail ($"\t{trackedStep}");

                if (!(visibleNode || isStepEdgeToEdge))
                    continue;

                var gb = new Rectangle (startPoint, options.Distance.InitSum (options.Dimension));

                var trackBound = new Rectangle(startPoint, new Size());
                if (trackedStep.Path != null) {
                    trackBound = getTrackBounds (trackedStep, gb);
                    gb.X = trackBound.Right;
                    gb.Y = trackBound.Bottom;
                }

                if (isStepEdgeToEdge) {
                    trackBounds [step.Node] = new Rectangle (gb.Location, new Size());
                    ReportDetail ($"{step.Node}\t{trackBounds [step.Node]}\t{trackedStep.Path}\t{trackBound}");
                }

                if (visibleNode) {

                    var loc = gb.Location;
                    var nodeBounds = aligner.MeasureItem (step.Node, options.Dimension, options.Distance, ref gb);
                    nodeBounds.Location = gb.Location.Max (loc);

                    aligner.Locator.SetLocation (step.Node, nodeBounds.Location);

                    trackBounds [step.Node] = new Rectangle (nodeBounds.Location, gb.Size.InitSum (options.Dimension));

                    if (trackedStep.Path != null) {
                        trackBound.Size = trackBound.Size.SumSize (nodeBounds.Size + options.Distance, options.Dimension.Adjacent ());
                        trackBounds [trackedStep.Path] = trackBound;
                        trackBoundsToken.Add (trackedStep.Path);
                    }

                    ReportDetail ($"{step.Node}\t{nodeBounds}\t{trackedStep.Path}\t{trackBound}");
                }

                sumTrackBounds (trackedStep, trackBound);

            }

            // aligns

            foreach (var trackBound in trackBounds.ToArray()) {
                if (trackBound.Value.Width==0) {
                    trackBounds [trackBound.Key] = new Rectangle (trackBound.Value.Location, new Size (options.Distance.Width/2,trackBound.Value.Height));
                }
            }
            foreach (var kvp in trackBounds.ToArray ()) {
                var step = trackedSteps [kvp.Key];
                if (step.Path == null)
                    continue;
                var lbounds = kvp.Value;
                var sum = 0d;
                while (step.Path != null) {
                    var trackBound = trackBounds [step.Path];
                    sum = Math.Max (trackBound.Right, sum);
                    step = trackedSteps [step.Path];
                }
                if(lbounds.X!=sum)
                    trackBounds [kvp.Key] = new Rectangle (new Point (sum, lbounds.Y), lbounds.Size);
            }

            Action<TItem> visitMeasure = null;
            var measure = new MeasureVisitBuilder<TItem> (aligner.Locator);
            var fBounds = measure.Bounds (ref visitMeasure);

            foreach (var step in trackedSteps.Values.Where (l => !(l.Node is TEdge))) {

                var nodeBounds = trackBounds [step.Node];
                var loc = aligner.Locator.GetLocation (step.Node);
                var size = aligner.Locator.GetSize (step.Node);

                if (options.Dimension == Dimension.X) {
                    if (nodeBounds.Height != 0) {
                        loc.Y += options.AlignY.Delta (nodeBounds.Height - options.Distance.Height, size.Height); ;
                    }
                    loc.X = nodeBounds.X;
                } else {
                    if (nodeBounds.Width != 0)
                        loc.X += options.AlignX.Delta (nodeBounds.Width - options.Distance.Width, size.Width);
                    loc.Y = nodeBounds.Y;
                }


                aligner.Locator.SetLocation (step.Node, loc);

                visitMeasure (step.Node);

            }

            bounds = fBounds ();
            return bounds;
        }

        [Test]
        public void BlockAlgin () {

            var options = new AlignerOptions {
                AlignX = Alignment.Start,
                AlignY = Alignment.Start,
                Dimension = Dimension.X,
                PointOrder = PointOrder.XY,
                Collisions = Collisions.None,
            };

            var scene = SceneWithTestData (3, 1);

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge> ();
            worker.Compose (scene, new VisualsRenderer ());
            worker.StyleSheet.BackColor = Colors.WhiteSmoke;
            worker.Layout.SetOptions (options);

            worker.Folder.AddRaw (scene.Graph.RootSource ()?.Source);
            worker.Modeller.Perform ();
            worker.Modeller.Finish ();

            var origins = scene.Graph.FindRoots (null);
            scene.Focused = origins.First ();

            options.Distance = worker.Layout.Distance;
            var visualComparer = new VisualComparer ();
            var aligner = new Aligner<IVisual, IVisualEdge> (worker.Scene, worker.Layout);

            var graphView = worker.Scene.Graph as SubGraph<IVisual, IVisualEdge>;
            graphView.Edges ().ForEach (e => e.Data = "");

            // var roots = new IVisual [] { worker.Scene.Focused };

            var walk = new Walker1<IVisual, IVisualEdge> (graphView);
            //walk.Trace = true;
            walk.Comparer = visualComparer;

            var startPoint = (Point)worker.Layout.Border;

            foreach (var origin in origins) {
                ReportDetail ($"-> {origin}\t{startPoint}");
                var bounds = AlignByPath (walk.DeepWalk (origin, 1, null, false), options, aligner, startPoint);
                startPoint.Y += bounds.Height + options.Distance.Height;

            }

            aligner.Commit ();
            worker.Modeller.Perform ();
            worker.Modeller.Finish ();

            ReportPainter.PushPaint (ctx => worker.Painter.Paint (ctx));
            foreach (var r in TrackBounds.Where (r => !r.IsEmpty))
                ReportPainter.PushPaint (ctx => {

                    var translate = worker.Painter.Viewport.ClipOrigin;
                    ctx.Save ();
                    ctx.Translate (-translate.X, -translate.Y);

                    ctx.SetColor (Colors.Red);
                    ctx.SetLineWidth (0.5);
                    ctx.SetLineDash (0, 2, 5);
                    ctx.Rectangle (r);
                    ctx.ClosePath ();
                    ctx.Stroke ();
                    ctx.Restore ();
                });
            WritePainter ();

        }
    }
}