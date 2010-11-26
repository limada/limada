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
 * 
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Common;
using Limaki.Common.Collections;
using System.Collections.Generic;
using Limaki.Widgets.Layout;

namespace Limaki.Winform.Widgets {
    public class WidgetMultiSelector:SelectionShape, IKeyAction {
        public WidgetMultiSelector(Handler<Scene> sceneHandler, IWinControl control, ICamera camera) : base(control, camera) {
            ShowGrips = false;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        bool canStart = false;
        public override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            Resolved = false;
            canStart = TestSceneHit(e.Location); 
            Exclusive = false;
        }

        public static void Select(Scene scene, IEnumerable<IWidget> selection) {
            Set<IWidget> oldSelected = new Set<IWidget>();
            foreach (IWidget widget in scene.Selected.Elements) {
                oldSelected.Add(widget);
            }
            if ((Form.ModifierKeys & Keys.Control) != Keys.Control) {
                scene.Selected.Clear();
            }
            foreach (IWidget widget in selection) {
                bool isLinkKey = (Form.ModifierKeys & Keys.Shift) == Keys.Shift;
                bool isLink = widget is IEdgeWidget;
                bool add = (isLinkKey && isLink) || (!isLinkKey && !isLink);
                if (add) {
                    scene.Selected.Add(widget);
                    if (!oldSelected.Contains(widget)) {
                        scene.Commands.Add(new Command<IWidget>(widget));
                    }
                    oldSelected.Remove(widget);
                }
            }
            foreach (IWidget widget in oldSelected) {
                scene.Commands.Add(new Command<IWidget>(widget));
            }            
        }

        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            base.OnMouseMoveResolved(e);
            Select (this.Scene, Scene.ElementsIn (this.Shape.BoundsRect));
            //control.CommandsExecute ();
        }

        public override bool HitTest(Point p) {
            bool result = false;
            if (Shape == null) {
                return result;
            }
            result = HitBorder(p);
            return result;
        }

        bool TestSceneHit(Point p) {
            bool result = true;
            Point pt = camera.ToSource(p);
            if (Scene.Hovered != null)
                result = !Scene.Hovered.Shape.IsHit(pt, this.HitSize);
            if (result && Scene.Focused != null)
                result = !Scene.Focused.Shape.IsHit(pt, this.HitSize);
            return result;
        }
        protected override void BaseMouseMove(MouseEventArgs e) {
            if (canStart&&!Exclusive) {
                canStart = TestSceneHit(e.Location);
            }
            if (canStart) {
                base.BaseMouseMove(e);
                Exclusive = Resolved;
            }
        }
        public override void OnMouseUp(MouseEventArgs e) {
            canStart = false;
            base.OnMouseUp(e);
            Exclusive = false;
            IShape oldShape = this.Shape;
            Shape = ShapeFactory.Shape(ShapeDataType, Point.Empty, Size.Empty);
            InvalidateShapeOutline(oldShape, this.Shape);

        }


        bool _exclusive = false;
        public override bool Exclusive {
            get {
                return _exclusive;
            }
            protected set {
                _exclusive = value;
            }
        }

        #region IKeyAction Member

        void IKeyAction.OnKeyDown(KeyEventArgs e) {
            
            if (e.KeyCode == System.Windows.Forms.Keys.F) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage(Scene.Graph.Fork (Scene.Focused)));
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.T) {
                if (Scene.Focused != null) {
                    Select(Scene, Scene.Graph.Foliage(Scene.Graph.Twig(Scene.Focused)));
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.D) {
                if (Scene.Focused != null) {
                    Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget> (Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(
                        walker.Edges(walker.DeepWalk(Scene.Focused,0))
                        )
                        );
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.E) {
                if (Scene.Focused != null) {
                    Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(
                        walker.Edges(walker.ExpandWalk(Scene.Focused, 0))
                        )
                        );
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.W) {
                if (Scene.Focused != null) {
                    Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(
                        walker.Edges(walker.Walk(Scene.Focused, 0))
                        )
                        );
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.C) {
                if (Scene.Focused != null) {
                    Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(
                        walker.Edges(walker.CollapseWalk(Scene.Focused, 0))
                        )
                        );
                }
            }
        }

        void IKeyAction.OnKeyPress(KeyPressEventArgs e) {}

        void IKeyAction.OnKeyUp(KeyEventArgs e) {}

        #endregion

       
    }
    

}
