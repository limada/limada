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
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using System;
using Limaki.Common.Collections;
using Limaki.Common;
using Limaki.Graphs.Extensions;
using Xwt;

namespace Limaki.Visuals {

    public class Scene : IGraphScene<IVisual,IVisualEdge>, IVisual, IComposite<IVisual> {

        #region Graph

        private IGraph<IVisual, IVisualEdge> _graph = null;
        public IGraph<IVisual,IVisualEdge> Graph {
            get {
                if (_graph == null) {
                    _graph = new VisualGraph();
                    _spatialIndex = null;
                    State.Hollow = true;
                }
                return _graph;
            }
            set {
                Clear ();
                _graph = value;
                State.Hollow = value != null;
            }
        }

        public bool ChangeEdge(IVisualEdge edge, IVisual target, bool asRoot) {
            bool result = true;
            // test if there is a loop:
            if (target is IVisualEdge) {
                foreach (var visual in this.Twig(edge)) {
                    if (visual == target) {
                        return false;
                    }
                }
            }
            Graph.ChangeEdge(edge, target, asRoot);
            if(result)
                State.Dirty = true;
            return result;
        }

        /// <summary>
        /// gives back the visuals wich are affected by a change
        /// of source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<IVisualEdge> Twig(IVisual source) {
            return Graph.Twig(source);
        }

        protected State _state = default(State);
        public virtual State State { get { return _state ?? (_state = new State { Hollow = true }); } }

        #endregion

        #region IVisual Member

        private RectangleShape _shape = null;
        public virtual IShape Shape {
            get {
                if (_shape == null)
                    _shape = new RectangleShape();

                if (SpatialIndex.BoundsDirty) {
                    _noHit = emptyPoint;
                }

                _shape.Data = SpatialIndex.Bounds;
                if (_shape.Size.Height < 0 || _shape.Size.Width < 0)
                    _shape.Size = Size.Zero;
                
                this.Size = _shape.Size;
                this.Location = _shape.Location;
                return _shape;
            }
            set { }
        }

        public virtual Size Size { get; set; }
        public virtual Point Location { get; set; }
        public virtual IStyleGroup Style { get; set; }

        object IVisual.Data {
            get { return this.Graph; }
            set {
                if (value is IGraph<IVisual,IVisualEdge>)
                    this.Graph = (IGraph<IVisual, IVisualEdge>)value;
            }
        }

        #endregion

        #region IComposite<IVisual> Member

        public IEnumerable<IVisual> Elements {
            get {
                return Graph;
                //foreach (var visual in Graph) {
                //    if (!(visual is IVisualEdge)) {
                //        yield return visual;
                //    }
                //}
                //foreach (var visualEdge in Graph.Edges()) {
                //    yield return visualEdge;
                //}
            }
        }

        public void Add(IVisual visual) {
            if (visual is IVisualEdge) {
                Graph.Add((IVisualEdge)visual);
            } else {
                Graph.Add(visual);
            }
            AddBounds (visual);
        }

        public virtual bool Remove(IVisual visual) {
            bool result = false;
            RemoveBounds (visual);
            if (visual is IVisualEdge) {
                result = Graph.Remove((IVisualEdge)visual);
            } else {
                result = Graph.Remove(visual);
            }
            Selected.Remove (visual);
            if (Focused == visual)
                Focused = null;
            if (Hovered == visual)
                Hovered = null;
            return result;
        }


        public virtual bool Contains(IVisual item) {
            if (item is IVisualEdge)
                return this.Contains((IVisualEdge)item);
            else
                return this.Graph.Contains(item);
        }

        public virtual bool Contains(IVisualEdge item) {
            return this.Graph.Contains(item);
        }

