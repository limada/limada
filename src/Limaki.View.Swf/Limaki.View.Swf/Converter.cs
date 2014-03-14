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

using System.Windows.Forms;
using Limaki.Drawing.Gdi;
using System.Collections.Generic;
using System.ComponentModel;
using Limaki.Common;
using Limaki.Viewers;
using Limaki.View.Gdi.UI;
using Limaki.View.Rendering;
using Limaki.View.UI;
using DialogResult = System.Windows.Forms.DialogResult;
using ModifierKeys = Xwt.ModifierKeys;
using Key = Xwt.Key;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using SD2 = System.Drawing.Drawing2D;
using Xwt.Gdi.Backend;
using System.Diagnostics;

namespace Limaki.View.Swf {

    public class Converter {

        public static IRenderEventArgs Convert(PaintEventArgs e) {
            return new GdiRenderEventArgs(e.Graphics, e.ClipRectangle);
        }

        public static PaintEventArgs Convert(IRenderEventArgs e) {
            return new PaintEventArgs(
                ((GdiSurface)e.Surface).Graphics,
                e.Clipper.Bounds.ToGdi ());
        }

        public static MouseActionButtons Convert(MouseButtons e) {
            return (MouseActionButtons)e;
        }

        public static KeyActionEventArgs Convert(KeyEventArgs e, System.Drawing.Point location) {
            return new KeyActionEventArgs(
                Convert(e.KeyCode),
                ConvertModifiers(e.KeyData),
                location.ToXwt());
        }

        public static KeyEventArgs Convert(KeyActionEventArgs e) {
            return new KeyEventArgs(Convert(e.Key, e.Modifiers));
        }

        public static ModifierKeys ConvertModifiers(Keys keys) {
            var result = ModifierKeys.None;
            if ((keys & Keys.Control) != 0)
                result |= ModifierKeys.Control;
            if ((keys & Keys.Alt) != 0)
                result |= ModifierKeys.Alt;
            if ((keys & Keys.Shift) != 0)
                result |= ModifierKeys.Shift;
            if (keys == Keys.LWin || keys == Keys.RWin)
                result |= ModifierKeys.Command;
            return result;

        }

        public static ModifierKeys ConvertModifiers(int keyState) {
            var result = ModifierKeys.None;
            if ((keyState & 4) != 0)
                result |= ModifierKeys.Shift;
            if ((keyState & 8) != 0)
                result |= ModifierKeys.Control;
            if ((keyState & 32) != 0)
                result |= ModifierKeys.Alt;
            return result;
        }

        public static MouseActionEventArgs Convert(MouseEventArgs e) {
            return new MouseActionEventArgs(
                Convert(e.Button),
                ConvertModifiers(Form.ModifierKeys),
                e.Clicks,
                e.X,
                e.Y,
                e.Delta);
        }
        static IDictionary<Key, Keys> keytableW = null;
        protected static IDictionary<Key, Keys> KeyTableW {
            get {
                if (keytableW == null) {
                    keytableW = new Dictionary<Key, Keys>();
                    foreach (var keys in KeyTable) {
                        keytableW[keys.Value] = keys.Key;
                    }
                }
                return keytableW;
            }
        }

