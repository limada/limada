/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

/* 
 * this file is ported from
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\PointF.cs
 */



using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Limaki.Drawing {
    [ComVisible(true)]
    public struct PointS {
        // Private x and y coordinate fields.
        private float x, y;

        // -----------------------
        // Public Shared Members
        // -----------------------

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized PointF Structure.
        /// </remarks>

        public static readonly PointS Empty;

        /// <summary>
        ///	Addition Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Translates a PointF using the Width and Height
        ///	properties of the given Size.
        /// </remarks>

        public static PointS operator +(PointS pt, SizeI sz) {
            return new PointS(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static PointS operator +(PointS pt, SizeS sz) {
            return new PointS(pt.X + sz.Width, pt.Y + sz.Height);
        }


        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two PointF objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </remarks>

        public static bool operator ==(PointS left, PointS right) {
            return ((left.X == right.X) && (left.Y == right.Y));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two PointF objects. The return value is
        ///	based on the equivalence of the X and Y properties 
        ///	of the two points.
        /// </remarks>

        public static bool operator !=(PointS left, PointS right) {
            return ((left.X != right.X) || (left.Y != right.Y));
        }

        /// <summary>
        ///	Subtraction Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Translates a PointF using the negation of the Width 
        ///	and Height properties of the given Size.
        /// </remarks>

        public static PointS operator -(PointS pt, SizeI sz) {
            return new PointS(pt.X - sz.Width, pt.Y - sz.Height);
        }

        public static PointS operator -(PointS pt, SizeS sz) {
            return new PointS(pt.X - sz.Width, pt.Y - sz.Height);
        }


        // -----------------------
        // Public Constructor
        // -----------------------

        /// <summary>
        ///	PointF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a PointF from a specified x,y coordinate pair.
        /// </remarks>

        public PointS(float x, float y) {
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
                return ((x == 0.0) && (y == 0.0));
            }
        }

        /// <summary>
        ///	X Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the PointF.
        /// </remarks>

        public float X {
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
        ///	The Y coordinate of the PointF.
        /// </remarks>

        public float Y {
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
        ///	Checks equivalence of this PointF and another object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is PointS))
                return false;

            return (this == (PointS)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return (int)x ^ (int)y;
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the PointF as a string in coordinate notation.
        /// </remarks>

        public override string ToString() {
            return String.Format("{{X={0}, Y={1}}}", x.ToString(CultureInfo.CurrentCulture),
                                 y.ToString(CultureInfo.CurrentCulture));
        }


        public static PointS Add(PointS pt, SizeI sz) {
            return new PointS(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static PointS Add(PointS pt, SizeS sz) {
            return new PointS(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static PointS Subtract(PointS pt, SizeI sz) {
            return new PointS(pt.X - sz.Width, pt.Y - sz.Height);
        }

        public static PointS Subtract(PointS pt, SizeS sz) {
            return new PointS(pt.X - sz.Width, pt.Y - sz.Height);
        }


    }
}

//
// System.Drawing.PointF.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004,2006 Novell, Inc (http://www.novell.com)
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