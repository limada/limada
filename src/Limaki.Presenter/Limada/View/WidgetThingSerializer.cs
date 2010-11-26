/*
 * Limada
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
using System.Xml.Linq;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Widgets;

namespace Limada.View {

    public class WidgetThingSerializer : ThingIdSerializer {
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

        private IGraphLayout<IWidget,IEdgeWidget> _layout = null;
        public virtual IGraphLayout<IWidget,IEdgeWidget> Layout {
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
            foreach (XElement node in Things.Elements()) {
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

        protected virtual IWidget ReadWidget(XElement node) {
            IThing thing = Read (node);
            
            this.ThingCollection.Add (thing);

            IWidget widget = null;
            if (thing != null) {
                widget = WidgetThingGraph.Get (thing);
                if (widget!=null && Layout != null) {
                    Layout.Invoke (widget);
                    int x = (int)ReadInt(node, "x", false);
                    int y = (int)ReadInt(node, "y", false);
                    int w = (int)ReadInt(node, "w", false);
                    int h = (int)ReadInt(node, "h", false);
                    widget.Shape.Location = new PointI(x, y);
                    widget.Shape.Size = new SizeI(w, h);
                }
                
            }
            return widget;
        }

        public virtual XElement Write(IWidget widget) {
            XElement xmlthing = null;

            IThing thing = WidgetThingGraph.Get (widget);
            if (thing != null) {
                xmlthing = Write (thing);
                xmlthing.Add( new XAttribute("x", widget.Location.X.ToString()));
                xmlthing.Add( new XAttribute("y", widget.Location.Y.ToString()));
                xmlthing.Add( new XAttribute("w", widget.Size.Width.ToString()));
                xmlthing.Add( new XAttribute("h", widget.Size.Height.ToString()));
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
            Write(WidgetCollection);
            if (WidgetCollection.Count > 0) {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create (s)) {
                    Document.Save (writer);
                    writer.Flush ();
                }
            }
        }
    }

}