        static IDictionary<Keys, Key> keytable = null;
        protected static IDictionary<Keys, Key> KeyTable {
            get {
                if (keytable == null) {
                    keytable = new Dictionary<Keys, Key>();

                    keytable.Add(Keys.Multiply, Key.Asterisk);
                    keytable.Add( Keys.Add, Key.Plus);
                    keytable.Add (Keys.Oemplus, Key.Plus);
                    keytable.Add( Keys.Subtract, Key.Minus );
                    keytable.Add (Keys.OemMinus, Key.Minus);
                    keytable.Add( Keys.Decimal, Key.NumPadDecimal );
                    keytable.Add( Keys.Divide, Key.NumPadDivide );
                    keytable.Add(Keys.Back, Key.BackSpace);
                    keytable.Add(Keys.Tab, Key.Tab);
                    keytable.Add(Keys.Enter, Key.Return);
                    keytable.Add(Keys.Shift, Key.ShiftLeft);
                    keytable.Add( Keys.ShiftKey, Key.ShiftLeft );
                    keytable.Add( Keys.LShiftKey, Key.ShiftLeft );
                    keytable.Add(Keys.RShiftKey, Key.ShiftRight);
                    keytable.Add(Keys.Control, Key.ControlLeft);
                    keytable.Add( Keys.ControlKey, Key.ControlLeft );
                    keytable.Add( Keys.LControlKey, Key.ControlLeft );
                    keytable.Add( Keys.RControlKey, Key.ControlRight);
                    keytable.Add(Keys.Alt, Key.AltLeft);
                    keytable.Add(Keys.CapsLock, Key.CapsLock);
                    keytable.Add(Keys.Escape, Key.Escape);
                    keytable.Add(Keys.Space, Key.Space);
                    keytable.Add(Keys.PageUp, Key.PageUp);
                    keytable.Add(Keys.PageDown, Key.PageDown);
                    keytable.Add(Keys.End, Key.End);
                    keytable.Add(Keys.Home, Key.Home);
                    keytable.Add(Keys.Left, Key.Left);
                    keytable.Add(Keys.Up, Key.Up);
                    keytable.Add(Keys.Right, Key.Right);
                    keytable.Add(Keys.Down, Key.Down);
                    keytable.Add(Keys.Insert, Key.Insert);
                    keytable.Add(Keys.Delete, Key.Delete);
                    keytable.Add(Keys.Menu, Key.Menu);
                    keytable.Add(Keys.LMenu, Key.Menu);
                    keytable.Add(Keys.RMenu, Key.Menu);
                }
                return keytable;
            }
        }

        public static Key Convert(Keys keys) {
            uint code = (uint)keys & 0x0000FFFF;
            Key result =0;

            if (code >= (uint)Keys.A && code <= (uint)Keys.Z)
                result = (Key)((uint)Key.A + code - (uint)Keys.A);

            else if (code >= (uint)Keys.D0 && code <= (uint)Keys.D9)
                result = (Key)((uint)Key.K0 + code - (uint)Keys.D0);

            else if (code >= (uint)Keys.F1 && code <= (uint)Keys.F12)
                result = (Key)((uint)Key.F1 + code - (uint)Keys.F1);

            else if (code >= (uint)Keys.NumPad0 && code <= (uint)Keys.NumPad9)
                result = (Key)((uint)Key.NumPad0 + code - (uint)Keys.NumPad0);

            else
                KeyTable.TryGetValue(keys, out result);

            return result;
        }

        [TODO("does not work, make a testcase")]
        public static Keys Convert(Key keys, ModifierKeys modifiers) {
            var code = (int)keys;
            Keys result = 0;

            if (code >= (byte)Key.A && code <= (byte)Key.Z)
                result = (Keys)((byte)Key.A + code - (byte)Key.A);

            else if (code >= (byte)Key.K0 && code <= (byte)Key.K9)
                result = (Keys)((byte)Keys.D0 + code - (byte)Key.K0);

            else if ( code >= (int) Key.F1 && code <= (int) Key.F10 )
                result = (Keys) ( (int) Key.F1 + code - (int) Key.F1 );

            else if ( code >= (int) Key.NumPad0 && code <= (int) Key.NumPad9 )
                result = (Keys) ( (int) Key.NumPad0 + code - (int) Key.NumPad0 );

            else
                KeyTableW.TryGetValue(keys, out result);

            if ((modifiers & ModifierKeys.Control) != 0)
                result |= Keys.Control;
            if ((modifiers & ModifierKeys.Alt) != 0)
                result |= Keys.Alt;
            if ((modifiers & ModifierKeys.Shift) != 0)
                result |= Keys.Shift;
            if ((modifiers & ModifierKeys.Command) != 0)
                result |= Keys.LWin;
            if ((modifiers & ModifierKeys.Control) != 0)
                result |= Keys.Control;


            return result;
        }
        public static Limaki.Viewers.DialogResult Convert(DialogResult value) {
            var result = Limaki.Viewers.DialogResult.None;
            if (value == DialogResult.OK) {
                result = Limaki.Viewers.DialogResult.Ok;
            }
            if (value == DialogResult.Cancel) {
                result = Limaki.Viewers.DialogResult.Cancel;
            }
            if (value == DialogResult.Abort) {
                result = Limaki.Viewers.DialogResult.Abort;
            }
            if (value == DialogResult.Retry) {
                result = Limaki.Viewers.DialogResult.Retry;
            }
            if (value == DialogResult.Ignore) {
                result = Limaki.Viewers.DialogResult.Ignore;
            }
            if (value == DialogResult.Yes) {
                result = Limaki.Viewers.DialogResult.Yes;
            }
            if (value == DialogResult.No) {
                result = Limaki.Viewers.DialogResult.No;
            }

            return result;
        }

