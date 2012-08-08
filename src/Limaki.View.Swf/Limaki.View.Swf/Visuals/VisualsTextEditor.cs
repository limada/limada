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
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Xwt.Gdi;
using DragEventArgs = System.Windows.Forms.DragEventArgs;
using WidgetRegistry = Xwt.Engine.WidgetRegistry;
using ModifierKeys = Xwt.ModifierKeys;
using Key = Xwt.Key;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf.Visuals {
    
    /// <summary>
    /// Activates a propriate editor for the selected visual
    /// </summary>
    public class VisualsTextEditor:MouseTimerActionBase, IKeyAction, IEditAction {
        public VisualsTextEditor():base() {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }

        ICamera camera = null;
        ContainerControl device = null;
        IDisplay display = null;

        public VisualsTextEditor(
            Get<IGraphScene<IVisual,IVisualEdge>> sceneHandler, 
            ContainerControl device,
            IDisplay display,
            ICamera camera,
            IGraphSceneLayout<IVisual,IVisualEdge> layout)
            : this() {
            this.device = device;
            this.display = display;
            this.camera = camera;
            this._sceneHandler = sceneHandler;
            this.Layout = layout;
        }


        Get<IGraphScene<IVisual, IVisualEdge>> _sceneHandler;
        public IGraphScene<IVisual, IVisualEdge> Scene {
            get { return _sceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in GraphItemResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }


        private IGraphSceneLayout<IVisual,IVisualEdge> _layout = null;
        public virtual IGraphSceneLayout<IVisual,IVisualEdge> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        public virtual IVisual Visual {
            get { return Scene.Focused; }
        }

        private IVisual _current = null;
        public virtual IVisual Current {
            get { return _current; }
            set { _current = value; }
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        bool HitTest(IVisual visual, Point p) {
            bool result = false;
            if (visual == null)
                return result;

            var sp = camera.ToSource(p);

            result = visual.Shape.IsHit(sp, HitSize);

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
                Resolved = Resolved && !(Visual is IVisualEdge) &&
                           HitTest(Visual, e.Location);
                Exclusive = Resolved;
                if (Resolved) {
                    Current = Visual;
                    AttachEditor();
                }
            }
            Resolved = false;
        }

        #endregion

        #region Editor-Handling
       
        private TextBox editor = new TextBox();

        void ActivateMarkers() {
            var scene = Scene;
            if (Current is IVisualEdge && scene.Markers != null) {
                editor.AutoCompleteCustomSource =
                    new AutoCompleteStringCollection ();
                editor.AutoCompleteCustomSource.AddRange (scene.Markers.MarkersAsStrings());
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

            device.Controls.Add(editor);

            editor.Text = DataToText(Current);
            
            editor.Visible = true;
            ActivateMarkers();
            editor.Focus ();
            device.ActiveControl = editor;
            display.ActiveControl = editor;
        }


        void DetachEditor(bool writeData) {
            if (Current == null)
                return;
            if (writeData) {
                TextToData (Current, editor.Text);
            }
            if (focusAfterEdit) {
                var scene = Scene;
                scene.Selected.Clear();
                if (scene.Focused != null)
                    scene.Requests.Add(new StateChangeCommand<IVisual>(scene.Focused, new Pair<UiState>(UiState.Selected, UiState.None)));
                scene.Focused = Current;
            }

            Current = null;

            editor.Visible = false;
            
            editor.KeyDown -= control_KeyCancelEdit;
            editor.KeyDown -= control_KeyStartOrConfirmEdit;

            editor.Text = String.Empty;
            if (hoverAfteredit && ! focusAfterEdit) { // this does not work!
                Cursor.Position =
                    device.PointToScreen(editor.Location + 
                    new System.Drawing.Size(editor.Size.Width / 2, editor.Size.Height / 2));
                //Scene scene = Scene;
                //if (scene.Hovered != null)
                //scene.Commands.Add(new StateChangeCommand(scene.Hovered, new Pair<UiState>(UiState.Hovered, UiState.None)));
                //scene.Hovered = Current;

            }

            editor.AutoCompleteMode = AutoCompleteMode.None;
            editor.AutoCompleteCustomSource = null;
            editor.AutoCompleteSource = AutoCompleteSource.None;

            device.ActiveControl = null;
            device.Controls.Remove (editor);
            device.Focus ();
            
            display.ActiveControl = null;
        }

        private GdiFontCache gdiFontCache = new GdiFontCache();
        void StyleEditor() {
            var style = Layout.StyleSheet.ItemStyle.DefaultStyle;
            var newFont = new FontMemento(GdiEngine.Registry.GetBackend(style.Font) as System.Drawing.Font);
            newFont.SizeInPoints = (float)camera.Matrice.TransformFontSize (newFont.SizeInPoints);
            editor.Font = gdiFontCache.GetFont(newFont);
            
            editor.BorderStyle = BorderStyle.FixedSingle;
            editor.Multiline = true;
            editor.ScrollBars = ScrollBars.None;
            editor.WordWrap = true;
            
            editor.BackColor = System.Drawing.Color.FromArgb((int)style.FillColor.ToRgb() );
            var location = camera.FromSource(Current.Location);
            var size = camera.FromSource(Current.Size);
            if (Current is IVisualEdge) {
                location = camera.FromSource(Current.Shape[Anchor.Center]);
                size = (Size)
                    GdiUtils.GetTextDimension (editor.Font, Current.Data.ToString (),new System.Drawing.SizeF ())
                    ;
                size.Height = Math.Max(size.Height+2,(int)newFont.SizeInPoints+2);
                size.Width = Math.Max (size.Width+2, (int)newFont.SizeInPoints*4);
                location.X = location.X - size.Width/2;
                location.Y = location.Y - size.Height/2;
            }
            editor.Location = location.ToGdi();
            editor.Size = size.ToGdi();
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
        TypeConverter GetConverter(IVisual visual) {
            return TypeDescriptor.GetConverter(visual.Data.GetType());
        }

        string DataToText(IVisual visual) {
            TypeConverter converter = GetConverter(visual);
            if (converter != null) {
                return converter.ConvertToString(visual.Data);
            } else {
                return "<error>";
            }
        }

        void TextToData(IVisual visual, string text) {
            var scene = this.Scene;
            
            TypeConverter converter = GetConverter (visual);
            if (converter == null) return;

            object data = converter.ConvertFromString (text);
            if (data==null) return;

            if (visual is IVisualEdge && scene.Markers !=null) {
                object marker = scene.Markers.FittingMarker(data);
                if (marker == null) {
                    marker = scene.Markers.CreateMarker(data);
                }
                data = marker;
                scene.Markers.DefaultMarker = marker;
            } 
            scene.Graph.OnChangeData(visual, data);
            scene.Graph.OnDataChanged(visual);
            scene.Requests.Add (new LayoutCommand<IVisual> (visual, LayoutActionType.Justify));
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
                    if (Visual != null) {//&& HitTest(Visual, lastMousePos)
                        Exclusive = Resolved = true;
                        Current = Visual;
                        AttachEditor();
                    }
                }
                e.Handled = true;
            }
        }

        bool focusAfterEdit = false;
        bool hoverAfteredit = false;
        public void OnKeyPressed( KeyActionEventArgs e ) {
            if (e.Key == Key.Escape) {
                control_KeyCancelEdit (this, new KeyEventArgs(Keys.Escape));
            }
            if (e.Key == Key.F2)
                control_KeyStartOrConfirmEdit(this, new KeyEventArgs(Keys.F2));

            focusAfterEdit = false;
            hoverAfteredit = false;
            bool insert = false;
            if (e.Key == Key.Return) {
                insert = true;
            }
            if (e.Key == Key.Insert) {
                insert = true;
                focusAfterEdit = true;
            }

            if (insert) {
                DetachEditor(true);
                Exclusive = Resolved = true;
                Current = Registry.Pool.TryGetCreate<IVisualFactory>().CreateItem<string>("XXXXXXXX");
                var scene = Scene;
                IVisual root = scene.Focused;

                if (root == null) {
                    var pt = device.PointToClient(Cursor.Position).ToXwt();
                    pt = camera.ToSource(pt) - Layout.Distance;
                    SceneExtensions.AddItem(scene, Current, Layout, pt);
                } else {
                    SceneExtensions.PlaceVisual(scene, root, Current, Layout);
                }
                
                display.Execute();
                TextToData(Current, string.Empty);

                AttachEditor();                
            }
        }

        public void OnKeyReleased( KeyActionEventArgs e ) {}

        public void AttachTo(IVisual visual) {
            this.Current = visual;
            DetachEditor (true);
            AttachEditor ();
        }

        #endregion
    }
}
