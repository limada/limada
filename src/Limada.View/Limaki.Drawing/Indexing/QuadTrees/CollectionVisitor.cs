/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with Xwt.Rectangle and Xwt.Point
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// 
    /// </summary>
    public class CollectionVisitor<TItem> : IItemVisitor<TItem> {
        private ICollection<TItem> _items = new List<TItem>();

        /// <summary>
        /// 
        /// </summary>
        public CollectionVisitor() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void VisitItem(TItem item) { _items.Add(item); }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<TItem> Items { get { return _items; } }

        public virtual Rectangle GetEnvelope(TItem item) { return default( Rectangle ); }

        public virtual bool ProvidesEnvelope { get { return false; } }

    }
}
