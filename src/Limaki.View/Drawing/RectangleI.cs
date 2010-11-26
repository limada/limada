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
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\Rectangle.cs
 */



using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Limaki.Drawing {
    [ComVisible(true)]
    public struct RectangleI {
        private int x, y, width, height;

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized Rectangle Structure.
        /// </remarks>

        public static readonly RectangleI Empty;


        /// <summary>
        ///	Ceiling Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Rectangle structure from a RectangleF 
        ///	structure by taking the ceiling of the X, Y, Width,
        ///	and Height properties.
        /// </remarks>

        public static RectangleI Ceiling(RectangleS value) {
            int x, y, w, h;
            checked {
                x = (int)Math.Ceiling(value.X);
                y = (int)Math.Ceiling(value.Y);
                w = (int)Math.Ceiling(value.Width);
                h = (int)Math.Ceiling(value.Height);
            }

            return new RectangleI(x, y, w, h);
        }

        /// <summary>
        ///	FromLTRB Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Rectangle structure from left, top, right,
        ///	and bottom coordinates.
        /// </remarks>

        public static RectangleI FromLTRB(int left, int top,
                                          int right, int bottom) {
            return new RectangleI(left, top, right - left,
                                  bottom - top);
        }

        /// <summary>
        ///	Inflate Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new Rectangle by inflating an existing 
        ///	Rectangle by the specified coordinate values.
        /// </remarks>

        public static RectangleI Inflate(RectangleI rect, int x, int y) {
            RectangleI r = new RectangleI(rect.Location, rect.Size);
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        ///	Inflate Method
        /// </summary>
        ///
        /// <remarks>
        ///	Inflates the Rectangle by a specified width and height.
        /// </remarks>

        public void Inflate(int width, int height) {
            Inflate(new SizeI(width, height));
        }

        /// <summary>
        ///	Inflate Method
        /// </summary>
        ///
        /// <remarks>
        ///	Inflates the Rectangle by a specified Size.
        /// </remarks>

        public void Inflate(SizeI size) {
            x -= size.Width;
            y -= size.Height;
            Width += size.Width * 2;
            Height += size.Height * 2;
        }

        /// <summary>
        ///	Intersect Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new Rectangle by intersecting 2 existing 
        ///	Rectangles. Returns null if there is no	intersection.
        /// </remarks>

        public static RectangleI Intersect(RectangleI a, RectangleI b) {
            // MS.NET returns a non-empty rectangle if the two rectangles
            // touch each other
            if (!a.IntersectsWithInclusive(b))
                return Empty;

            return RectangleI.FromLTRB(
                Math.Max(a.Left, b.Left),
                Math.Max(a.Top, b.Top),
                Math.Min(a.Right, b.Right),
                Math.Min(a.Bottom, b.Bottom));
        }

        /// <summary>
        ///	Intersect Method
        /// </summary>
        ///
        /// <remarks>
        ///	Replaces the Rectangle with the intersection of itself
        ///	and another Rectangle.
        /// </remarks>

        public void Intersect(RectangleI rect) {
            this = RectangleI.Intersect(this, rect);
        }

        /// <summary>
        ///	Round Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Rectangle structure from a RectangleF by
        ///	rounding the X, Y, Width, and Height properties.
        /// </remarks>

        public static RectangleI Round(RectangleS value) {
            int x, y, w, h;
            checked {
                x = (int)Math.Round(value.X);
                y = (int)Math.Round(value.Y);
                w = (int)Math.Round(value.Width);
                h = (int)Math.Round(value.Height);
            }

            return new RectangleI(x, y, w, h);
        }

        /// <summary>
        ///	Truncate Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a Rectangle structure from a RectangleF by
        ///	truncating the X, Y, Width, and Height properties.
        /// </remarks>

        // LAMESPEC: Should this be floor, or a pure cast to int?

        public static RectangleI Truncate(RectangleS value) {
            int x, y, w, h;
            checked {
                x = (int)value.X;
                y = (int)value.Y;
                w = (int)value.Width;
                h = (int)value.Height;
            }

            return new RectangleI(x, y, w, h);
        }

        /// <summary>
        ///	Union Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new Rectangle from the union of 2 existing 
        ///	Rectangles.
        /// </remarks>

        public static RectangleI Union(RectangleI a, RectangleI b) {
            return FromLTRB(Math.Min(a.Left, b.Left),
                            Math.Min(a.Top, b.Top),
                            Math.Max(a.Right, b.Right),
                            Math.Max(a.Bottom, b.Bottom));
        }

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Rectangle objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two Rectangles.
        /// </remarks>

        public static bool operator ==(RectangleI left, RectangleI right) {
            return ((left.Location == right.Location) &&
                    (left.Size == right.Size));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two Rectangle objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two Rectangles.
        /// </remarks>

        public static bool operator !=(RectangleI left, RectangleI right) {
            return ((left.Location != right.Location) ||
                    (left.Size != right.Size));
        }


        // -----------------------
        // Public Constructors
        // -----------------------

        /// <summary>
        ///	Rectangle Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Rectangle from Point and Size values.
        /// </remarks>

        public RectangleI(PointI location, SizeI size) {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        ///	Rectangle Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a Rectangle from a specified x,y location and
        ///	width and height values.
        /// </remarks>

        public RectangleI(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }



        /// <summary>
        ///	Bottom Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Y coordinate of the bottom edge of the Rectangle.
        ///	Read only.
        /// </remarks>


        public int Bottom {
            get {
                return y + height;
            }
        }

        /// <summary>
        ///	Height Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Height of the Rectangle.
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
        ///	IsEmpty Property
        /// </summary>
        ///
        /// <remarks>
        ///	Indicates if the width or height are zero. Read only.
        /// </remarks>		

        public bool IsEmpty {
            get {
                return ((x == 0) && (y == 0) && (width == 0) && (height == 0));
            }
        }

        /// <summary>
        ///	Left Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the left edge of the Rectangle.
        ///	Read only.
        /// </remarks>


        public int Left {
            get {
                return x;
            }
        }

        /// <summary>
        ///	Location Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Location of the top-left corner of the Rectangle.
        /// </remarks>

        [Browsable(false)]
        public PointI Location {
            get {
                return new PointI(x, y);
            }
            set {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        ///	Right Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the right edge of the Rectangle.
        ///	Read only.
        /// </remarks>


        public int Right {
            get {
                return x + width;
            }
        }

        /// <summary>
        ///	Size Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Size of the Rectangle.
        /// </remarks>

        [Browsable(false)]
        public SizeI Size {
            get {
                return new SizeI(width, height);
            }
            set {
                width = value.Width;
                height = value.Height;
            }
        }

        /// <summary>
        ///	Top Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Y coordinate of the top edge of the Rectangle.
        ///	Read only.
        /// </remarks>

        [Browsable(false)]
        public int Top {
            get {
                return y;
            }
        }

        /// <summary>
        ///	Width Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Width of the Rectangle.
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
        ///	X Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the Rectangle.
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
        ///	The Y coordinate of the Rectangle.
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
        ///	Contains Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if an x,y coordinate lies within this Rectangle.
        /// </remarks>

        public bool Contains(int x, int y) {
            return ((x >= this.x) && (x < Right) &&
                    (y >= this.y) && (y < Bottom));
        }

        /// <summary>
        ///	Contains Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a Point lies within this Rectangle.
        /// </remarks>

        public bool Contains(PointI pt) {
            return Contains(pt.X, pt.Y);
        }

        /// <summary>
        ///	Contains Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a Rectangle lies entirely within this 
        ///	Rectangle.
        /// </remarks>

        public bool Contains(RectangleI rect) {
            return (rect == Intersect(this, rect));
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this Rectangle and another object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is RectangleI))
                return false;

            return (this == (RectangleI)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return (height + width) ^ x + y;
        }

        /// <summary>
        ///	IntersectsWith Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a Rectangle intersects with this one.
        /// </remarks>

        public bool IntersectsWith(RectangleI rect) {
            return !((x >= rect.Right) || (Right <= rect.Left) ||
                     (y >= rect.Bottom) || (Bottom <= rect.Top));
        }

        private bool IntersectsWithInclusive(RectangleI r) {
            return !((x > r.Right) || (Right < r.Left) ||
                     (y > r.Bottom) || (Bottom < r.Top));
        }

        /// <summary>
        ///	Offset Method
        /// </summary>
        ///
        /// <remarks>
        ///	Moves the Rectangle a specified distance.
        /// </remarks>

        public void Offset(int x, int y) {
            this.x += x;
            this.y += y;
        }

        /// <summary>
        ///	Offset Method
        /// </summary>
        ///
        /// <remarks>
        ///	Moves the Rectangle a specified distance.
        /// </remarks>

        public void Offset(PointI pos) {
            x += pos.X;
            y += pos.Y;
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the Rectangle as a string in (x,y,w,h) notation.
        /// </remarks>

        public override string ToString() {
            return String.Format("{{X={0},Y={1},Width={2},Height={3}}}",
                                 x, y, width, height);
        }

    }
}

//
// System.Drawing.Rectangle.cs
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