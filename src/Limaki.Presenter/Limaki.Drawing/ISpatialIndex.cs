using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public interface ISpatialIndex<TItem> {

        bool BoundsDirty { get; set; }
        Rectangle Bounds { get; set; }

        void Add(TItem item);
        void Remove(TItem item);
        void AddRange(IEnumerable<TItem> items);
        void Update(Rectangle invalid, TItem visual);

        IEnumerable<TItem> Query();
        IEnumerable<TItem> Query(Rectangle clipBounds);
        IEnumerable<TItem> Query(Rectangle clipBounds, ZOrder zOrder);
        void Clear();

    }
}