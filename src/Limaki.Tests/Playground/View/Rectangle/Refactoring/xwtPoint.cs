// 
// Point.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Globalization;

namespace Xwt2 {

	public struct PointD {

		public double X { get; set; }
		public double Y { get; set; }

		public static PointD Zero = new PointD ();

		public override string ToString ()
		{
			return String.Format ("({0},{1})", X.ToString (CultureInfo.InvariantCulture), Y.ToString (CultureInfo.InvariantCulture));
		}
		
		public PointD (double x, double y): this ()
		{
			this.X = x;
			this.Y = y;
		}
		
		public PointD (SizeD sz): this ()
		{
			this.X = sz.Width;
			this.Y = sz.Height;
		}
		
		public override bool Equals (object o)
		{
			if (!(o is PointD))
				return false;
		
			return (this == (PointD) o);
		}
		
		public override int GetHashCode ()
		{
			return X.GetHashCode () ^ Y.GetHashCode ();
		}
		
		public PointD Offset (double dx, double dy)
		{
			PointD p = this;
			p.X += dx;
			p.Y += dy;
			return p;
		}
		
		public bool IsEmpty {
			get {
				return ((X == 0) && (Y == 0));
			}
		}
		
		public static explicit operator SizeD (PointD pt)
		{
			return new SizeD (pt.X, pt.Y);
		}
		
		public static PointD operator + (PointD pt, SizeD sz)
		{
			return new PointD (pt.X + sz.Width, pt.Y + sz.Height);
		}
		
		public static PointD operator - (PointD pt, SizeD sz)
		{
			return new PointD (pt.X - sz.Width, pt.Y - sz.Height);
		}
		
		public static bool operator == (PointD pt_a, PointD pt_b)
		{
			return ((pt_a.X == pt_b.X) && (pt_a.Y == pt_b.Y));
		}
		
		public static bool operator != (PointD pt_a, PointD pt_b)
		{
			return ((pt_a.X != pt_b.X) || (pt_a.Y != pt_b.Y));
		}
	}
}
