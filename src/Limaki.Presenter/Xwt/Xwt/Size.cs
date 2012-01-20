// 
// Size.cs
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

/*  
 * REMARK lytico
 * this is to refactor Limaki.Presenter to run with 
 * Mono.Xwt https://github.com/mono/xwt
 * will be removed after full support is reached
*/

using System;
using System.ComponentModel;


namespace Limaki.Xwt {
	
	[TypeConverter (typeof(SizeValueConverter))]
	public struct Size
	{		
		double width, height;

		public static readonly Size Zero;

		public Size (double width, double height)
		{
			this.width = width;
			this.height = height;
		}

		public bool IsZero {
			get {
				return ((width == 0) && (height == 0));
			}
		}
		
		[DefaultValue (0d)]
		public double Width {
			get {
				return width;
			}
			set {
				width = value;
			}
		}

		[DefaultValue (0d)]
		public double Height {
			get {
				return height;
			}
			set {
				height = value;
			}
		}

		public static Size operator + (Size s1, Size s2)
		{
			return new Size (s1.width + s2.width, s1.height + s2.height);
		}
		
		public static Size operator - (Size s1, Size s2)
		{
			return new Size (s1.width - s2.width, s1.height - s2.height);
		}
		
		public static bool operator == (Size s1, Size s2)
		{
			return (s1.width == s2.width) && (s1.height == s2.height);
		}
		
		public static bool operator != (Size s1, Size s2)
		{
			return (s1.width != s2.width) || (s1.height != s2.height);
		}
		
		public override bool Equals (object ob)
		{
			return (ob is Size) && this == (Size)ob;
		}

		public override int GetHashCode ()
		{
			return width.GetHashCode () ^ height.GetHashCode ();
		}

		public override string ToString ()
		{
			return String.Format ("[Width={0}, Height={1}]", width, height);
		}
	}
	
	class SizeValueConverter: TypeConverter
	{
		public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}
		
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}
	}
	
	
}
