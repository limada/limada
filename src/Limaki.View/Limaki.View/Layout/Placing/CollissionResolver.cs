using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using System;
using Xwt;

namespace Limaki.View.Layout {

    public interface ILocationDetector<TItem> {
        Point NextFreePosition (Point start, Size sizeNeeded, Dimension dimension, IEnumerable<TItem> ignore);
    }

    public class GraphSceneLocationDetector<TItem, TEdge> : ILocationDetector<TItem> where TEdge : IEdge<TItem>, TItem {

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

        public Point NextFreePosition(Point start, Size sizeNeeded, Dimension dimension, IEnumerable<TItem> ignore) {
            var rect = new Rectangle (start, sizeNeeded);
            var loc = new SGraphSceneLocator { GraphScene = this.GraphScene };
            var measure = new MeasureVisits<TItem> (loc);
            Action<TItem> visit = null;
            
            IEnumerable<TItem> elems = null;
            do {
                elems = GraphScene.ElementsIn(rect).Except(ignore);
                var frect = measure.Bounds (ref visit);
                foreach (var item in elems)
                    visit (item);
                rect = frect ();
                if (!rect.IsEmpty) {
                    rect = new Rectangle (new Point (rect.Right, rect.Top), sizeNeeded);
                }

            } while (!rect.IsEmpty);


            return rect.Location;
        }

    }

    public class CollissionResolver<TItem>:LocateVisits<TItem> {
        public CollissionResolver (ILocator<TItem> locator, ILocationDetector<TItem> detector):base(locator) {
            this.Locator = locator;
            this.Detector = detector;
        }

        public virtual ILocationDetector<TItem> Detector { get; protected set; }

        public virtual void Locate(ref Action<TItem> visitor, Func<Size, double> Xer, Func<Size, double> Yer, Dimension dimension, IEnumerable<TItem> ignore) {
            visitor += item => {
                var size = Locator.GetSize(item);
                var location = new Point(Xer(size), Yer(size));

                location = Detector.NextFreePosition(location, size, dimension, ignore);
                Locator.SetLocation(item, location);
            };
        }
    }
}