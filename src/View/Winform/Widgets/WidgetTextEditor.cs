/*
 * Limaki 
 * Version 0.071
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Drawing.Shapes;

namespace Limaki.Winform.Widgets {
    /// <summary>
    /// Activates a propriate editor for the selected widget
    /// </summary>
    public class WidgetTextEditor:MouseTimerActionBase, IKeyAction {
        public WidgetTextEditor():base() {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }
        ///<directed>True</directed>
        ICamera camera = null;
        ContainerControl control = null;
        public WidgetTextEditor(Handler<Scene> sceneHandler, ContainerControl control, ICamera camera)
            : this() {
            this.control = control;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }


        private IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        public IWidget Widget {
            get { return Scene.Focused; }
        }

        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        bool HitTest(IWidget widget, Point p) {
            bool result = false;
            if (widget == null)
                return result;

            Point sp = camera.ToSource(p);

            result = widget.Shape.IsHit(sp, HitSize);

            return result;
        }

        #region Mouse-Handling

        public override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (Exclusive) {
                bool doCancel = !HitTest (Current, e.Location);
                if (doCancel) {
                    Exclusive = false;
                    DetachEditor(true);
                }
            } 

        }
        public override void OnMouseMove(MouseEventArgs e) {
            lastMousePos = e.Location;
        }

        public override void OnMouseUp(MouseEventArgs e) {
            if (!Exclusive) {
                base.OnMouseUp(e);
                Resolved = Resolved && !(Widget is IEdgeWidget) &&
                           HitTest(Widget, e.Location);
                Exclusive = Resolved;
                if (Resolved) {
                    Current = Widget;
                    AttachEditor();
                }
            }
            Resolved = false;
        }

        #endregion
        #region Editor-Handling
        private TextBox editor = new TextBox ();
        void AttachEditor() {
            editor.KeyDown += control_KeyCancelEdit;
            editor.KeyDown += control_KeyStartOrConfirmEdit;

            StyleEditor ();

            control.Controls.Add(editor);

            editor.Text = DataToText(Current);

            editor.Visible = true;
            editor.Focus ();
            control.ActiveControl = editor;
        }


        void DetachEditor(bool writeData) {
            if (writeData) {
                TextToData (Current, editor.Text);
            }

            Current = null;

            editor.Visible = false;
            
            editor.KeyDown -= control_KeyCancelEdit;
            editor.KeyDown -= control_KeyStartOrConfirmEdit;

            editor.Text = String.Empty;

            control.ActiveControl = null;
            control.Controls.Remove (editor);
            control.Focus ();
        }

        private FontCache fontCache = new FontCache();
        void StyleEditor() {
            FontMemento newFont = new FontMemento (Style.Font);
            newFont.SizeInPoints = camera.Matrice.TransformFontSize (newFont.SizeInPoints);
            editor.Font = fontCache.GetFont(newFont);
            
            editor.BorderStyle = BorderStyle.FixedSingle;
            editor.Multiline = true;
            editor.ScrollBars = ScrollBars.None;
            editor.WordWrap = true;
            editor.BackColor = Style.FillColor;
            Point location = camera.FromSource(Current.Location);
            Size size = camera.FromSource(Current.Size);
            if (Current is IEdgeWidget) {
                location = camera.FromSource(Current.Shape[Anchor.Center]);
                size = 
                    ShapeUtils.GetTextDimension (editor.Font, Current.Data.ToString (),new SizeF ())
                    .ToSize();
                size.Height = Math.Max(size.Height+2,(int)newFont.SizeInPoints+2);
                size.Width = Math.Max (size.Width+2, (int)newFont.SizeInPoints*4);
                location.X = location.X - size.Width/2;
                location.Y = location.Y - size.Height/2;
            }
            editor.Location = location;
            editor.Size = size;
            editor.AllowDrop = true;
            editor.DragOver += new DragEventHandler(editor_DragOver);
            editor.DragDrop += new DragEventHandler(editor_DragDrop);
            // does not work:
            //editor.Scale (new SizeF (camera.Matrice.Elements[0], transformer.Matrice.Elements[3]));
        }

        void editor_DragDrop(object sender, DragEventArgs e) {
            string text = e.Data.GetData (typeof (string)) as string;
            if (text != null) {
                editor.Text = text;
            }
        }

        void editor_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(string))) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region Data-Handling
        TypeConverter GetConverter(IWidget widget) {
            return TypeDescriptor.GetConverter(widget.Data.GetType());
        }

        string DataToText(IWidget widget) {
            TypeConverter converter = GetConverter(widget);
            if (converter != null) {
                return converter.ConvertToString(widget.Data);
            } else {
                return "<error>";
            }
        }
        void TextToData(IWidget widget, string text) {
            TypeConverter converter = GetConverter(widget);
            if (converter != null) {
                widget.Data = converter.ConvertFromString(text);
                this.Scene.Graph.OnDataChanged(widget);
            }
        }
        #endregion

        #region IKeyAction Member

        void control_KeyCancelEdit(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Escape) {
                Exclusive = false;
                DetachEditor(false);
                e.Handled = true;
            }
        }

        void control_KeyStartOrConfirmEdit(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.F2) {
                if (Exclusive) {
                    Exclusive = false;
                    DetachEditor(true);
                } else {
                    if (Widget != null && HitTest(Widget, lastMousePos)) {
                        Exclusive = Resolved = true;
                        Current = Widget;
                        AttachEditor();
                    }
                }
                e.Handled = true;
            }
        }

        public void OnKeyDown(KeyEventArgs e) {
            control_KeyCancelEdit (this, e);
            control_KeyStartOrConfirmEdit (this, e);
        }

        public void OnKeyPress(KeyPressEventArgs e) {}

        public void OnKeyUp(KeyEventArgs e) {}

        #endregion
    }
}