        public static DialogResult Convert(Limaki.Viewers.DialogResult value) {
            var result = DialogResult.None;
            if (value == null)
                return result;
            if ( value == Limaki.Viewers.DialogResult.Ok ) {
                result = DialogResult.OK;
            }
            if ( value == Limaki.Viewers.DialogResult.Cancel ) {
                result = DialogResult.Cancel;
            }
            if ( value == Limaki.Viewers.DialogResult.Abort ) {
                result = DialogResult.Abort;
            }
            if ( value == Limaki.Viewers.DialogResult.Retry ) {
                result = DialogResult.Retry;
            }
            if ( value == Limaki.Viewers.DialogResult.Ignore ) {
                result = DialogResult.Ignore;
            }
            if ( value == Limaki.Viewers.DialogResult.Yes ) {
                result = DialogResult.Yes;
            }
            if ( value == Limaki.Viewers.DialogResult.No ) {
                result = DialogResult.No;
            }

            return result;
        }

        public static MessageBoxButtons Convert(Limaki.Viewers.MessageBoxButtons value) {
            var result = MessageBoxButtons.OK;
            if (value == null)
                return result;
            if (value == Limaki.Viewers.MessageBoxButtons.OkCancel)
                return MessageBoxButtons.OKCancel;
            if (value == Limaki.Viewers.MessageBoxButtons.AbortRetryIgnore)
                return MessageBoxButtons.AbortRetryIgnore;
            if (value == Limaki.Viewers.MessageBoxButtons.YesNoCancel)
                return MessageBoxButtons.YesNoCancel;
            if (value == Limaki.Viewers.MessageBoxButtons.YesNo)
                return MessageBoxButtons.YesNo;
            if (value == Limaki.Viewers.MessageBoxButtons.RetryCancel)
                return MessageBoxButtons.RetryCancel;
            return result;
        }

        public static Limaki.Viewers.MessageBoxButtons Convert(MessageBoxButtons value) {
            var result = Limaki.Viewers.MessageBoxButtons.Ok;
            if (value == null)
                return result;
            if (value == MessageBoxButtons.OKCancel)
                return Limaki.Viewers.MessageBoxButtons.OkCancel;
            if (value == MessageBoxButtons.AbortRetryIgnore)
                return Limaki.Viewers.MessageBoxButtons.AbortRetryIgnore;
            if (value == MessageBoxButtons.YesNoCancel)
                return Limaki.Viewers.MessageBoxButtons.YesNoCancel;
            if (value == MessageBoxButtons.YesNo)
                return Limaki.Viewers.MessageBoxButtons.YesNo;
            if (value == MessageBoxButtons.RetryCancel)
                return Limaki.Viewers.MessageBoxButtons.RetryCancel;
            return result;
        }

        public static void FileDialogSetValue(FileDialog target, FileDialogMemento source) {
            target.AddExtension = true;
            target.AutoUpgradeEnabled = true;
            target.CheckFileExists = false;
            target.CheckPathExists = false;
            target.DefaultExt = source.DefaultExt;
            target.DereferenceLinks = true;
            target.FileName = IoUtils.NiceFileName(source.FileName);
            target.Filter = source.Filter;
            target.FilterIndex = source.FilterIndex;
            target.InitialDirectory = source.InitialDirectory;
            target.RestoreDirectory = false;
            target.ShowHelp = false;
            target.Title = source.Title;
            target.ValidateNames = true;
            var saveDialog = target as SaveFileDialog;
            if (saveDialog != null)
                saveDialog.OverwritePrompt = source.OverwritePrompt;
        }

        public static void FileDialogSetValue(FileDialogMemento target, FileDialog source) {
            target.DefaultExt = source.DefaultExt;

            target.Filter = source.Filter;
            target.FilterIndex = source.FilterIndex;
            target.InitialDirectory = source.InitialDirectory;
            target.Title = source.Title;

            target.FileName = source.FileName;
            target.FileNames = source.FileNames;
            var saveDialog = source as SaveFileDialog;
            if (saveDialog != null)
                target.OverwritePrompt = saveDialog.OverwritePrompt;
        }

       

    }

   
}
