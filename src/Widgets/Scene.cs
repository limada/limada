/*
 * Limaki 
 * Version 0.07
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

using System.Collections.Generic;
using System.Drawing;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using System;
using Limaki.Common.Collections;
using Limaki.Common;
using Limaki.Drawing.Indexing.QuadTrees;

namespace Limaki.Widgets {
    public class Scene : IWidget, IComposite<IWidget> {

        #region Graph


        private IGraph<IWidget, IEdgeWidget> _graph = null;
        public IGraph<IWidget,IEdgeWidget> Graph {
            get {
                if (_graph == null) {
                    _graph = new Graph<IWidget,IEdgeWidget>();
                    _spatialIndex = null;
                }
                return _graph;
            }
            set {
                Clear ();
                _graph = value;
            }
        }

        public bool ChangeLink(IEdgeWidget edge, IWidget target, bool asRoot) {
            bool result = true;
            // test if there is a loop:
            if (target is IEdgeWidget) {
                foreach (IWidget widget in this.AffectedByChange(edge)) {
                    if (widget == target) {
                        return false;
                    }
                }
            }
            IWidget oldTarget = null;
            if (asRoot) {
                oldTarget = edge.Root;
            } else {
                oldTarget = edge.Leaf;
            }
            Graph.ChangeEdge(edge, oldTarget, target);
            return result;
        }

        /// <summary>
        /// gives back the widgets wich are affected by a change
        /// of source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<IEdgeWidget> AffectedByChange(IWidget source) {
             //preorder-traversal; could be changed to Graph.PreorderEdges(source);
            //foreach (IEdgeWidget widget in Graph.Edges(source)) {
            //    yield return widget;
            //    foreach (IEdgeWidget edge in AffectedByChange(widget)) {
            //        yield return edge;
            //    }
            //}
            return Graph.Twig(source);
        }

        #endregion

        #region IWidget Member

        private RectangleShape _shape = new RectangleShape();
        public virtual IShape Shape {
            get {
                if (SpatialIndex.BoundsDirty) {
                    noHit = emptyPoint;
                }
                _shape.Data = SpatialIndex.Bounds;
                
                this.Size = _shape.Size;
                this.Location = _shape.Location;
                return _shape;
            }
            set { }
        }

        private Size _size = Size.Empty;
        public virtual Size Size {
            get { return _size; }
            set { _size = value; }
        }

        private Point _location = Point.Empty;
        public virtual Point Location {
            get { return _location; }
            set { _location = value; }
        }

        private IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        object IWidget.Data {
            get { return this.Graph; }
            set {
                if (value is IGraph<IWidget,IEdgeWidget>)
                    this.Graph = (IGraph<IWidget, IEdgeWidget>)value;
            }
        }

        #endregion

        #region IComposite<IWidget> Member

        public IEnumerable<IWidget> Elements {
            get {
                foreach (IWidget widget in Graph) {
                    if (!(widget is IEdgeWidget)) {
                        yield return widget;
                    }
                }
                foreach (IWidget widget in Graph.Edges()) {
                    yield return widget;
                }
            }
        }

        public void Add(IWidget widget) {
            if (widget is IEdgeWidget) {
                Graph.Add((IEdgeWidget)widget);
            } else {
                Graph.Add(widget);
            }
            AddBounds (widget);
        }

        public virtual bool Remove(IWidget widget) {
            bool result = false;
			RemoveBounds (widget);
            if (widget is IEdgeWidget) {
                result = Graph.Remove((IEdgeWidget)widget);
            } else {
                result = Graph.Remove(widget);
            }
            return result;
        }


        public bool Contains(IWidget item) {
            return this.Graph.Contains(item);
        }

        public void Clear() {
            _graph = null;
            Selected.Clear ();
            Focused = null;
            Hovered = null;
            _spatialIndex = null;
        }

        public virtual int Count {
            get { return Graph.Count; }
        }
        #endregion
        
        #region Geo-Location

        const bool useQuadTree = true;
        
        public void ClearSpatialIndex() {
            SpatialIndex = null;
        }
        private SpatialIndex _spatialIndex = null;
        public SpatialIndex SpatialIndex {
            get {
                if(_spatialIndex == null) {
                    _spatialIndex = new QuadTreeIndex ();
                    _spatialIndex.Fill(this.Elements);
                }
                return _spatialIndex;
            }
            set {_spatialIndex = value;}
        }
        
        public IEnumerable<IWidget> ElementsIn(RectangleF clipBounds) {
            return SpatialIndex.Query (clipBounds);
        }
        #endregion
        
        #region Action-Handling (selection, hit-tests, commandQueue)

        private IWidget _selected = null;
        public virtual IWidget Focused {
            get { return _selected; }
            set { _selected = value; }
        }

        private WidgetComposite<Set<IWidget>> _selectedItems = null;
        public virtual WidgetComposite<Set<IWidget>> Selected {
            get {
                if (_selectedItems == null) {
                    _selectedItems = new WidgetComposite<Set<IWidget>>(new Set<IWidget>());
                }
                return _selectedItems;
            }
            set { _selectedItems = value; }
        }

        private IWidget _hovered = null;
        public virtual IWidget Hovered {
            get { return _hovered; }
            set { _hovered = value; }
        }

        static Point emptyPoint = new Point (int.MinValue, int.MinValue);
        private Point noHit = emptyPoint;

        protected IWidget TestHit(Point p, int hitSize, HitTest hitTest) {
            if (p==noHit) {
                return null;
            }
            if ((Focused != null) && hitTest(Focused, p, hitSize)) {
                return Focused;
            }
            if ((Hovered != null) && hitTest(Hovered, p, hitSize)) {
                return Hovered;
            }
            int halfSize = hitSize / 2;
            Rectangle hitBounds = new Rectangle(p.X - halfSize, p.Y - halfSize, hitSize, hitSize);
            foreach (IWidget widget in SpatialIndex.Query(hitBounds)) {
                if ((widget == Focused) || (widget == Hovered)) {
                    continue;
                }
                if (hitTest(widget, p, hitSize)) {
                    return widget;
                }
            }

            noHit = p;
            return null;
        }

        public IWidget Hit(Point p, int hitSize) {
            HitTest hitTest = delegate(IWidget w, Point ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }

        public IWidget HitBorder(Point p, int hitSize) {
            HitTest hitTest = delegate(IWidget w, Point ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsBorderHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }


        public List<ICommand<IWidget>> Commands = new List<ICommand<IWidget>>();

        #endregion

        #region Bounds-Handling

        public virtual void ReCalculateBounds() {

        }

        public virtual void UpdateBounds(IWidget widget, Rectangle invalid) {
            SpatialIndex.Update (invalid, widget);
        }

        public virtual void RemoveBounds(IWidget widget) {
            SpatialIndex.Remove (widget);
        }
        public virtual void AddBounds(IWidget widget) {
            SpatialIndex.Add (widget);
        }

        #endregion


        #region ICloneable Member

        //object ICloneable.Clone() {
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //#endregion

        //#region IDisposable Member

        //void IDisposable.Dispose() {
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        #endregion
    }


    public delegate bool HitTest(IWidget w, Point p, int hitSize);
}
