//
// Html5DesktopBackend.cs
//
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc.
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
using System.Reflection;
using System.Runtime.InteropServices;
using Xwt.Backends;

using System.Collections.Generic;

namespace Xwt.Html5.Backend {

	public class Html5DesktopBackend: DesktopBackend
	{

		public Html5DesktopBackend ()
		{

		}

		static bool cannotCallGetDpiForMonitor;
		public override double GetScaleFactor (object backend) {
		    return 1;
		}

		#region implemented abstract members of DesktopBackend

		public override Point GetMouseLocation() {
		    return Point.Zero;
		}

		public override IEnumerable<object> GetScreens () {
		    return new object[0];
		}

		public override bool IsPrimaryScreen (object backend) {
		    return true;
		}

		public override Rectangle GetScreenBounds (object backend)
		{
			return new Rectangle(0,0,800,600);
		}

		public override Rectangle GetScreenVisibleBounds (object backend) {
		    return GetScreenBounds(backend);
		}

		public override string GetScreenDeviceName (object backend) {
		    return "Html5ScreenDUmmy";
		}

		#endregion

	}
}

