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

using System.Windows.Forms;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;
using System.Collections.Generic;

namespace Limaki.Winform {
    /// <summary>
    /// Converts Windows.Forms.PaintEventArgs and Limaki.PaintActionEventArgs
    /// </summary>
    public class Converter {
        public static IPaintActionEventArgs Convert(PaintEventArgs e) {
            return new GDIPaintActionEventArgs(e.Graphics, e.ClipRectangle);
        }

        public static PaintEventArgs Convert(IPaintActionEventArgs e) {
            return new PaintEventArgs(
                ((GDISurface) e.Surface).Graphics,
                GDIConverter.Convert(e.ClipRectangle));
        }
            
        public static MouseActionButtons Convert(MouseButtons e) {
            return (MouseActionButtons) e;
        }

        public static KeyActionEventArgs Convert(KeyEventArgs e) {
            return new KeyActionEventArgs(
                Convert(e.KeyCode), 
                ConvertModifiers(e.KeyData));
        }

        public static ModifierKeys ConvertModifiers (Keys keys) {
            ModifierKeys result = ModifierKeys.None;
            if ((keys & Keys.Control) != 0)
                result |= ModifierKeys.Control;
            if ((keys & Keys.Alt) != 0)
                result |= ModifierKeys.Alt;
            if ((keys & Keys.Shift) != 0)
                result |= ModifierKeys.Shift;
            if (keys == Keys.LWin || keys == Keys.RWin)
                result |= ModifierKeys.Windows;
            return result;
                
        }

        public static ModifierKeys ConvertModifiers(int keyState) {
            ModifierKeys result = ModifierKeys.None;
            if ((keyState & 4) != 0)
                result |= ModifierKeys.Shift;
            if ((keyState & 8) != 0)
                result |= ModifierKeys.Control;
            if ((keyState & 32) != 0)
                result |= ModifierKeys.Alt;
            return result;
        }

        public static MouseActionEventArgs Convert(MouseEventArgs e) {
            return new MouseActionEventArgs (
                Convert(e.Button),
                ConvertModifiers(Form.ModifierKeys),
                e.Clicks, 
                e.X, 
                e.Y, 
                e.Delta);
        }

        static IDictionary<Keys, Key> keytable = null;
        protected static IDictionary<Keys, Key> KeyTable {
            get {
                if (keytable == null) {
                    keytable = new Dictionary<Keys, Key> ();

                    keytable.Add (Keys.Multiply, Key.Multiply);
                    keytable.Add (Keys.Add, Key.Add);
                    keytable.Add(Keys.Oemplus, Key.Add);
                    keytable.Add (Keys.Subtract, Key.Subtract);
                    keytable.Add(Keys.OemMinus, Key.Subtract);
                    keytable.Add (Keys.Decimal, Key.Decimal);
                    keytable.Add (Keys.Divide, Key.Divide);
                    keytable.Add (Keys.Back, Key.Back);
                    keytable.Add (Keys.Tab, Key.Tab);
                    keytable.Add (Keys.Enter, Key.Enter);
                    keytable.Add (Keys.Shift, Key.Shift);
                    keytable.Add (Keys.ShiftKey, Key.Shift);
                    keytable.Add (Keys.LShiftKey, Key.Shift);
                    keytable.Add (Keys.RShiftKey, Key.Shift);
                    keytable.Add (Keys.Control, Key.Ctrl);
                    keytable.Add (Keys.ControlKey, Key.Ctrl);
                    keytable.Add (Keys.LControlKey, Key.Ctrl);
                    keytable.Add (Keys.RControlKey, Key.Ctrl);
                    keytable.Add (Keys.Alt, Key.Alt);
                    keytable.Add (Keys.CapsLock, Key.CapsLock);
                    keytable.Add (Keys.Escape, Key.Escape);
                    keytable.Add (Keys.Space, Key.Space);
                    keytable.Add (Keys.PageUp, Key.PageUp);
                    keytable.Add (Keys.PageDown, Key.PageDown);
                    keytable.Add (Keys.End, Key.End);
                    keytable.Add (Keys.Home, Key.Home);
                    keytable.Add (Keys.Left, Key.Left);
                    keytable.Add (Keys.Up, Key.Up);
                    keytable.Add (Keys.Right, Key.Right);
                    keytable.Add (Keys.Down, Key.Down);
                    keytable.Add (Keys.Insert, Key.Insert);
                    keytable.Add (Keys.Delete, Key.Delete);
                    keytable.Add (Keys.Menu, Key.Menu);
                    keytable.Add (Keys.LMenu, Key.Menu);
                    keytable.Add (Keys.RMenu, Key.Menu);
                }
                return keytable;
            }
        }
        public static Key Convert(Keys keys) {
            uint code = (uint)keys & 0x0000FFFF;
            Key result = Key.None;

            if (code >= (uint)Keys.A && code <= (uint)Keys.Z)
                result = (Key)((uint)Key.A + code - (uint)Keys.A);

            else if (code >= (uint)Keys.D0 && code <= (uint)Keys.D9)
                result = (Key)((uint)Key.D0 + code - (uint)Keys.D0);

            else if (code >= (uint)Keys.F1 && code <= (uint)Keys.F12)
                result = (Key)((uint)Key.F1 + code - (uint)Keys.F1);

            else if (code >= (uint)Keys.NumPad0 && code <= (uint)Keys.NumPad9)
                result = (Key)((uint)Key.NumPad0 + code - (uint)Keys.NumPad0);

            else
                KeyTable.TryGetValue(keys, out result);

            return result;
        }
    }
}
