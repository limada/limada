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
 * http://limada.sourceforge.net
 * 
 */

using System.Windows.Forms;
using Limaki.Drawing.GDI;
using System.Collections.Generic;
using Limaki.Presenter.UI;
using Limaki.Presenter.GDI.UI;
using System.ComponentModel;
using Limaki.Common;

namespace Limaki.Presenter.Winform {
    /// <summary>
    /// Converts Windows.Forms.PaintEventArgs and Limaki.PaintActionEventArgs
    /// </summary>
    public class Converter {

        public static IRenderEventArgs Convert(PaintEventArgs e) {
            return new GDIRenderEventArgs(e.Graphics, e.ClipRectangle);
        }

        public static PaintEventArgs Convert(IRenderEventArgs e) {
            return new PaintEventArgs(
                ((GDISurface)e.Surface).Graphics,
                GDIConverter.Convert(e.Clipper.Bounds));
        }

        public static MouseActionButtons Convert(MouseButtons e) {
            return (MouseActionButtons)e;
        }

        public static KeyActionEventArgs Convert(KeyEventArgs e, System.Drawing.Point location) {
            return new KeyActionEventArgs(
                Convert(e.KeyCode),
                ConvertModifiers(e.KeyData),
                GDIConverter.Convert(location));
        }

        public static KeyEventArgs Convert(KeyActionEventArgs e) {
            return new KeyEventArgs(Convert(e.Key, e.ModifierKeys));
        }

