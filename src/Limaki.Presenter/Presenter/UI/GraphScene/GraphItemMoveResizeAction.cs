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


using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Presenter.Layout;
using System;


namespace Limaki.Presenter.UI {
    /// <summary>
    /// Moves or resizes an item
    /// </summary>
    public class GraphItemMoveResizeAction<TItem, TEdge> : MoveResizeAction, ICheckable
        where TEdge:TItem,IEdge<TItem> {

        public GraphItemMoveResizeAction() {
            Priority = ActionPriorities.SelectionPriority - 10;
            FocusFilter = e => e;
        }
        public Get<IGraphScene<TItem, TEdge>> SceneHandler;
        public virtual IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        public Func<TItem, TItem> FocusFilter { get; set; }
        public virtual TItem GraphItem {
            get {
                IGraphScene<TItem, TEdge> scene = SceneHandler();
                if (scene == null) return default(TItem);
                return FocusFilter(scene.Focused);
            }
            set { }
        }

        // TODO: Shape is used to draw the grips; find another solution to 
        // get rid of ShapeSelectionBase-Inheritance
        public override IShape Shape {
            get {
                TItem item = this.GraphItem;
                if (item != null)
                    return Scene.ItemShape(item);
                else
                    return null;
            }
            set { }
        }

        public override bool HitTest(PointI p) {
            PointI sp = Camera.ToSource(p);
            TItem item = this.GraphItem;
            bool result = ((item != null) && 
                (Scene.ItemShape(item)!= null) &&
                (Scene.ItemShape(item).IsBorderHit(sp, HitSize)));
            Anchor anchor = Anchor.None;
            if (result && ShowGrips) {
                anchor = HitAnchor(p);
                if (!Resolved) {
                    hitAnchor = anchor;
                }
                if (!(item is TEdge))
                    DeviceCursor.SetCursor (anchor, result);
            }

            return result;
        }


        public override void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            TItem item = this.GraphItem;
            if (item != null && !(item is TEdge)) {
                base.OnMouseDown(e);
            }
            if (Resolved != ShowGrips) {
                ShowGrips = Resolved;
                if (_selectionRenderer != null)
                    this.SelectionRenderer.ShowGrips = Resolved;
            }
        }

        protected virtual void UpdateGrip(IShape shape) {}
        
        bool hideGrips = false;
        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            if (HitTest(e.Location)) {
                ShowGrips = true;
                if (_selectionRenderer != null)
                    this.SelectionRenderer.ShowGrips = true;
                hideGrips = true;
            } else if (hideGrips) {
                ShowGrips = false;
                if (_selectionRenderer != null)
                    this.SelectionRenderer.ShowGrips = false;
                hideGrips = false;
                DeviceCursor.RestoreCursor ();
            }
        }

        protected virtual bool CheckResizing() {
            return Resolved && resizing &&
                   this.Camera.Matrice.Elements[0] > 0.01f &&
                   this.Camera.Matrice.Elements[3] > 0.01f;
        }

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            TItem item = this.GraphItem;
            if (!(item is TEdge) && (moving || resizing)) {
                ShowGrips = true;

                Resolved = (Resolved) && (item != null);
                if (Resolved) {
                    ICommand<TItem> command = null;
                    if (moving) {
                        RectangleI delta = Camera.ToSource(
                            RectangleI.FromLTRB(e.Location.X, e.Location.Y,
                                                LastMousePos.X, LastMousePos.Y));

                        foreach(TItem selected in Scene.Selected.Elements) {
                            if (!(selected is TEdge)) {
                                Scene.Requests.Add (
                                    new MoveByCommand<TItem> (selected, Scene.ItemShape, delta.Size));
                                foreach (TItem twig in Scene.Graph.Twig (selected)) {
                                    Scene.Requests.Add(
                                        new LayoutCommand<TItem>(twig, LayoutActionType.Justify));
                                }
                            }
                        }

                    } else if (CheckResizing()) {
                        RectangleI rect = Camera.ToSource(
                            RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                                LastMousePos.X, LastMousePos.Y));

                        var shape = Scene.ItemShape (item);
                        // do not normalize Links!!
                        if (!(shape is IEdgeShape)) {
                            rect = ShapeUtils.NormalizedRectangle(rect);
                        }
                        command = new ResizeCommand<TItem>(item, Scene.ItemShape, rect);
                        Scene.Requests.Add(command);

                        foreach (TItem twig in Scene.Graph.Twig(item)) {
                            Scene.Requests.Add(new LayoutCommand<TItem>(twig, LayoutActionType.Justify));
                        }
                    }
                    
                    // saving current mouse position to be used for next dragging
                    this.LastMousePos = e.Location;
                }
            } else {
                Resolved = false;
            }


        }

        public override bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem,TEdge>));
            }
            return base.Check();
        }
    }
}