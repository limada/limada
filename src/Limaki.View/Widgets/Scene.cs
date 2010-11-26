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

using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using System;
using Limaki.Common.Collections;
using Limaki.Common;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Graphs.Extensions;

namespace Limaki.Widgets {
    public class Scene : IWidget, IComposite<IWidget> {

        #region Graph


        private IGraph<IWidget, IEdgeWidget> _graph = null;
        public IGraph<IWidget,IEdgeWidget> Graph {
            get {
                if (_graph == null) {
                    _graph = new WidgetGraph();
                    _spatialIndex = null;
                }
                return _graph;
            }
            set {
                Clear ();
                _graph = value;
            }
        }

        public bool ChangeEdge(IEdgeWidget edge, IWidget target, bool asRoot) {
            bool result = true;
            // test if there is a loop:
            if (target is IEdgeWidget) {
                foreach (IWidget widget in this.Twig(edge)) {
                    if (widget == target) {
                        return false;
                    }
                }
            }
            Graph.ChangeEdge(edge, target, asRoot);
            return result;
        }

        /// <summary>
        /// gives back the widgets wich are affected by a change
        /// of source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<IEdgeWidget> Twig(IWidget source) {
            return Graph.Twig(source);
        }

        #endregion

        #region IWidget Member

        private RectangleShape _shape = null;
        public virtual IShape Shape {
            get {
                if (_shape == null)
                    _shape = new RectangleShape();

                if (SpatialIndex.BoundsDirty) {
                    noHit = emptyPoint;
                }

                _shape.Data = SpatialIndex.Bounds;
                if (_shape.Size.Height < 0 || _shape.Size.Width < 0)
                    _shape.Size = SizeI.Empty;
                
                this.Size = _shape.Size;
                this.Location = _shape.Location;
                return _shape;
            }
            set { }
        }

        private SizeI _size = SizeI.Empty;
        public virtual SizeI Size {
            get { return _size; }
            set { _size = value; }
        }

        private PointI _location = PointI.Empty;
        public virtual PointI Location {
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
                return Graph;
                //foreach (IWidget widget in Graph) {
                //    if (!(widget is IEdgeWidget)) {
                //        yield return widget;
                //    }
                //}
                //foreach (IWidget widget in Graph.Edges()) {
                //    yield return widget;
                //}
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
            Selected.Remove (widget);
            if (Focused == widget)
                Focused = null;
            if (Hovered == widget)
                Hovered = null;
            return result;
        }


        public virtual bool Contains(IWidget item) {
            if (item is IEdgeWidget)
                return this.Contains((IEdgeWidget)item);
            else
                return this.Graph.Contains(item);
        }

        public virtual bool Contains(IEdgeWidget item) {
            return this.Graph.Contains(item);
        }

        public void ClearView() {
            Selected.Clear();
            Focused = null;
            Hovered = null;
            _spatialIndex = null;
            Markers = null;
            Commands.Clear ();
            this.Size = SizeI.Empty;
            this.Location = PointI.Empty;
            this._shape = null;
        }

        public void Clear() {
            _graph = null;
            ClearView ();
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
                    // ensure we have a graph, 
                    // cause if graph==null, _spatialIndex will be set to null
                    bool dummy = this.Graph.IsReadOnly;
                    _spatialIndex = new QuadTreeIndex ();
                    _spatialIndex.Fill(this.Elements);
                }
                return _spatialIndex;
            }
            set {_spatialIndex = value;}
        }
        
        public IEnumerable<IWidget> ElementsIn(RectangleS clipBounds) {
            return SpatialIndex.Query (clipBounds);
        }

        public IEnumerable<IWidget> ElementsIn(RectangleS clipBounds, ZOrder zOrder) {
            return SpatialIndex.Query(clipBounds, zOrder);
        }

        #endregion
        
        #region Action-Handling (selection, hit-tests, commandQueue)



        private IWidget _focused = null;
        public virtual IWidget Focused {
            get { return _focused; }
            set {
                if (value != null) {
                    if (!this.Contains(value))
                        this.Add (value);
                    Selected.Add (value);
                }
                if (_focused != value) {
                    _focused = value;
                    if (FocusChanged != null) {
                        FocusChanged (this, _focused);
                    }
                }
            }
        }

        private Action<Scene, IWidget> _focusChanged = null;
        public Action<Scene, IWidget> FocusChanged {
            get { return _focusChanged; }
            set { _focusChanged = value; }
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



        static PointI emptyPoint = new PointI (int.MinValue, int.MinValue);
        private PointI noHit = emptyPoint;

        protected IWidget TestHit(PointI p, int hitSize, HitTest hitTest) {
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
            RectangleI hitBounds = new RectangleI(p.X - halfSize, p.Y - halfSize, hitSize, hitSize);
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

        public IWidget Hit(PointI p, int hitSize) {
            HitTest hitTest = delegate(IWidget w, PointI ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }

        public IWidget HitBorder(PointI p, int hitSize) {
            HitTest hitTest = delegate(IWidget w, PointI ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsBorderHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }


        private List<ICommand<IWidget>> _commands = new List<ICommand<IWidget>>();
        public List<ICommand<IWidget>> Commands {
            get { return _commands; }
            set { _commands = value; }
        }

        #endregion

        #region Bounds-Handling

        public virtual void UpdateBounds(IWidget widget, RectangleI invalid) {
            SpatialIndex.Update (invalid, widget);
        }

        public virtual void RemoveBounds(IWidget widget) {
            SpatialIndex.Remove (widget);
        }
        public virtual void AddBounds(IWidget widget) {
            SpatialIndex.Add (widget);
        }

        #endregion

        #region Marker-Handling

        private IMarkerFacade<IWidget, IEdgeWidget> _markers = null;
        public IMarkerFacade<IWidget, IEdgeWidget> Markers {
            get { return _markers; }
            set { _markers = value; }
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


    public delegate bool HitTest(IWidget w, PointI p, int hitSize);
}
