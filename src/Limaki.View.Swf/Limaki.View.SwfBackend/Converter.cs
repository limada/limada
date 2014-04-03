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

using Limaki.Common;
using Limaki.View.GdiBackend;
using Limaki.View.SwfBackend.Viz;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;
using System.Collections.Generic;
using System.Windows.Forms;
using Xwt;
using Xwt.GdiBackend;
using DialogResult = System.Windows.Forms.DialogResult;
using FileDialog = System.Windows.Forms.FileDialog;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace Limaki.View.SwfBackend {

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
        public static View.Vidgets.DialogResult Convert(DialogResult value) {
            var result = View.Vidgets.DialogResult.None;
            if (value == DialogResult.OK) {
                result = View.Vidgets.DialogResult.Ok;
            }
            if (value == DialogResult.Cancel) {
                result = View.Vidgets.DialogResult.Cancel;
            }
            if (value == DialogResult.Abort) {
                result = View.Vidgets.DialogResult.Abort;
            }
            if (value == DialogResult.Retry) {
                result = View.Vidgets.DialogResult.Retry;
            }
            if (value == DialogResult.Ignore) {
                result = View.Vidgets.DialogResult.Ignore;
            }
            if (value == DialogResult.Yes) {
                result = View.Vidgets.DialogResult.Yes;
            }
            if (value == DialogResult.No) {
                result = View.Vidgets.DialogResult.No;
            }

            return result;
        }

        public static DialogResult Convert(View.Vidgets.DialogResult value) {
            var result = DialogResult.None;
            if (value == null)
                return result;
            if ( value == View.Vidgets.DialogResult.Ok ) {
                result = DialogResult.OK;
            }
            if ( value == View.Vidgets.DialogResult.Cancel ) {
                result = DialogResult.Cancel;
            }
            if ( value == View.Vidgets.DialogResult.Abort ) {
                result = DialogResult.Abort;
            }
            if ( value == View.Vidgets.DialogResult.Retry ) {
                result = DialogResult.Retry;
            }
            if ( value == View.Vidgets.DialogResult.Ignore ) {
                result = DialogResult.Ignore;
            }
            if ( value == View.Vidgets.DialogResult.Yes ) {
                result = DialogResult.Yes;
            }
            if ( value == View.Vidgets.DialogResult.No ) {
                result = DialogResult.No;
            }

            return result;
        }

        public static MessageBoxButtons Convert(View.Vidgets.MessageBoxButtons value) {
            var result = MessageBoxButtons.OK;
            if (value == null)
                return result;
            if (value == View.Vidgets.MessageBoxButtons.OkCancel)
                return MessageBoxButtons.OKCancel;
            if (value == View.Vidgets.MessageBoxButtons.AbortRetryIgnore)
                return MessageBoxButtons.AbortRetryIgnore;
            if (value == View.Vidgets.MessageBoxButtons.YesNoCancel)
                return MessageBoxButtons.YesNoCancel;
            if (value == View.Vidgets.MessageBoxButtons.YesNo)
                return MessageBoxButtons.YesNo;
            if (value == View.Vidgets.MessageBoxButtons.RetryCancel)
                return MessageBoxButtons.RetryCancel;
            return result;
        }

        public static View.Vidgets.MessageBoxButtons Convert(MessageBoxButtons value) {
            var result = View.Vidgets.MessageBoxButtons.Ok;
            if (value == null)
                return result;
            if (value == MessageBoxButtons.OKCancel)
                return View.Vidgets.MessageBoxButtons.OkCancel;
            if (value == MessageBoxButtons.AbortRetryIgnore)
                return View.Vidgets.MessageBoxButtons.AbortRetryIgnore;
            if (value == MessageBoxButtons.YesNoCancel)
                return View.Vidgets.MessageBoxButtons.YesNoCancel;
            if (value == MessageBoxButtons.YesNo)
                return View.Vidgets.MessageBoxButtons.YesNo;
            if (value == MessageBoxButtons.RetryCancel)
                return View.Vidgets.MessageBoxButtons.RetryCancel;
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
