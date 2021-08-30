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

using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// A visitor for items in an index.
    /// </summary>
    public interface IItemVisitor<TItem> {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void VisitItem(TItem item);

        Rectangle GetEnvelope ( TItem item );
        bool ProvidesEnvelope { get; }
    }
}
