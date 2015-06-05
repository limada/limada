using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Xwt;
using System.Linq;

namespace Limaki.Drawing.Indexing {

    public class SpatialQuadTreeIndex<TItem> : SpatialIndex<TItem> {

        private Quadtree<TItem> _geoIndex = null;
        public Quadtree<TItem> GeoIndex {
            get {
                if (_geoIndex == null) {
                    _geoIndex = new Quadtree<TItem>();
                }
                return _geoIndex;
            }
            set { _geoIndex = value; }
        }

        protected override void Add (Rectangle bounds, TItem item) {
            if (bounds != Rectangle.Zero)
                GeoIndex.Add(bounds, item);
        }

        protected override void Remove (Rectangle bounds, TItem item) {
            if (bounds != Rectangle.Zero)
                GeoIndex.Remove(bounds, item);
        }

        public override IEnumerable<TItem> Query (Rectangle clipBounds) {
            return GeoIndex.Query(clipBounds).Where(item=>DrawingExtensions.Intersects(clipBounds, BoundsOf(item)));
        }

        public override IEnumerable<TItem> Query () {
            return GeoIndex.QueryAll();
        }

        protected override Rectangle CalculateBounds () {
            // remark: the starting values could be used to 
            // opimize further if boundsDirty is more refined;
            // eg: leftDirty leads to : 
            // l = float.MaxValue, t = float.MinValue, r = float.maxvalue, b = float.maxValue

            var l = double.MaxValue;
            var t = double.MaxValue;
            var r = double.MinValue;
            var b = double.MinValue;

            GeoIndex.QueryBounds(
                ref l, ref t, ref r, ref b,
                GeoIndex.Root,
                BoundsOf);

            if (l > 0)
                l = 0;
            if (t > 0)
                t = 0;
            if (r < 0)
                r = 0;
            if (b < 0)
                b = 0;
            return Rectangle.FromLTRB((int) l, (int) t, (int) r, (int) b);
        }

        public override void Clear () {
            BoundsDirty = true;
            Bounds = Rectangle.Zero;
            GeoIndex = null;
        }
    }
}