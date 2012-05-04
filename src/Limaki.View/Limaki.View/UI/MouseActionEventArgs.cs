/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Authors: 
 * Peter Bartok	pbartok@novell.com
 * 
 * Copyright (c) 2004-2005 Novell, Inc.
 *
 * http://www.limada.org
 * 
 */

/* This code is derived from
 * 
 * Mono 2.4
 * 
 */


using System;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.UI {
    public class MouseActionEventArgs : EventArgs {
        private MouseActionButtons buttons;
        private int clicks;
        private double delta;
        private double x;
        private double y;
        private ModifierKeys modifiers;

        #region Public Constructors
        
        public MouseActionEventArgs(
            MouseActionButtons button, ModifierKeys modifiers,
            int clicks, double x, double y, double delta) {

            this.buttons = button;
            this.modifiers = modifiers;
            this.clicks = clicks;
            this.delta = delta;
            this.x = x;
            this.y = y;

        }

        #endregion	// Public Constructors

        #region Public Instance Properties
        public MouseActionButtons Button {
            get {
                return this.buttons;
            }
        }

        public int Clicks {
            get {
                return this.clicks;
            }
        }

        public double Delta {
            get {
                return this.delta;
            }
        }

        public double X {
            get {
                return this.x;
            }
        }

        public double Y {
            get {return this.y;}
        }

        public Point Location {
            get { return new Point(this.x, this.y); }
        }

        public ModifierKeys Modifiers {
            get { return modifiers; }
        }

        #endregion	// Public Instance Properties
    }

    [Flags]
    public enum MouseActionButtons {
        None = 0x00000000,
        Left = 0x00100000,
        Right = 0x00200000,
        Middle = 0x00400000,
        XButton1 = 0x00800000,
        XButton2 = 0x01000000
    }
}

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
// Copyright (c) 2004-2005 Novell, Inc.
//
// Authors:
//	Peter Bartok	pbartok@novell.com
//


// COMPLETE