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
 * Copyright (C) 2001 Mike Kestner
 * Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
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
using Xwt;
using Limaki.Drawing;

namespace Xwt {
    public struct RectangleD {
        private double x, y, width, height;

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized RectangleF Structure.
        /// </remarks>

        public static readonly RectangleD Zero;


        /// <summary>
        ///	FromLTRB Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a RectangleF structure from left, top, right,
        ///	and bottom coordinates.
        /// </remarks>

        public static RectangleD FromLTRB(double left, double top,
                                          double right, double bottom) {
            return new RectangleD(left, top, right - left, bottom - top);
        }

        /// <summary>
        ///	Inflate Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new RectangleF by inflating an existing 
        ///	RectangleF by the specified coordinate values.
        /// </remarks>

        public static RectangleD Inflate(RectangleD rect,
                                         double x, double y) {
            var ir = new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
            ir.Inflate(x, y);
            return ir;
        }

        /// <summary>
        ///	Inflate Method
        /// </summary>
        ///
        /// <remarks>
        ///	Inflates the RectangleF by a specified width and height.
        /// </remarks>

        public RectangleD Inflate(double x, double y) {
            return Inflate(new Size(x, y));
        }

        /// <summary>
        ///	Inflate Method
        /// </summary>
        ///
        /// <remarks>
        ///	Inflates the RectangleF by a specified Size.
        /// </remarks>

        public RectangleD Inflate(Size size) {
            x -= size.Width;
            y -= size.Height;
            width += size.Width * 2;
            height += size.Height * 2;
            return this;
        }

        /// <summary>
        ///	Intersect Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new RectangleF by intersecting 2 existing 
        ///	RectangleFs. Returns null if there is no intersection.
        /// </remarks>

        

        /// <summary>
        ///	Intersect Method
        /// </summary>
        ///
        /// <remarks>
        ///	Replaces the RectangleF with the intersection of itself
        ///	and another RectangleF.
        /// </remarks>

        public void Intersect(RectangleD rect) {
            this = DrawingExtensions.Intersect(this, rect);
        }

        

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two RectangleF objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two RectangleFs.
        /// </remarks>

