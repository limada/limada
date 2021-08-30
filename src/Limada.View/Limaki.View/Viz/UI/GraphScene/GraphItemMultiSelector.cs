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

using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Xwt;
using System;

namespace Limaki.View.Viz.UI.GraphScene {

    public class GraphItemMultiSelector<TItem,TEdge>:SelectorAction, IKeyAction 
    where TEdge:TItem,IEdge<TItem>{
        public GraphItemMultiSelector() : base() {
            this.ShowGrips = false;
            this.Priority = ActionPriorities.SelectionPriority + 50;
        }


        public virtual Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        bool canStart = false;
        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            Resolved = false;
            canStart = TestSceneHit(e.Location); 
            Exclusive = false;
        }

        public static void Select(IGraphScene<TItem, TEdge> scene, IEnumerable<TItem> selection, ModifierKeys modifiers) {
            bool isLinkKey = modifiers.HasFlag(ModifierKeys.Shift);
            var oldSelected = new Set<TItem>();
            foreach (var item in scene.Selected.Elements) {
                oldSelected.Add(item);
            }

            if ((modifiers & ModifierKeys.Control) != ModifierKeys.Control) {
                scene.Selected.Clear();
            }

            foreach (var item in selection) {
                bool isLink = item is TEdge;
                bool add = (isLinkKey && isLink) || (!isLinkKey && !isLink);
                if (add) {
                    scene.Selected.Add(item);
                    if (!oldSelected.Contains(item)) {
                        scene.Requests.Add(new Command<TItem>(item));
                    }
                    oldSelected.Remove(item);
                }
            }
            foreach (var item in oldSelected) {
                scene.Requests.Add(new Command<TItem>(item));
            }            
        }


        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            base.OnMouseMoveResolved(e);
            Select (this.Scene, Scene.ElementsIn (this.Shape.BoundsRect), e.Modifiers);
        }

        public override bool HitTest(Point p) {
            bool result = false;
            if (this.Shape == null) {
                return result;
            }
            result = HitBorder(p);
            return result;
        }

        bool TestSceneHit(Point p) {
            bool result = true;
            var pt = Camera.ToSource(p);
            if (Scene.Hovered != null)
                result = !Scene.ItemShape(Scene.Hovered).IsHit(pt, this.HitSize);
            if (result && Scene.Focused != null)
                result = !Scene.ItemShape(Scene.Focused).IsHit(pt, this.HitSize);
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
            var oldShape = this.Shape;
            this.Shape = ShapeFactory.Shape(ShapeDataType, Point.Zero, Size.Zero,false);
            SelectionRenderer.InvalidateShapeOutline(oldShape, ShapeFactory.Shape(ShapeDataType, Point.Zero, Size.Zero, false));
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

        void IKeyAction.OnKeyPressed( KeyActionEventArgs e ) {
            if (!(e.Modifiers.HasFlag(ModifierKeys.Control) || e.Modifiers.HasFlag(ModifierKeys.Alt)))
                return;

            var key = e.Key.ToUpper();

            if (key == Key.A) {
                
                Select(Scene, Scene.Elements, e.Modifiers);
                
                e.Handled = true;
            }

            if (key == Key.F) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage(Scene.Graph.Fork (Scene.Focused)), e.Modifiers);
                }
                e.Handled = true;
            }

            if (key == Key.T) {
                if (Scene.Focused != null) {
                    Select(Scene, Scene.Graph.Foliage(Scene.Graph.Twig(Scene.Focused)), e.Modifiers);
                }
                e.Handled = true;
            }

            if (key == Key.D) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage (Scene.Graph.Walk ().DeepWalk (Scene.Focused, 0).Edges<TItem, TEdge> ()),
                           e.Modifiers);
                }
                e.Handled = true;
            }

            if (key == Key.E) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage (Scene.Graph.Walk ().ExpandWalk (Scene.Focused, 0).Edges<TItem, TEdge>()),
                           e.Modifiers);
                }
                e.Handled = true;

            }

            if (key == Key.W) {
                if (Scene.Focused != null) {
                    Select (Scene, Scene.Graph.Foliage (Scene.Graph.Walk ().Walk (Scene.Focused, 0).Edges<TItem, TEdge>()),
                           e.Modifiers);
                }
                e.Handled = true;

            }

            if (key == Key.K) {
                if (Scene.Focused != null) {
                    Select(Scene, Scene.Graph.Foliage(Scene.Graph.Walk ().CollapseWalk(Scene.Focused, 0).Edges<TItem, TEdge>()),
                           e.Modifiers);
                }
                e.Handled = true;
            }
        }

        void IKeyAction.OnKeyReleased( KeyActionEventArgs e ) {}

        #endregion

        public override bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
            }
            return base.Check();
        }

    }
}