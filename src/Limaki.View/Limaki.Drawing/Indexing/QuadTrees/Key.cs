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
using System;
using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary> 
    /// A Key is a unique identifier for a node in a quadtree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key {
        // the fields which make up the key
        private Point _pt = new Point();
        public virtual Point Point {
            get { return _pt; }
        }

        private int _level = 0;
        public virtual int Level {
            get { return _level; }
        }

        // auxiliary data which is derived from the key for use in computation
        private Rectangle _env = default(Rectangle);
        public virtual Rectangle Envelope {
            get { return _env; }
        }


        public Key ( Rectangle itemEnv ) {
            ComputeKey(itemEnv);
        }

        public virtual Point Centre {
            get {
                return new Point(
                    ( _env.X + _env.X + _env.Width ) / 2,
                    ( _env.Y + _env.Y + _env.Height ) / 2);
            }
        }

        public static int ComputeQuadLevel ( Rectangle env ) {
            var dx = env.Width;
            var dy = env.Height;
            var dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public virtual void ComputeKey ( Rectangle itemEnv ) {
            _level = ComputeQuadLevel(itemEnv);
            _env = new Rectangle();
            ComputeKey(_level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!DrawingExtensions.Contains(_env, itemEnv)) {
                _level += 1;
                ComputeKey(_level, itemEnv);
            }
        }

        private void ComputeKey ( int level, Rectangle itemEnv ) {
            double quadSize = DoubleBits.PowerOf2(level);
            _pt.X = ( Math.Floor(itemEnv.X / quadSize) * quadSize );
            _pt.Y = ( Math.Floor(itemEnv.Y / quadSize) * quadSize );
            _env = new Rectangle(_pt.X, _pt.Y, quadSize, quadSize);
        }
    }
}