        public static bool operator ==(RectangleD left, RectangleD right) {
            return (left.X == right.X) && (left.Y == right.Y) &&
                   (left.Width == right.Width) && (left.Height == right.Height);
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two RectangleF objects. The return value is
        ///	based on the equivalence of the Location and Size 
        ///	properties of the two RectangleFs.
        /// </remarks>

        public static bool operator !=(RectangleD left, RectangleD right) {
            return (left.X != right.X) || (left.Y != right.Y) ||
                   (left.Width != right.Width) || (left.Height != right.Height);
        }

       


        // -----------------------
        // Public Constructors
        // -----------------------

        /// <summary>
        ///	RectangleF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a RectangleF from PointF and SizeF values.
        /// </remarks>

        public RectangleD(Point location, Size size) {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        ///	RectangleF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a RectangleF from a specified x,y location and
        ///	width and height values.
        /// </remarks>

        public RectangleD(double x, double y, double width, double height) {
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
        ///	The Y coordinate of the bottom edge of the RectangleF.
        ///	Read only.
        /// </remarks>

        [Browsable(false)]
        public double Bottom {
            get {
                return y + height;
            }
        }

        /// <summary>
        ///	Height Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Height of the RectangleF.
        /// </remarks>

        public double Height {
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
        //		
        [Browsable(false)]
        public bool IsEmpty {
            get {
                return (width <= 0 || height <= 0);
            }
        }

        /// <summary>
        ///	Left Property
        /// </summary>
        ///
        /// <remarks>
        ///	The X coordinate of the left edge of the RectangleF.
        ///	Read only.
        /// </remarks>

        [Browsable(false)]
        public double Left {
            get {
                return x;
            }
        }

        /// <summary>
        ///	Location Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Location of the top-left corner of the RectangleF.
        /// </remarks>

        [Browsable(false)]
        public Point Location {
            get {
                return new Point(x, y);
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
        ///	The X coordinate of the right edge of the RectangleF.
        ///	Read only.
        /// </remarks>

        [Browsable(false)]
        public double Right {
            get {
                return x + width;
            }
        }

        /// <summary>
        ///	Size Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Size of the RectangleF.
        /// </remarks>

        [Browsable(false)]
        public Size Size {
            get {
                return new Size(width, height);
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
        ///	The Y coordinate of the top edge of the RectangleF.
        ///	Read only.
        /// </remarks>

        [Browsable(false)]
        public double Top {
            get {
                return y;
            }
        }

        /// <summary>
        ///	Width Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Width of the RectangleF.
        /// </remarks>

        public double Width {
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
        ///	The X coordinate of the RectangleF.
        /// </remarks>

        public double X {
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
        ///	The Y coordinate of the RectangleF.
        /// </remarks>

        public double Y {
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
        ///	Checks if an x,y coordinate lies within this RectangleF.
        /// </remarks>

        public bool Contains(double x, double y) {
            return ((x >= this.x) && (x < Right) &&
                    (y >= this.y) && (y < Bottom));
        }

        /// <summary>
        ///	Contains Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a Point lies within this RectangleF.
        /// </remarks>

        public bool Contains(Point pt) {
            return Contains(pt.X, pt.Y);
        }

        /// <summary>
        ///	Contains Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a RectangleF lies entirely within this 
        ///	RectangleF.
        /// </remarks>

        public bool Contains(RectangleD rect) {
            return (rect == DrawingExtensions.Intersect(this, rect));
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this RectangleF and an object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is RectangleD))
                return false;

            return (this == (RectangleD)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return (int)(x + y + width + height);
        }

        /// <summary>
        ///	IntersectsWith Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks if a RectangleF intersects with this one.
        /// </remarks>

        public bool IntersectsWith(RectangleD rect) {
            return !((x >= rect.Right) || (Right <= rect.Left) ||
                     (y >= rect.Bottom) || (Bottom <= rect.Top));
        }

        private bool IntersectsWithInclusive(RectangleD r) {
            return !((x > r.Right) || (Right < r.Left) ||
                     (y > r.Bottom) || (Bottom < r.Top));
        }

        /// <summary>
        ///	Offset Method
        /// </summary>
        ///
        /// <remarks>
        ///	Moves the RectangleF a specified distance.
        /// </remarks>

        public void Offset(double x, double y) {
            this.x += x;
            this.y += y;
        }

        /// <summary>
        ///	Offset Method
        /// </summary>
        ///
        /// <remarks>
        ///	Moves the RectangleF a specified distance.
        /// </remarks>

        public void Offset(Point pos) {
            Offset(pos.X, pos.Y);
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the RectangleF in (x,y,w,h) notation.
        /// </remarks>

        public override string ToString() {
            return String.Format("{{X={0},Y={1},Width={2},Height={3}}}",
                                 x, y, width, height);
        }

    }
}

//
// System.Drawing.RectangleF.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
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
namespace Xwt0 {
    public static class DrawingExtensions1 {
        public static RectangleD Intersect(RectangleD a, RectangleD b) {

            Func<bool> intersectsWithInclusive = () => !((a.X > b.Right) || (a.Right < b.Left) ||
                                                         (a.Y > b.Bottom) || (b.Bottom < b.Top));

            // MS.NET returns a non-empty rectangle if the two rectangles
            // touch each other
            if (!intersectsWithInclusive())
                return RectangleD.Zero;

            return RectangleD.FromLTRB(
                Math.Max(a.Left, b.Left),
                Math.Max(a.Top, b.Top),
                Math.Min(a.Right, b.Right),
                Math.Min(a.Bottom, b.Bottom));
        }
    }
}