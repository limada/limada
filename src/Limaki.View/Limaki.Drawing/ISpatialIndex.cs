using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public interface ISpatialIndex<TItem> {

        bool BoundsDirty { get; set; }
        Rectangle Bounds { get; set; }

        void Add(TItem item);
        void Remove(TItem item);
        void AddRange(IEnumerable<TItem> items);
        void Update(Rectangle invalid, TItem item);

        IEnumerable<TItem> Query();
        IEnumerable<TItem> Query(Rectangle clipBounds);
        
        void Clear();

    }

  
}