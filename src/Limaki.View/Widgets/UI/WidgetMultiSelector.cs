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
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Graphs.Extensions;


namespace Limaki.Widgets.UI {
    public class WidgetMultiSelector:SelectionShape, IKeyAction {
        public WidgetMultiSelector(Func<Scene> sceneHandler, IControl control, ICamera camera) : base(control, camera) {
            this.ShowGrips = false;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        Func<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        bool canStart = false;
        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            Resolved = false;
            canStart = TestSceneHit(e.Location); 
            Exclusive = false;
        }

        public static void Select(Scene scene, IEnumerable<IWidget> selection, ModifierKeys modifiers) {
            bool isLinkKey = (modifiers & ModifierKeys.Shift) != 0;
            Set<IWidget> oldSelected = new Set<IWidget>();
            foreach (IWidget widget in scene.Selected.Elements) {
                oldSelected.Add(widget);
            }

            if ((modifiers & ModifierKeys.Control) != ModifierKeys.Control) {
                scene.Selected.Clear();
            }

            foreach (IWidget widget in selection) {
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


        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            base.OnMouseMoveResolved(e);
            Select (this.Scene, Scene.ElementsIn (this.Shape.BoundsRect), e.Modifiers);
        }

        public override bool HitTest(PointI p) {
            bool result = false;
            if (this.Shape == null) {
                return result;
            }
            result = HitBorder(p);
            return result;
        }

        bool TestSceneHit(PointI p) {
            bool result = true;
            PointI pt = camera.ToSource(p);
            if (Scene.Hovered != null)
                result = !Scene.Hovered.Shape.IsHit(pt, this.HitSize);
            if (result && Scene.Focused != null)
                result = !Scene.Focused.Shape.IsHit(pt, this.HitSize);
            return result;
        }

        protected override void BaseMouseMove(MouseActionEventArgs e) {
            if (canStart&&!Exclusive) {
                canStart = TestSceneHit(e.Location);
            }
            if (canStart) {
                base.BaseMouseMove(e);
                Exclusive = Resolved;
            }
        }
        public override void OnMouseUp(MouseActionEventArgs e) {
            canStart = false;
            base.OnMouseUp(e);
            Exclusive = false;
            IShape oldShape = this.Shape;
            this.Shape = ShapeFactory.Shape(ShapeDataType, PointI.Empty, SizeI.Empty);
            InvalidateShapeOutline(oldShape, ShapeFactory.Shape(ShapeDataType, PointI.Empty, SizeI.Empty));
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

        void IKeyAction.OnKeyDown( KeyActionEventArgs e ) {
            if (e.ModifierKeys != ModifierKeys.Control)
                return;

            if (e.Key == Key.F) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage(Scene.Graph.Fork (Scene.Focused)), e.ModifierKeys);
                }
                e.Handled = true;
            }

            if (e.Key == Key.T) {
                if (Scene.Focused != null) {
                    Select(Scene, Scene.Graph.Foliage(Scene.Graph.Twig(Scene.Focused)), e.ModifierKeys);
                }
                e.Handled = true;
            }

            if (e.Key == Key.D) {
                if (Scene.Focused != null) {
                    var walker = new Walker<IWidget, IEdgeWidget> (Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(walker.Edges(walker.DeepWalk(Scene.Focused,0))),
                           e.ModifierKeys);
                }
                e.Handled = true;
            }

            if (e.Key == Key.E) {
                if (Scene.Focused != null) {
                    var walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(walker.Edges(walker.ExpandWalk(Scene.Focused, 0))),
                           e.ModifierKeys);
                }
                e.Handled = true;

            }

            if (e.Key == Key.W) {
                if (Scene.Focused != null) {
                    var walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(walker.Edges(walker.Walk(Scene.Focused, 0))),
                           e.ModifierKeys);
                }
                e.Handled = true;

            }

            if (e.Key == Key.C) {
                if (Scene.Focused != null) {
                    var walker = new Walker<IWidget, IEdgeWidget>(Scene.Graph);
                    Select(Scene, Scene.Graph.Foliage(walker.Edges(walker.CollapseWalk(Scene.Focused, 0))),
                           e.ModifierKeys);
                }
                e.Handled = true;
            }
        }

        void IKeyAction.OnKeyPress( KeyActionPressEventArgs e ) {}

        void IKeyAction.OnKeyUp( KeyActionEventArgs e ) {}

        #endregion

       
    }
}