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
 * http://limada.sourceforge.net
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

using System;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary> 
    /// A Key is a unique identifier for a node in a quadtree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key {
        // the fields which make up the key
        private PointS _pt = new PointS();
        public virtual PointS Point {
            get { return _pt; }
        }

        private int _level = 0;
        public virtual int Level {
            get { return _level; }
        }

        // auxiliary data which is derived from the key for use in computation
        private RectangleS _env = default(RectangleS);
        public virtual RectangleS Envelope {
            get { return _env; }
        }


        public Key ( RectangleS itemEnv ) {
            ComputeKey(itemEnv);
        }

        public virtual PointS Centre {
            get {
                return new PointS(
                    ( _env.X + _env.X + _env.Width ) / 2,
                    ( _env.Y + _env.Y + _env.Height ) / 2);
            }
        }

        public static int ComputeQuadLevel ( RectangleS env ) {
            double dx = env.Width;
            double dy = env.Height;
            double dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public virtual void ComputeKey ( RectangleS itemEnv ) {
            _level = ComputeQuadLevel(itemEnv);
            _env = new RectangleS();
            ComputeKey(_level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while ( !ShapeUtils.Contains(_env, itemEnv) ) {
                _level += 1;
                ComputeKey(_level, itemEnv);
            }
        }

        private void ComputeKey ( int level, RectangleS itemEnv ) {
            double quadSize = DoubleBits.PowerOf2(level);
            _pt.X = (float) ( Math.Floor(itemEnv.X / quadSize) * quadSize );
            _pt.Y = (float) ( Math.Floor(itemEnv.Y / quadSize) * quadSize );
            _env = new RectangleS(_pt.X, _pt.Y, (float) quadSize, (float) quadSize);
        }
    }
}
