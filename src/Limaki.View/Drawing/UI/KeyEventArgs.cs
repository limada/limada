/*
 * Limaki 
 * Version 0.081
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

using System;

namespace Limaki.Drawing.UI {
    public class KeyActionEventArgs : EventArgs {

        public KeyActionEventArgs(Key key):base() {
            this._key = key;
            this.Handled = false;
        }

        public KeyActionEventArgs(Key key, ModifierKeys modifiers)
            : this(key) {
            this._modifierKeys = modifiers;
        }

        public bool Handled {
            get;
            set;
        }

        private Key _key = Key.None;
        public Key Key {
            get { return _key; }
        }

        private ModifierKeys _modifierKeys = ModifierKeys.None;
        public ModifierKeys ModifierKeys {
            get { return _modifierKeys; }
        }

        public int PlatformKeyCode {
            get { throw new NotImplementedException (); }
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