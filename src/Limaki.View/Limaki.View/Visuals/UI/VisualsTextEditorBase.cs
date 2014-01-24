using System;
using System.ComponentModel;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;


namespace Limaki.View.Visuals.UI {
    /// <summary>
    /// Activates a propriate editor for the selected visual
    /// </summary>
    public abstract class VisualsTextEditorBase:MouseTimerActionBase, IKeyAction, IEditAction {

        protected VisualsTextEditorBase (): base() {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }

        protected VisualsTextEditorBase (
            Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, 
            IDisplay display, ICamera camera,
            IGraphSceneLayout<IVisual,IVisualEdge> layout): this() {

            this.display = display;
            this.camera = camera;
            this._sceneHandler = sceneHandler;
            this.Layout = layout;
        }

        protected abstract void AttachEditor();
        protected abstract void DetachEditor (bool writeData);
        protected abstract void ActivateMarkers ();
        public abstract void OnKeyPressed (KeyActionEventArgs e);

        protected ICamera camera { get; set; }

        protected IDisplay display { get; set; }

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

        #region Data-Handling
        protected TypeConverter GetConverter(IVisual visual) {
            if (visual.Data == null)
                return TypeDescriptor.GetConverter(typeof (object));
            return TypeDescriptor.GetConverter(visual.Data.GetType());
        }

        protected string DataToText (IVisual visual) {
            TypeConverter converter = GetConverter(visual);
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


        protected bool focusAfterEdit = false;
        protected bool hoverAfteredit = false;
   
        public void OnKeyReleased( KeyActionEventArgs e ) {}

        public void AttachTo(IVisual visual) {
            this.Current = visual;
            DetachEditor (true);
            AttachEditor ();
        }

        #endregion
    }
}