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

using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using System.Linq;
using System;
using Limaki.View;
using Limaki.View.Viz.Modelling;
using Xwt;

namespace Limaki.Playground.View {

    public interface ILocationDetector<TItem> {
        Point NextFreePosition (Point start, Size sizeNeeded, IEnumerable<TItem> ignore, Dimension dimension, double distance);
        /// <summary>
        /// sets items location and size to be aware in NextFreePosition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="location"></param>
        void SetLocation (TItem item, Point location, Size size);
    }

    public class GraphSceneLocationDetector<TItem, TEdge> : ILocationDetector<TItem> where TEdge : IEdge<TItem>, TItem {

        public GraphSceneLocationDetector (IGraphScene<TItem, TEdge> scene) {
            this.GraphScene = scene;
        }
        public IGraphScene<TItem, TEdge> GraphScene { get; protected set; }
        
        
        public Point NextFreePosition0 (Point start, Size sizeNeeded, Dimension dimension, IEnumerable<TItem> ignore, double distance) {
            var result = new Rectangle (start, sizeNeeded);
            var loc = new GraphSceneItemShapeLocator<TItem, TEdge> { GraphScene = this.GraphScene };
            var measure = new MeasureVisitBuilder<TItem> (loc);
            
            var iRect = result;

            while (!iRect.IsEmpty) {
                var elems = GraphScene.ElementsIn (iRect).Where (e => !(e is TEdge)).Except (ignore);
                if (!elems.Any())
                    break;
                Action<TItem> visit = null;
                var frect = measure.Bounds (ref visit);
                foreach (var item in elems)
                    visit (item);
                iRect = frect ();
                if (!iRect.IsEmpty) {
                    iRect = new Rectangle (new Point (iRect.Right + distance, result.Top), sizeNeeded);
                    result = iRect;
                }
            } 


            return result.Location;
        }

        IList<Rectangle> FreeSpace = null;

        IList<Rectangle> CalculateFreeSpace(Point start, Size sizeNeeded, Dimension dimension, IEnumerable<TItem> ignore, double distance) {
            var h = dimension==Dimension.X ? sizeNeeded.Height : GraphScene.Shape.Size.Height;
            var w = dimension==Dimension.X ? GraphScene.Shape.Size.Width : sizeNeeded.Width;
            var iRect = new Rectangle(start, new Size(w, h));

            var comparer = new PointComparer { Order = dimension == Dimension.X ? PointOrder.X : PointOrder.Y };
            var loc = new GraphSceneItemShapeLocator<TItem,TEdge> { GraphScene = this.GraphScene };
            var elems = GraphScene.ElementsIn(iRect)
                .Where(e => !(e is TEdge)).Except(ignore).OrderBy(e=>loc.GetLocation(e),comparer);

            return new Rectangle[0];
        }

        public Point NextFreePosition(Point start, Size sizeNeeded, IEnumerable<TItem> ignore, Dimension dimension, double distance) {
            var result = start;
           
           var freeSpace = CalculateFreeSpace(start, sizeNeeded, dimension, ignore, distance);
           
           


            return result;
        }

        public void SetLocation (TItem item, Point location, Size size) {

        }

    }

    public class CollissionResolver<TItem> : LocateVisitBuilder<TItem> {
        public CollissionResolver (ILocator<TItem> locator, ILocationDetector<TItem> detector, IEnumerable<TItem> ignore, Dimension dimension, double distance)
            : base (locator) {
            this.Locator = locator;
            this.Detector = detector;
            this.ignore = new HashSet<TItem>(ignore);
            this.dimension = dimension;
            this.distance = distance;
        }

        private ISet<TItem> ignore;
        private Dimension dimension;
        private double distance;
        public virtual ILocationDetector<TItem> Detector { get; protected set; }

        public override void Locate (ref Action<TItem> visit, Func<Size, double> Xer, Func<Size, double> Yer) {
            visit += item => {
                var size = Locator.GetSize (item);
                var location = new Point (Xer (size), Yer (size));

                location = Detector.NextFreePosition (location, size, ignore, dimension, distance);
                Locator.SetLocation (item, location);
                //!!: ignore.Remove (item);
                Detector.SetLocation (item, location,size);
            };
        }
    }
}