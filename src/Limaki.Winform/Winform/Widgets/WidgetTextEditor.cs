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
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;
using Limaki.Widgets;

namespace Limaki.Winform.Widgets {
    /// <summary>
    /// Activates a propriate editor for the selected widget
    /// </summary>
    public class WidgetTextEditor:MouseTimerActionBase, IKeyAction {
        public WidgetTextEditor():base() {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }

        ICamera camera = null;
        ContainerControl control = null;
        IControl uiControl = null;

        public WidgetTextEditor(
            Func<Scene> sceneHandler, 
            ContainerControl control,
            IControl uiControl,
            ICamera camera,
            ILayout<Scene,IWidget> layout)
            : this() {
            this.control = control;
            this.uiControl = uiControl;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        ///<directed>True</directed>
        Func<Scene> SceneHandler;
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


        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        public virtual IWidget Widget {
            get { return Scene.Focused; }
        }

        private IWidget _current = null;
        public virtual IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        bool HitTest(IWidget widget, PointI p) {
            bool result = false;
            if (widget == null)
                return result;

            PointI sp = camera.ToSource(p);

            result = widget.Shape.IsHit(sp, HitSize);

            return result;
        }

        #region Mouse-Handling

        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            if (Exclusive) {
                bool doCancel = !HitTest (Current, e.Location);
                if (doCancel) {
                    Exclusive = false;
                    DetachEditor(true);
                }
            } 

        }
        public override void OnMouseMove(MouseActionEventArgs e) {
            lastMousePos = e.Location;
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
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

        void ActivateMarkers() {
            if (Current is IEdgeWidget && Scene.Markers != null) {
                editor.AutoCompleteCustomSource =
                    new AutoCompleteStringCollection ();
                editor.AutoCompleteCustomSource.AddRange (Scene.Markers.MarkersAsStrings());
                editor.AutoCompleteMode = AutoCompleteMode.Suggest;
                editor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                editor.Multiline = false;
            } else {
                editor.Multiline = true;
            }
        }

        void AttachEditor() {
            editor.KeyDown += control_KeyCancelEdit;
            editor.KeyDown += control_KeyStartOrConfirmEdit;

            StyleEditor ();

            control.Controls.Add(editor);

            editor.Text = DataToText(Current);
            
            editor.Visible = true;
            ActivateMarkers();
            editor.Focus ();
            control.ActiveControl = editor;
        }


        void DetachEditor(bool writeData) {
            if (Current == null)
                return;
            if (writeData) {
                TextToData (Current, editor.Text);
            }
            if (focusAfterEdit) {
                Scene scene = Scene;
                scene.Selected.Clear();
                if (scene.Focused != null)
                    scene.Commands.Add(new StateChangeCommand(scene.Focused, new Pair<UiState>(UiState.Selected, UiState.None)));
                scene.Focused = Current;
            }

            Current = null;

            editor.Visible = false;
            
            editor.KeyDown -= control_KeyCancelEdit;
            editor.KeyDown -= control_KeyStartOrConfirmEdit;

            editor.Text = String.Empty;
            if (hoverAfteredit && ! focusAfterEdit) { // this does not work!
                Cursor.Position =
                    control.PointToScreen(editor.Location + 
                    new System.Drawing.Size(editor.Size.Width / 2, editor.Size.Height / 2));
                //Scene scene = Scene;
                //if (scene.Hovered != null)
                //scene.Commands.Add(new StateChangeCommand(scene.Hovered, new Pair<UiState>(UiState.Hovered, UiState.None)));
                //scene.Hovered = Current;

            }

            editor.AutoCompleteMode = AutoCompleteMode.None;
            editor.AutoCompleteCustomSource = null;
            editor.AutoCompleteSource = AutoCompleteSource.None;

            control.ActiveControl = null;
            control.Controls.Remove (editor);
            control.Focus ();
            
            
        }

        private GDIFontCache gdiFontCache = new GDIFontCache();
        void StyleEditor() {
            IStyle style = Layout.StyleSheet.DefaultStyle;
            FontMemento newFont = new FontMemento(((GDIFont)style.Font).Native);
            newFont.SizeInPoints = camera.Matrice.TransformFontSize (newFont.SizeInPoints);
            editor.Font = gdiFontCache.GetFont(newFont);
            
            editor.BorderStyle = BorderStyle.FixedSingle;
            editor.Multiline = true;
            editor.ScrollBars = ScrollBars.None;
            editor.WordWrap = true;
            Color color = style.FillColor;
            color = Color.FromArgb (color.R, color.G, color.B);
            editor.BackColor = GDIConverter.Convert(color);
            PointI location = camera.FromSource(Current.Location);
            SizeI size = camera.FromSource(Current.Size);
            if (Current is IEdgeWidget) {
                location = camera.FromSource(Current.Shape[Anchor.Center]);
                size = 
                    GDIUtils.GetTextDimension (editor.Font, Current.Data.ToString (),new System.Drawing.SizeF ())
                    .ToSize();
                size.Height = Math.Max(size.Height+2,(int)newFont.SizeInPoints+2);
                size.Width = Math.Max (size.Width+2, (int)newFont.SizeInPoints*4);
                location.X = location.X - size.Width/2;
                location.Y = location.Y - size.Height/2;
            }
            editor.Location = GDIConverter.Convert(location);
            editor.Size = GDIConverter.Convert(size);
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
            Scene scene = this.Scene;
            
            TypeConverter converter = GetConverter (widget);
            if (converter == null) return;

            object data = converter.ConvertFromString (text);
            if (data==null) return;

            if (widget is IEdgeWidget && scene.Markers !=null) {
                object marker = scene.Markers.FittingMarker(data);
                if (marker == null) {
                    marker = scene.Markers.CreateMarker(data);
                }
                data = marker;
                scene.Markers.DefaultMarker = marker;
            } 
            scene.Graph.OnChangeData(widget, data);
            scene.Graph.OnDataChanged(widget);
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
                    if (Widget != null ) {//&& HitTest(Widget, lastMousePos)
                        Exclusive = Resolved = true;
                        Current = Widget;
                        AttachEditor();
                    }
                }
                e.Handled = true;
            }
        }

