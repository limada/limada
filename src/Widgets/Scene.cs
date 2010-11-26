/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Widgets {
    public class Scene:IWidget {

        private IStyle _style;
        
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

		///<directed>True</directed>
        private SceneGraph _graph = null;
        ///<directed>True</directed>
        public SceneGraph Graph {
            get {
                if (_graph==null) {
                    _graph = new SceneGraph();
                    boundsDirty = true;
                }
                return _graph;
            }
            set { 
                _graph = value;
                boundsDirty = true;
        }
        }

        public IEnumerable<IWidget> Widgets {
            get {
                foreach (IWidget widget in Graph) {
                    if (!(widget is ILinkWidget)) {
                        yield return widget;
                    }
                }
                foreach (IWidget widget in Graph.Edges()) {
                    yield return widget;
                }
            }
        }


        private RectangleShape _shape= new RectangleShape();
        bool boundsDirty = true;
        public virtual IShape Shape {
            get {
                // TODO: this is crazy! make better thing here
                if (boundsDirty) {
                    ReCalculateBounds ();
                }
                
                this.Size = _shape.Size;
                this.Location = _shape.Location;
                return _shape;
            }
            set {}
        }


        public virtual void ReCalculateBounds() {
            int h = 0;
            int w = 0;

            foreach (IWidget widget in Widgets) {
                Rectangle bounds = widget.Shape.BoundsRect;
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w) w = r;
                if (b > h) h = b;

            }
            _shape.Data = new Rectangle(0,0,w,h);
            boundsDirty = false;
        }

        public virtual void RemoveBounds(IWidget widget) {
            boundsDirty = true;
        }
        public virtual void AddBounds(IWidget widget) {
            boundsDirty = true;
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

        public void Add(IWidget widget) {
            if (widget is ILinkWidget) {
                Graph.Add ((ILinkWidget) widget);
            } else {
                Graph.Add (widget);
            }
            boundsDirty = true;
        }
        public void Remove ( IWidget widget ) {
            if ( widget is ILinkWidget ) {
                Graph.Remove((ILinkWidget) widget);
            } else {
                Graph.Remove(widget);
            }
            boundsDirty = true;
        }
        public bool ChangeLink(ILinkWidget link, IWidget target, bool asRoot) {
            bool result = true;
            // test if there is a loop:
            if (target is ILinkWidget) {
               foreach(IWidget widget in this.AffectedByChange(link)) {
                   if (widget == target) {
                       return false;
                   }
               }
            }
            IWidget oldTarget = null;
            if (asRoot) {
                oldTarget = link.Root;
                link.Root = target;
            } else {
                oldTarget = link.Leaf;
                link.Leaf = target;
            }
            Graph.ChangeEdge (link, oldTarget, target);
            boundsDirty = true;
            return result;
        }

        private IWidget _selected = null;
        public virtual IWidget Selected {
            get { return _selected; }
            set { _selected = value; }
        }

		private IWidget _hovered = null;
        public virtual IWidget Hovered {
            get { return _hovered; }
            set { _hovered = value; }
        }

        public List<ICommand<IWidget>> Commands = new List<ICommand<IWidget>>();

        protected IWidget TestHit(Point p, int hitSize, HitTest hitTest) {
            if ((Selected != null) && hitTest(Selected, p, hitSize)) {
                return Selected;
            }
            if ((Hovered != null) && hitTest(Hovered, p, hitSize)) {
                return Hovered;
            }
            foreach (IWidget widget in Widgets) {
                if ((widget == Selected) || (widget == Hovered)) {
                    continue;
                }
                if (hitTest(widget, p, hitSize)) {
                    return widget;
                }
            }

            return null; 
        }

        public IWidget Hit(Point p, int hitSize) {
            HitTest hitTest = delegate ( IWidget w, Point ap, int ahitSize ) {
                                  return w.Shape.IsHit (ap, ahitSize);
                              };
           return TestHit(p,hitSize,hitTest);
        }

        public IWidget HitBorder(Point p, int hitSize) {
            HitTest hitTest = delegate(IWidget w, Point ap, int ahitSize) {
                return w.Shape.IsBorderHit(ap, ahitSize);
            };
            return TestHit(p, hitSize, hitTest);
        }


        /// <summary>
        /// gives back the widgets wich are affected by a change
        /// of source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>

        public IEnumerable<IWidget> AffectedByChange(IWidget source) {
            // preorder-traversal; could be changed to Graph.PreorderEdges(source);
            foreach (ILinkWidget widget in Graph.Edges(source)) {
                yield return widget;
                foreach (ILinkWidget link in AffectedByChange(widget)) {
                    yield return link;
                }
            }
        }

        object IWidget.Data {
            get { return this.Graph; }
            set {
                if (value is SceneGraph) {
                    this.Graph = (SceneGraph)value;
                }
            }
        }
   }


    public delegate bool HitTest(IWidget w, Point p, int hitSize);

    public class LocationList : SortedMultiDictionary<Point, IWidget> {
        public LocationList():base(new PointComparer()){}
    }
}