/*
 * Limaki 
 * Version 0.081
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
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\Size.cs
 */



using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Limaki.Drawing;

namespace Limaki.Drawing {
    [ComVisible(true)]
    public struct SizeI {

        // Private Height and width fields.
        private int width, height;

        // -----------------------
        // Public Shared Members
        // -----------------------

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized Size Structure.
        /// </remarks>

        public static readonly SizeI Empty;

        /// <summary>
        ///	Ceiling Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Size structure from a SizeF structure by
        ///	taking the ceiling of the Width and Height properties.
        /// </remarks>

        public static SizeI Ceiling(SizeS value) {
            int w, h;
            checked {
                w = (int)Math.Ceiling(value.Width);
                h = (int)Math.Ceiling(value.Height);
            }

            return new SizeI(w, h);
        }

        /// <summary>
        ///	Round Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Size structure from a SizeF structure by
        ///	rounding the Width and Height properties.
        /// </remarks>

        public static SizeI Round(SizeS value) {
            int w, h;
            checked {
                w = (int)Math.Round(value.Width);
                h = (int)Math.Round(value.Height);
            }

            return new SizeI(w, h);
        }

        /// <summary>
        ///	Truncate Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Size structure from a SizeF structure by
        ///	truncating the Width and Height properties.
        /// </remarks>

        public static SizeI Truncate(SizeS value) {
            int w, h;
            checked {
                w = (int)value.Width;
                h = (int)value.Height;
            }

            return new SizeI(w, h);
        }

        /// <summary>
        ///	Addition Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Addition of two Size structures.
        /// </remarks>

        public static SizeI operator +(SizeI sz1, SizeI sz2) {
            return new SizeI(sz1.Width + sz2.Width,
                             sz1.Height + sz2.Height);
        }

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Size objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator ==(SizeI sz1, SizeI sz2) {
            return ((sz1.Width == sz2.Width) &&
                    (sz1.Height == sz2.Height));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Size objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator !=(SizeI sz1, SizeI sz2) {
            return ((sz1.Width != sz2.Width) ||
                    (sz1.Height != sz2.Height));
        }

        /// <summary>
        ///	Subtraction Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Subtracts two Size structures.
        /// </remarks>

        public static SizeI operator -(SizeI sz1, SizeI sz2) {
            return new SizeI(sz1.Width - sz2.Width,
                             sz1.Height - sz2.Height);
        }

        /// <summary>
        ///	Size to Point Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Returns a Point based on the dimensions of a given 
        ///	Size. Requires explicit cast.
        /// </remarks>

        public static explicit operator PointI(SizeI size) {
            return new PointI(size.Width, size.Height);
        }

        /// <summary>
        ///	Size to SizeF Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a SizeF based on the dimensions of a given 
        ///	Size. No explicit cast is required.
        /// </remarks>

        public static implicit operator SizeS(SizeI p) {
            return new SizeS(p.Width, p.Height);
        }


        // -----------------------
        // Public Constructors
        // -----------------------

        /// <summary>
        ///	Size Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Size from a Point value.
        /// </remarks>

        public SizeI(PointI pt) {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        ///	Size Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Size from specified dimensions.
        /// </remarks>

        public SizeI(int width, int height) {
            this.width = width;
            this.height = height;
        }

        // -----------------------
        // Public Instance Members
        // -----------------------

        /// <summary>
        ///	IsEmpty Property
        /// </summary>
        ///
        /// <remarks>
        ///	Indicates if both Width and Height are zero.
        /// </remarks>


        public bool IsEmpty {
            get {
                return ((width == 0) && (height == 0));
            }
        }

        /// <summary>
        ///	Width Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Width coordinate of the Size.
        /// </remarks>

        public int Width {
            get {
                return width;
            }
            set {
                width = value;
            }
        }

        /// <summary>
        ///	Height Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Height coordinate of the Size.
        /// </remarks>

        public int Height {
            get {
                return height;
            }
            set {
                height = value;
            }
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this Size and another object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is SizeI))
                return false;

            return (this == (SizeI)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return width ^ height;
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the Size as a string in coordinate notation.
        /// </remarks>

        public override string ToString() {
            return String.Format("{{Width={0}, Height={1}}}", width, height);
        }


        public static SizeI Add(SizeI sz1, SizeI sz2) {
            return new SizeI(sz1.Width + sz2.Width,
                             sz1.Height + sz2.Height);

        }

        public static SizeI Subtract(SizeI sz1, SizeI sz2) {
            return new SizeI(sz1.Width - sz2.Width,
                             sz1.Height - sz2.Height);
        }


    }
}

//
// System.Drawing.Size.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004 Novell, Inc. http://www.novell.com
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