        public static ModifierKeys ConvertModifiers(Keys keys) {
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

                    keytable.Add(Keys.Multiply, Key.Multiply);
                    keytable.Add(Keys.Add, Key.Add);
                    keytable.Add(Keys.Oemplus, Key.Add);
                    keytable.Add(Keys.Subtract, Key.Subtract);
                    keytable.Add(Keys.OemMinus, Key.Subtract);
                    keytable.Add(Keys.Decimal, Key.Decimal);
                    keytable.Add(Keys.Divide, Key.Divide);
                    keytable.Add(Keys.Back, Key.Back);
                    keytable.Add(Keys.Tab, Key.Tab);
                    keytable.Add(Keys.Enter, Key.Enter);
                    keytable.Add(Keys.Shift, Key.Shift);
                    keytable.Add(Keys.ShiftKey, Key.Shift);
                    keytable.Add(Keys.LShiftKey, Key.Shift);
                    keytable.Add(Keys.RShiftKey, Key.Shift);
                    keytable.Add(Keys.Control, Key.Ctrl);
                    keytable.Add(Keys.ControlKey, Key.Ctrl);
                    keytable.Add(Keys.LControlKey, Key.Ctrl);
                    keytable.Add(Keys.RControlKey, Key.Ctrl);
                    keytable.Add(Keys.Alt, Key.Alt);
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

        [TODO("does not work, make a testcase")]
        public static Keys Convert(Key keys, ModifierKeys modifiers) {
            byte code = (byte)keys;
            Keys result = 0;

            if (code >= (byte)Key.A && code <= (byte)Key.Z)
                result = (Keys)((byte)Key.A + code - (byte)Key.A);

            else if (code >= (byte)Key.D0 && code <= (byte)Key.D9)
                result = (Keys)((byte)Key.D0 + code - (byte)Key.D0);

            else if (code >= (byte)Key.F1 && code <= (byte)Key.F12)
                result = (Keys)((byte)Key.F1 + code - (byte)Key.F1);

            else if (code >= (byte)Key.NumPad0 && code <= (byte)Key.NumPad9)
                result = (Keys)((byte)Key.NumPad0 + code - (byte)Key.NumPad0);

            else
                KeyTableW.TryGetValue(keys, out result);

            if ((modifiers & ModifierKeys.Control) != 0)
                result |= Keys.Control;
            if ((modifiers & ModifierKeys.Alt) != 0)
                result |= Keys.Alt;
            if ((modifiers & ModifierKeys.Shift) != 0)
                result |= Keys.Shift;
            if ((modifiers & ModifierKeys.Windows) != 0)
                result |= Keys.LWin;
            if ((modifiers & ModifierKeys.Control) != 0)
                result |= Keys.Control;


            return result;
        }
        public static Limaki.UseCases.Viewers.DialogResult Convert(DialogResult value) {
            var result = Limaki.UseCases.Viewers.DialogResult.None;
            if (value == DialogResult.OK) {
                result = Limaki.UseCases.Viewers.DialogResult.OK;
            }
            if (value == DialogResult.Cancel) {
                result = Limaki.UseCases.Viewers.DialogResult.Cancel;
            }
            if (value == DialogResult.Abort) {
                result = Limaki.UseCases.Viewers.DialogResult.Abort;
            }
            if (value == DialogResult.Retry) {
                result = Limaki.UseCases.Viewers.DialogResult.Retry;
            }
            if (value == DialogResult.Ignore) {
                result = Limaki.UseCases.Viewers.DialogResult.Ignore;
            }
            if (value == DialogResult.Yes) {
                result = Limaki.UseCases.Viewers.DialogResult.Yes;
            }
            if (value == DialogResult.No) {
                result = Limaki.UseCases.Viewers.DialogResult.No;
            }

            return result;
        }

        public static DialogResult Convert(Limaki.UseCases.Viewers.DialogResult value) {
            var result = DialogResult.None;
            if (value == null)
                return result;
            if (value == Limaki.UseCases.Viewers.DialogResult.OK) {
                result = DialogResult.OK;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.Cancel) {
                result = DialogResult.Cancel;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.Abort) {
                result = DialogResult.Abort;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.Retry) {
                result = DialogResult.Retry;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.Ignore) {
                result = DialogResult.Ignore;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.Yes) {
                result = DialogResult.Yes;
            }
            if (value == Limaki.UseCases.Viewers.DialogResult.No) {
                result = DialogResult.No;
            }

            return result;
        }

        public static MessageBoxButtons Convert(Limaki.UseCases.Viewers.MessageBoxButtons value) {
            var result = MessageBoxButtons.OK;
            if (value == null)
                return result;
            if (value == Limaki.UseCases.Viewers.MessageBoxButtons.OKCancel)
                return MessageBoxButtons.OKCancel;
            if (value == Limaki.UseCases.Viewers.MessageBoxButtons.AbortRetryIgnore)
                return MessageBoxButtons.AbortRetryIgnore;
            if (value == Limaki.UseCases.Viewers.MessageBoxButtons.YesNoCancel)
                return MessageBoxButtons.YesNoCancel;
            if (value == Limaki.UseCases.Viewers.MessageBoxButtons.YesNo)
                return MessageBoxButtons.YesNo;
            if (value == Limaki.UseCases.Viewers.MessageBoxButtons.RetryCancel)
                return MessageBoxButtons.RetryCancel;
            return result;
        }

        public static Limaki.UseCases.Viewers.MessageBoxButtons Convert(MessageBoxButtons value) {
            var result = Limaki.UseCases.Viewers.MessageBoxButtons.OK;
            if (value == null)
                return result;
            if (value == MessageBoxButtons.OKCancel)
                return Limaki.UseCases.Viewers.MessageBoxButtons.OKCancel;
            if (value == MessageBoxButtons.AbortRetryIgnore)
                return Limaki.UseCases.Viewers.MessageBoxButtons.AbortRetryIgnore;
            if (value == MessageBoxButtons.YesNoCancel)
                return Limaki.UseCases.Viewers.MessageBoxButtons.YesNoCancel;
            if (value == MessageBoxButtons.YesNo)
                return Limaki.UseCases.Viewers.MessageBoxButtons.YesNo;
            if (value == MessageBoxButtons.RetryCancel)
                return Limaki.UseCases.Viewers.MessageBoxButtons.RetryCancel;
            return result;
        }

        public static void FileDialogSetValue(FileDialog target, Limaki.UseCases.Viewers.FileDialogMemento source) {
            target.AddExtension = source.AddExtension;
            target.AutoUpgradeEnabled = source.AutoUpgradeEnabled;
            target.CheckFileExists = source.CheckFileExists;
            target.CheckPathExists = source.CheckPathExists;
            target.DefaultExt = source.DefaultExt;
            target.DereferenceLinks = source.DereferenceLinks;
            target.FileName = IOUtils.NiceFileName(source.FileName);
            target.Filter = source.Filter;
            target.FilterIndex = source.FilterIndex;
            target.InitialDirectory = source.InitialDirectory;
            target.RestoreDirectory = source.RestoreDirectory;
            target.ShowHelp = source.ShowHelp;
            target.Title = source.Title;
            target.ValidateNames = source.ValidateNames;
        }

        public static void FileDialogSetValue(Limaki.UseCases.Viewers.FileDialogMemento target, FileDialog source) {
            target.AddExtension = source.AddExtension;
            target.AutoUpgradeEnabled = source.AutoUpgradeEnabled;
            target.CheckFileExists = source.CheckFileExists;
            target.CheckPathExists = source.CheckPathExists;
            target.DefaultExt = source.DefaultExt;
            target.DereferenceLinks = source.DereferenceLinks;

            target.Filter = source.Filter;
            target.FilterIndex = source.FilterIndex;
            target.InitialDirectory = source.InitialDirectory;
            target.RestoreDirectory = source.RestoreDirectory;
            target.ShowHelp = source.ShowHelp;
            target.Title = source.Title;
            target.ValidateNames = source.ValidateNames;

            target.FileName = source.FileName;
            target.FileNames = source.FileNames;

        }
    }


}
