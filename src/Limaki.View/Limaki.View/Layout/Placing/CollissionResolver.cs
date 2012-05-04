using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using System;
using Xwt;

namespace Limaki.View.Layout {

    public interface ILocationDetector {
        double NextFreePosition (Point start, Size sizeNeeded, Dimension dimension);
    }

    public class GraphSceneLocationDetector<TItem, TEdge> : ILocationDetector where TEdge : IEdge<TItem>, TItem {

        public GraphSceneLocationDetector (IGraphScene<TItem, TEdge> scene) {
            this.GraphScene = scene;
        }
        public IGraphScene<TItem, TEdge> GraphScene { get; protected set; }
        public class SGraphSceneLocator : ILocator<TItem> {
            public IGraphScene<TItem, TEdge> GraphScene { get; set; }

            public Point GetLocation (TItem item) {
                return GraphScene.ItemShape (item).Location;
            }

            public void SetLocation (TItem item, Point location) {
                throw new NotImplementedException ();
            }

            public Size GetSize (TItem item) {
                return GraphScene.ItemShape (item).Size;
            }

            public void SetSize (TItem item, Size value) {
                throw new NotImplementedException ();
            }
        }

        public double NextFreePosition (Point start, Size sizeNeeded, Dimension dimension) {
            var rect = new Rectangle (start, sizeNeeded);
            var loc = new SGraphSceneLocator { GraphScene = this.GraphScene };
            var measure = new MeasureVisits<TItem> (loc);
            Action<TItem> visit = null;
            
            IEnumerable<TItem> elems = null;
            do {
                elems = GraphScene.ElementsIn (rect);
                var frect = measure.Bounds (ref visit);
                foreach (var item in elems)
                    visit (item);
                rect = frect ();
                if (!rect.IsEmpty) {
                    rect = new Rectangle (new Point (rect.Right, rect.Top), sizeNeeded);
                }

            } while (!rect.IsEmpty);


            return rect.X;
        }

    }

    public class CollissionResolver<TItem> {
        public CollissionResolver (ILocator<TItem> locator, ILocationDetector detector) {
            this.Locator = locator;
            this.Detector = detector;
        }

        public virtual ILocator<TItem> Locator { get; protected set; }
        public virtual ILocationDetector Detector { get; protected set; }
    }
}