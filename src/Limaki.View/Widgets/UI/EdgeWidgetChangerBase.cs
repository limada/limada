using System;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Graphs;
using Limaki.Widgets;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Changes Root or Link of a LinkWidget
    /// </summary>
    public class EdgeWidgetChangerBase : SelectionBase {

        public EdgeWidgetChangerBase(Func<Scene> sceneHandler, IControl control, ICamera camera)
            : base(control, camera) {
            this.SceneHandler = sceneHandler;
            this.Priority = ActionPriorities.SelectionPriority - 20;
        }

        ///<directed>True</directed>
        Func<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }


        public virtual IEdgeWidget Edge {
            get {
                if (Scene == null)
                    return null;
                if (Scene.Focused is IEdgeWidget) {
                    return (IEdgeWidget)Scene.Focused;
                }

                return null;
            }
            set { }
        }

        private IWidget _target = null;
        public virtual IWidget Target {
            get { return _target; }
            set { _target = value; }
        }

        public virtual bool Dragging { get; set; }

        public override bool HitTest(PointI p) {
            bool result = false;
            if (Edge != null) {
                PointI sp = camera.ToSource(p);
                hitAnchor = Edge.Shape.IsAnchorHit(sp, HitSize);
                CursorHandler.SetEdgeCursor(this.control,hitAnchor);
                result = hitAnchor != Anchor.None;
            }
            return result;
        }

        // TOTO: Shape is used to draw the grips; find another solution to get
        // rid of ShapeSelectionBase-Inheritance
        public override IShape Shape {
            get {
                if (Edge != null) {
                    return Edge.Shape;
                } else {
                    return null;
                }
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        protected bool rootMoving = false;
        protected bool checkResizing() {
            return (this.hitAnchor == Anchor.LeftTop) || (this.hitAnchor == Anchor.RightBottom);
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            Exclusive = false;
            if (Edge != null) {
                base.OnMouseDown(e);
                this.resizing = this.resizing && checkResizing();
                Resolved = this.resizing;
                if (Resolved) {
                    rootMoving = Edge.Shape.Location == Edge.Shape[hitAnchor];
                    Exclusive = true;

                }
            }
            //Dragging = !Exclusive;
        }


        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            if ((Edge != null) && (resizing)) {
                ShowGrips = true;

                RectangleI rect = camera.ToSource(
                    RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                        LastMousePos.X, LastMousePos.Y));

                Scene.Commands.Add(new ResizeCommand(Edge, rect));
                foreach (IEdgeWidget twigEdge in Scene.Twig(Edge)) {
                    Scene.Commands.Add(new LayoutCommand<IWidget>(twigEdge, LayoutActionType.Justify));
                }

                // saving current mouse position to be used for next dragging
                this.LastMousePos = e.Location;

                IsTargetHit(this.LastMousePos);

            } else {
                Resolved = false;
            }


        }

        RectangleI clipRect = RectangleI.Empty;
        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            if (HitTest(e.Location)) {
                ShowGrips = true;
            } else {
                ShowGrips = false;
            }
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            Exclusive = false;
            base.OnMouseMove(e);
            if (e.Button != MouseActionButtons.Left) {
                if (Resolved || Dragging) {
                    EndAction();
                    Resolved = false;
                    Dragging = false;
                }
                return;
            }
            Dragging = !Resolved;
        }

        public virtual bool IsTargetHit(PointI p) {
            bool result = false;
            IWidget widget = Scene.Hovered;
            IEdgeWidget edge = this.Edge;
            if ((widget != null) && (edge != null) && (edge != widget)) {
                if (edge.Leaf != widget && edge.Root != widget) {
                    PointI sp = camera.ToSource(p);
                    result = widget.Shape.IsHit(sp, HitSize);
                }
            }
            if (result) {
                this.Target = widget;
            } else {
                Target = null;
            }
            return result;
        }

        protected virtual void SetTarget(IEdgeWidget edge, IWidget target) {
            if ((target != edge.Root) && (target != edge.Leaf)) {
                Scene.ChangeEdge(edge, target, rootMoving);
                Scene.Graph.OnGraphChanged(edge, GraphChangeType.Update);
            }
        }

        protected override void EndAction() {
            if (Edge != null) {
                if (Target != null) {
                    SetTarget(Edge, Target);
                }

                Scene.Commands.Add(new LayoutCommand<IWidget>(Edge, LayoutActionType.Justify));
                foreach (IWidget widget in Scene.Twig(Edge)) {
                    Scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                }
            }

            Target = null;
            base.EndAction();
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp(e);
        }
    }
}