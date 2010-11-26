/*
 * Limada
 * Version 0.08
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
using System.Xml;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Widgets;

namespace Limada.View {
#if ! SILVERLIGHT
    public class WidgetThingSerializer : ThingSerializer {
        private IGraphPair<IWidget, IThing, IEdgeWidget, ILink> _widgetThingGraph = null;
        public virtual IGraphPair<IWidget, IThing, IEdgeWidget, ILink> WidgetThingGraph {
            get { return _widgetThingGraph; }
            set { _widgetThingGraph = value; }
        }

        public override IThingGraph Graph {
            get { 
                if (WidgetThingGraph !=null) {
                    return WidgetThingGraph.Two as IThingGraph;
                }
                return null;
            }
            set {throw new ArgumentException ("Use WidgetThingGraph!");}
        }



        private ICollection<IWidget> _widgetCollection = null;
        public virtual ICollection<IWidget> WidgetCollection {
            get {
                if (_widgetCollection == null && WidgetThingGraph != null) {
                    _widgetCollection = new Set<IWidget>();
                    //ReadInto(_widgetCollection);
                }
                return _widgetCollection;
            }
            set { _widgetCollection = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }



        protected override void ReadThings() {
           ReadInto(WidgetCollection);
           foreach (IWidget widget in WidgetCollection) {
                IThing thing = WidgetThingGraph.Get (widget);
                if (thing != null && !ThingCollection.Contains (thing)) {
                    ThingCollection.Add(thing);
                }
            }
        }

        protected virtual void ReadInto(ICollection<IWidget> widgets) {
            foreach (XmlElement node in Things) {
                IWidget widget = ReadWidget(node);
                if (widget != null && !widgets.Contains(widget)) {
                    widgets.Add(widget);
                }
            }
            ICollection<IWidget> remove = new Set<IWidget> ();
            foreach(IWidget widget in widgets) {
                IEdgeWidget edge = widget as IEdgeWidget;
                if (edge != null) {
                    if (!widgets.Contains(edge.Root) || !widgets.Contains(edge.Leaf))
                        remove.Add (widget);
                }
            }
            foreach (IWidget widget in remove)
                widgets.Remove (widget);
        }

        protected virtual IWidget ReadWidget(XmlElement node) {
            IThing thing = Read (node);
            IWidget widget = null;
            if (thing != null) {
                widget = WidgetThingGraph.Get (thing);
                if (Layout != null) {
                    Layout.Invoke (widget);
                    int x = (int)ReadInt(node, "x", false);
                    int y = (int)ReadInt(node, "y", false);
                    int w = (int)ReadInt(node, "w", false);
                    int h = (int)ReadInt(node, "h", false);
                    //XmlNodeReader reader = new XmlNodeReader(node);
                    //try {
                    //    reader.MoveToAttribute ("x");
                    //    int x = reader.ReadContentAsInt ();
                    //    reader.MoveToAttribute ("y");
                    //    int y = reader.ReadContentAsInt ();
                    //    reader.MoveToAttribute("w");
                    //    int w = reader.ReadContentAsInt();
                    //    reader.MoveToAttribute("h");
                    //    int h = reader.ReadContentAsInt();

                    //} catch {}
                    widget.Shape.Location = new PointI(x, y);
                    widget.Shape.Size = new SizeI(w, h);
                }
                
            }
            return widget;
        }

        public virtual XmlElement Write(IWidget widget) {
            XmlElement xmlthing = null;

            IThing thing = WidgetThingGraph.Get (widget);
            if (thing != null) {
                xmlthing = Write (thing);
                xmlthing.SetAttribute("x", widget.Location.X.ToString());
                xmlthing.SetAttribute("y", widget.Location.Y.ToString());
                xmlthing.SetAttribute("w", widget.Size.Width.ToString());
                xmlthing.SetAttribute("h", widget.Size.Height.ToString());
            }

            return xmlthing;
        }

        public virtual void Write(IEnumerable<IWidget> widgets) {
            if (widgets == null) return;
            foreach (IWidget widget in widgets) {
                Write(widget);
            }
        }

        public override void Write(System.IO.Stream s) {
            Write (WidgetCollection);
            Document.Save (s);
        }
    }
#endif
}