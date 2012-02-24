using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public interface ISpatialIndex<TItem> {

        bool BoundsDirty { get; set; }
        RectangleD Bounds { get; set; }

        void Add(TItem item);
        void Remove(TItem item);
        void AddRange(IEnumerable<TItem> items);
        void Update(RectangleD invalid, TItem visual);

        IEnumerable<TItem> Query();
        IEnumerable<TItem> Query(RectangleD clipBounds);
        IEnumerable<TItem> Query(RectangleD clipBounds, ZOrder zOrder);
        void Clear();

    }
}