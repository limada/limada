/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with RectangleF and PointF
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://www.limada.org
 * 
 */

/* NetTopologySuite is a collection of .NET classes written in C# that
implement the fundamental operations required to validate a given
geo-spatial data set to a known topological specification.

This collection of classes is a porting (with some additions and modifications) of 
JTS Topology Suite (see next license for more informations).

Copyright (C) 2005 Diego Guidi

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

For more information, contact:

    Diego Guidi
    via Po 15
	61031 Cuccurano di Fano (PU)
    diegoguidi@libero.it
    http://blogs.ugidotnet.org/gissharpblog

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
