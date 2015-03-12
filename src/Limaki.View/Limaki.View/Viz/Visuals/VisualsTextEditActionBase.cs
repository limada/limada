/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.ComponentModel;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.UI;
using Xwt;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Visuals {
    /// <summary>
    /// Activates a propriate editor for the selected visual
    /// </summary>
    public abstract class VisualsTextEditActionBase:MouseTimerActionBase, IKeyAction, IEditAction {

        protected VisualsTextEditActionBase (): base() {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }

        protected VisualsTextEditActionBase (
            Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, 
            IDisplay display, ICamera camera,
            IGraphSceneLayout<IVisual,IVisualEdge> layout): this() {

            this.Display = display;
            this.Camera = camera;
            this._sceneHandler = sceneHandler;
            this.Layout = layout;
        }

        protected abstract void AttachEditor();
        protected abstract void DetachEditor (bool writeData);
        protected abstract void ActivateMarkers ();

        protected ICamera Camera { get; set; }

        protected IDisplay Display { get; set; }

        Func<IGraphScene<IVisual, IVisualEdge>> _sceneHandler;
        public IGraphScene<IVisual, IVisualEdge> Scene {
            get { return _sceneHandler(); }
        }

        /// <summary>
        /// has to be the same as in GraphItemResizer
        /// </summary>
        public int HitSize { get; set; }

        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }

        public virtual IVisual Visual {
            get { return Scene.Focused; }
        }

        public virtual IVisual Current { get; set; }

        private bool _exclusive;
        public override bool Exclusive { get { return _exclusive; } protected set { _exclusive = value; } }

        bool HitTest(IVisual visual, Point p) {
            bool result = false;
            if (visual == null)
                return result;

            var sp = Camera.ToSource (p);

            result = visual.Shape.IsHit (sp, HitSize);

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
            LastMousePos = e.Location;
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

        #region Data-Handling

        protected TypeConverter GetConverter(IVisual visual) {
            if (visual.Data == null)
                return TypeDescriptor.GetConverter(typeof (object));
            return TypeDescriptor.GetConverter(visual.Data.GetType());
        }

        protected string DataToText (IVisual visual) {
            var converter = GetConverter(visual);
            if (converter != null) {
                if(visual.Data==null)
                    return Limada.Schemata.CommonSchema.NullString;
                return converter.ConvertToString(visual.Data);
            } else {
                return "<error>";
            }
        }

        protected void TextToData (IVisual visual, string text) {
            var scene = this.Scene;
            
            var converter = GetConverter (visual);
            if (converter == null) return;

            object data = null;
            try {
                data = converter.ConvertFromString (text);

            } catch (Exception ex) {
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
            if (data == null) return;
           

            if (visual is IVisualEdge && scene.Markers !=null) {
                object marker = scene.Markers.FittingMarker(data);
                if (marker == null) {
                    marker = scene.Markers.CreateMarker(data);
                }
                data = marker;
                scene.Markers.DefaultMarker = marker;
            } 
            scene.Graph.DoChangeData(visual, data);
            scene.Graph.OnDataChanged(visual);
            scene.Requests.Add (new LayoutCommand<IVisual> (visual, LayoutActionType.Justify));
        }

        protected virtual void AfterEdit () {
            var scene = Scene;
            if (focusAfterEdit) {
             
                scene.Selected.Clear();
                if (scene.Focused != null)
                    scene.Requests.Add(new StateChangeCommand<IVisual>(scene.Focused, new Pair<UiState>(UiState.Selected, UiState.None)));
                scene.Focused = Current;
            }
            if (Current != null) {
                var aligner = new Aligner<IVisual, IVisualEdge>(scene, this.Layout);
                aligner.Justify(new IVisual[] {Current});
                aligner.Commit();
            }
        }
        #endregion

        #region IKeyAction Member


        protected bool focusAfterEdit = false;
        protected bool hoverAfteredit = false;
   
        public void OnKeyReleased( KeyActionEventArgs e ) {}

        public void AttachTo (IVisual visual) {
            DetachEditor (true);
            this.Current = visual;
            Resolved = true;
            Exclusive = true;
            AttachEditor ();
        }

        protected virtual void StartOrConfirmEdit () {
            if (Exclusive) {
                Exclusive = false;
                DetachEditor (true);
            } else {
                if (Visual != null) {//&& HitTest(Visual, lastMousePos)
                    Exclusive = Resolved = true;
                    Current = Visual;
                    AttachEditor ();
                }
            }
        }

        protected virtual bool IsKeyStartOrConfirmEdit (KeyEventArgs e) {
            return e.Key == Key.F2 || (e.Key == Key.Space && e.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt));
        }

        protected virtual bool IsKeyCancelEdit (KeyEventArgs e) {
            return e.Key == Key.Escape;
        }

        protected virtual void KeyEditBehaviour (object sender, KeyEventArgs e) {
            KeyEditBehaviour (e);
        }

        protected virtual void KeyEditBehaviour (KeyEventArgs e) {
            if (IsKeyCancelEdit (e)) {
                CancelEdit ();
                e.Handled = true;
            } else if (IsKeyStartOrConfirmEdit (e)) {
                StartOrConfirmEdit ();
                e.Handled = true;
            }
        }

        protected virtual void CancelEdit () {
            Exclusive = false;
            DetachEditor (false);
        }

        protected abstract Point CursorPosition();

        public virtual void OnKeyPressed (KeyActionEventArgs e) {

            KeyEditBehaviour (e);
            if (e.Handled)
                return;

            focusAfterEdit = false;
            hoverAfteredit = false;
            bool insert = false;

            if (e.Key == Key.Return && e.Modifiers == ModifierKeys.Control) {
                insert = true;
            }

            if (e.Key == Key.Insert) {
                insert = true;
                focusAfterEdit = true;
            }

            if (insert) {
                DetachEditor (true);
                Exclusive = Resolved = true;
                Current = Registry.Pooled<IVisualFactory> ().CreateItem ("XXXXXXXX");
                var scene = Scene;
                var root = scene.Focused;

                if (root == null) {
                    var pt = CursorPosition();
                    pt = Camera.ToSource (pt) - Layout.Distance;
                    SceneExtensions.AddItem (scene, Current, Layout, pt);
                } else {
                    SceneExtensions.PlaceVisual (scene, root, Current, Layout);
                }

                Display.Perform ();
                TextToData (Current, string.Empty);

                AttachEditor ();
                e.Handled = true;
            }
           
        }
        #endregion
    }
}