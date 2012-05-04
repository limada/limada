/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.UI {
    public class KeyActionEventArgs : EventArgs {

        public KeyActionEventArgs(Key key, Point location):base() {
            this._key = key;
            this._location = location;
            this.Handled = false;
        }

        public KeyActionEventArgs(Key key, ModifierKeys modifiers, Point location)
            : this(key, location) {
            this._modifierKeys = modifiers;
        }

        public bool Handled {
            get;
            set;
        }

        private Key _key = 0;
        public Key Key {
            get { return _key; }
        }

        private Point _location = Point.Zero;
        public Point Location {
            get { return _location; }
        }

        private ModifierKeys _modifierKeys = ModifierKeys.None;
        public ModifierKeys ModifierKeys {
            get { return _modifierKeys; }
        }

        public int PlatformKeyCode {
            get {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception(this.GetType().Name + ".PlatformKeyCode not implemented"), MessageType.OK);
                return 0;
            }
        }
    }

    public class KeyActionPressEventArgs : EventArgs {

        public KeyActionPressEventArgs(char keyChar) {
            this.KeyChar = keyChar;
            this.Handled = false;
        }


        public bool Handled {
            get;
            set;
        }

        public char KeyChar {
            get;
            set;
        }

    }

    public delegate void KeyEventHandler(object sender, KeyActionEventArgs e);
}