        public void ClearView() {
            Selected.Clear();
            Focused = null;
            Hovered = null;
            _spatialIndex = null;
            Markers = null;
            Requests.Clear ();
            this.Size = Size.Zero;
            this.Location = Point.Zero;
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

        private ISpatialIndex<IVisual> _spatialIndex = null;
        public ISpatialIndex<IVisual> SpatialIndex {
            get {
                if(_spatialIndex == null) {
                    // ensure we have a graph, 
                    // cause if graph==null, _spatialIndex will be set to null
                    bool dummy = this.Graph.IsReadOnly;
                    _spatialIndex = new QuadTreeIndex ();
                    _spatialIndex.AddRange(this.Elements);
                }
                return _spatialIndex;
            }
            set {_spatialIndex = value;}
        }
        
        public IEnumerable<IVisual> ElementsIn(Rectangle clipBounds) {
            return SpatialIndex.Query (clipBounds);
        }

        public IEnumerable<IVisual> ElementsIn(Rectangle clipBounds, ZOrder zOrder) {
            return SpatialIndex.Query(clipBounds, zOrder);
        }

        #endregion
        
        #region Action-Handling (selection, hit-tests, commandQueue)

        private IVisual _focused = null;
        public virtual IVisual Focused {
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

        
        public Action<IGraphScene<IVisual, IVisualEdge>, IVisual> FocusChanged {get;set;}

        private VisualComposite<Set<IVisual>> _selectedItems = null;
        public virtual IComposite<IVisual> Selected {
            get {
                if (_selectedItems == null) {
                    _selectedItems = new VisualComposite<Set<IVisual>>(new Set<IVisual>());
                }
                return _selectedItems;
            }
            set { _selectedItems = value as VisualComposite<Set<IVisual>>; }
        }

       
        public virtual IVisual Hovered {get;set;}
          

        static Point emptyPoint = new Point (int.MinValue, int.MinValue);
        private Point _noHit = emptyPoint;

		public Point NoHit {get{return _noHit;}}
		
		protected IVisual TestHit(Point p, int hitSize, HitTest hitTest) {
            if (p==_noHit) {
                return null;
            }
            if ((Focused != null) && hitTest(Focused, p, hitSize)) {
                return Focused;
            }
            if ((Hovered != null) && hitTest(Hovered, p, hitSize)) {
                return Hovered;
            }
            int halfSize = hitSize / 2;
            var hitBounds = new Rectangle(p.X - halfSize, p.Y - halfSize, hitSize, hitSize);
            foreach (var visual in SpatialIndex.Query(hitBounds,ZOrder.EdgesFirst)) {
                if ((visual == Focused) || (visual == Hovered)) {
                    continue;
                }
                if (hitTest(visual, p, hitSize)) {
                    return visual;
                }
            }

            _noHit = p;
            return null;
        }

        public IVisual Hit(Point p, int hitSize) {
            HitTest hitTest = delegate(IVisual w, Point ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }

        public IVisual HitBorder(Point p, int hitSize) {
            HitTest hitTest = delegate(IVisual w, Point ap, int ahitSize) {
                if (w.Shape == null) return false;
                return w.Shape.IsBorderHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }

        public virtual IShape ItemShape(IVisual item) {
            return item.Shape; 
        }

        private ICollection<ICommand<IVisual>> _commands = new List<ICommand<IVisual>>();
        public ICollection<ICommand<IVisual>> Requests {
            get { return _commands; }
            set { _commands = value; }
        }

        #endregion

        #region Bounds-Handling

        public virtual void UpdateBounds(IVisual visual, Rectangle invalid) {
            SpatialIndex.Update (invalid, visual);
        }

        public virtual void RemoveBounds(IVisual visual) {
            SpatialIndex.Remove (visual);
        }
        public virtual void AddBounds(IVisual visual) {
            SpatialIndex.Add (visual);
        }

        #endregion

        #region Marker-Handling

        private IMarkerFacade<IVisual, IVisualEdge> _markers = null;
        public IMarkerFacade<IVisual, IVisualEdge> Markers {
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
    
    public delegate bool HitTest(IVisual w, Point p, int hitSize);
}