        bool focusAfterEdit = false;
        bool hoverAfteredit = false;
        public void OnKeyDown( KeyActionEventArgs e ) {
            if (e.Key == Key.Escape) {
                control_KeyCancelEdit (this, new KeyEventArgs(Keys.Escape));
            }
            if (e.Key == Key.F2)
                control_KeyStartOrConfirmEdit(this, new KeyEventArgs(Keys.F2));

            focusAfterEdit = false;
            hoverAfteredit = false;
            bool insert = false;
            if (e.Key == Key.Enter) {
                insert = true;
            }
            if (e.Key == Key.Insert) {
                insert = true;
                focusAfterEdit = true;
            }

            if (insert) {
                DetachEditor(true);
                Exclusive = Resolved = true;
                Current = Registry.Pool.TryGetCreate<IWidgetFactory>().CreateWidget<string>("XXXXXXXX");
                Scene scene = Scene;
                IWidget root = scene.Focused;

                if (root == null) {
                    PointI pt = GDIConverter.Convert(control.PointToClient(Cursor.Position));
                    pt = camera.ToSource(pt) - Layout.Distance;
                    SceneTools.AddItem(scene, Current, Layout, pt);
                } else {
                    SceneTools.PlaceWidget(root, Current, scene, Layout);
                }
                
                uiControl.CommandsExecute();
                TextToData(Current, string.Empty);

                AttachEditor();                
            }
        }

        public void OnKeyPress( KeyActionPressEventArgs e ) {}

        public void OnKeyUp( KeyActionEventArgs e ) {}

        public void AttachTo(IWidget widget) {
            this.Current = widget;
            DetachEditor (true);
            AttachEditor ();
        }

        #endregion
    }
}
