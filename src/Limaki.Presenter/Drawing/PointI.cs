/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Authors: 
 * Mike Kestner (mkestner@speakeasy.net)
 * 
 * Copyright (C) 2004 Novell, Inc (http://www.novell.com)
 *
 * http://limada.sourceforge.net
 * 
 */

/* 
 * this file is ported from
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\Point.cs
 */


using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Limaki.Drawing {
    [ComVisible(true)]
    public struct PointI {
        // Private x and y coordinate fields.
        private int x, y;

        // -----------------------
        // Public Shared Members
        // -----------------------

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized Point Structure.
        /// </remarks>

        public static readonly PointI Empty;

        /// <summary>
        ///	Ceiling Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Point structure from a PointF structure by
        ///	taking the ceiling of the X and Y properties.
        /// </remarks>

        public static PointI Ceiling(PointS value) {
            int x, y;
            checked {
                x = (int)Math.Ceiling(value.X);
                y = (int)Math.Ceiling(value.Y);
            }

            return new PointI(x, y);
        }

        /// <summary>
        ///	Round Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Point structure from a PointF structure by
        ///	rounding the X and Y properties.
        /// </remarks>

        public static PointI Round(PointS value) {
            int x, y;
            checked {
                x = (int)Math.Round(value.X);
                y = (int)Math.Round(value.Y);
            }

            return new PointI(x, y);
        }

        /// <summary>
        ///	Truncate Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Point structure from a PointF structure by
        ///	truncating the X and Y properties.
        /// </remarks>

        // LAMESPEC: Should this be floor, or a pure cast to int?

        public static PointI Truncate(PointS value) {
            int x, y;
            checked {
                x = (int)value.X;
                y = (int)value.Y;
            }

            return new PointI(x, y);
        }

        /// <summary>
        ///	Addition Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Translates a Point using the Width and Height
        ///	properties of the given <typeref>Size</typeref>.
        /// </remarks>

        public static PointI operator +(PointI pt, SizeI sz) {
            return new PointI(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Point objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </remarks>

        public static bool operator ==(PointI left, PointI right) {
            return ((left.X == right.X) && (left.Y == right.Y));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Point objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </remarks>

        public static bool operator !=(PointI left, PointI right) {
            return ((left.X != right.X) || (left.Y != right.Y));
        }

        /// <summary>
        ///	Subtraction Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Translates a Point using the negation of the Width 
        ///	and Height properties of the given Size.
        /// </remarks>

        public static PointI operator -(PointI pt, SizeI sz) {
            return new PointI(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        ///	Point to Size Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Returns a Size based on the Coordinates of a given 
        ///	Point. Requires explicit cast.
        /// </remarks>

        public static explicit operator SizeI(PointI p) {
            return new SizeI(p.X, p.Y);
        }

        /// <summary>
        ///	Point to PointF Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a PointF based on the coordinates of a given 
        ///	Point. No explicit cast is required.
        /// </remarks>

        public static implicit operator PointS(PointI p) {
            return new PointS(p.X, p.Y);
        }


        // -----------------------
        // Public Constructors
        // -----------------------

        /// <summary>
        ///	Point Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Point from an integer which holds the X
        ///	coordinate in the high order 16 bits and the Y
        ///	coordinate in the low order 16 bits.
        /// </remarks>

        public PointI(int dw) {
            x = dw >> 16;
            y = dw & 0xffff;
        }

        /// <summary>
        ///	Point Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Point from a Size value.
        /// </remarks>

        public PointI(SizeI sz) {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        ///	Point Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Point from a specified x,y coordinate pair.
        /// </remarks>

        public PointI(int x, int y) {
            this.x = x;
            this.y = y;
        }

        // -----------------------
        // Public Instance Members
        // -----------------------

        /// <summary>
        ///	IsEmpty Property
        /// </summary>
        ///
        /// <remarks>
        ///	Indicates if both X and Y are zero.
        /// </remarks>


        public bool IsEmpty {
            get {
                return ((x == 0) && (y == 0));
            }
        }

        /// <summary>
        ///	X Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the Point.
        /// </remarks>

        public int X {
            get {
                return x;
            }
            set {
                x = value;
            }
        }

        /// <summary>
        ///	Y Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Y coordinate of the Point.
        /// </remarks>

        public int Y {
            get {
                return y;
            }
            set {
                y = value;
            }
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this Point and another object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is PointI))
                return false;

            return (this == (PointI)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return x ^ y;
        }

        /// <summary>
        ///	Offset Method
        /// </summary>
        ///
        /// <remarks>
        ///	Moves the Point a specified distance.
        /// </remarks>

        public void Offset(int dx, int dy) {
            x += dx;
            y += dy;
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the Point as a string in coordinate notation.
        /// </remarks>

        public override string ToString() {
            return string.Format("{{X={0},Y={1}}}", x.ToString(CultureInfo.InvariantCulture),
                                 y.ToString(CultureInfo.InvariantCulture));
        }

        public static PointI Add(PointI pt, SizeI sz) {
            return new PointI(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public void Offset(PointI p) {
            Offset(p.X, p.Y);
        }

        public static PointI Subtract(PointI pt, SizeI sz) {
            return new PointI(pt.X - sz.Width, pt.Y - sz.Height);
        }


    }
}

//
// System.Drawing.Point.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004 Novell, Inc.  http://www.novell.com 
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//