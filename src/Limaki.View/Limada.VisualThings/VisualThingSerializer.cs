/*
 * Limada
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

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Visuals;
using Xwt;

namespace Limada.VisualThings {

    public class VisualThingSerializer : ThingIdSerializer {
        private IGraphPair<IVisual, IThing, IVisualEdge, ILink> _visualThingGraph = null;
        
        public virtual IGraphPair<IVisual, IThing, IVisualEdge, ILink> VisualThingGraph {
            get { return _visualThingGraph; }
            set { _visualThingGraph = value; }
        }

        public override IThingGraph Graph {
            get { 
                if (VisualThingGraph !=null) {
                    return VisualThingGraph.Two as IThingGraph;
                }
                return null;
            }
            set { throw new ArgumentException("Use VisualThingGraph!"); }
        }



        private ICollection<IVisual> _visualsCollection = null;
        public virtual ICollection<IVisual> VisualsCollection {
            get {
                if (_visualsCollection == null && VisualThingGraph != null) {
                    _visualsCollection = new Set<IVisual>();
                    //ReadInto(_visualsCollection);
                }
                return _visualsCollection;
            }
            set { _visualsCollection = value; }
        }

        private IGraphSceneLayout<IVisual,IVisualEdge> _layout = null;
        public virtual IGraphSceneLayout<IVisual,IVisualEdge> Layout {
            get { return _layout; }
            set { _layout = value; }
        }



        protected override void ReadThings() {
           ReadInto(VisualsCollection);
           foreach (var visual in VisualsCollection) {
                IThing thing = VisualThingGraph.Get (visual);
                if (thing != null && !ThingCollection.Contains (thing)) {
                    ThingCollection.Add(thing);
                }
            }
        }

        protected virtual void ReadInto(ICollection<IVisual> visuals) {
            foreach (XElement node in Things.Elements()) {
                IVisual visual = ReadVisual(node);
                if (visual != null && !visuals.Contains(visual)) {
                    visuals.Add(visual);
                }
            }
            ICollection<IVisual> remove = new Set<IVisual> ();
            foreach(IVisual visual in visuals) {
                IVisualEdge edge = visual as IVisualEdge;
                if (edge != null) {
                    if (!visuals.Contains(edge.Root) || !visuals.Contains(edge.Leaf))
                        remove.Add (visual);
                }
            }
            foreach (IVisual visual in remove)
                visuals.Remove (visual);
        }

        protected virtual IVisual ReadVisual(XElement node) {
            IThing thing = Read (node);
            
            this.ThingCollection.Add (thing);

            IVisual visual = null;
            if (thing != null) {
                visual = VisualThingGraph.Get (thing);
                if (visual!=null && Layout != null) {
                    Layout.Invoke (visual);
                    int x = (int)ReadInt(node, "x", false);
                    int y = (int)ReadInt(node, "y", false);
                    int w = (int)ReadInt(node, "w", false);
                    int h = (int)ReadInt(node, "h", false);
                    visual.Shape.Location = new Point(x, y);
                    visual.Shape.Size = new Size(w, h);
                }
                
            }
            return visual;
        }

        public virtual XElement Write(IVisual visual) {
            XElement xmlthing = null;

            IThing thing = VisualThingGraph.Get (visual);
            if (thing != null) {
                xmlthing = Write (thing);
                xmlthing.Add( new XAttribute("x", ((int)visual.Location.X).ToString()));
                xmlthing.Add( new XAttribute("y", ((int)visual.Location.Y).ToString()));
                xmlthing.Add( new XAttribute("w", ((int)visual.Size.Width).ToString()));
                xmlthing.Add( new XAttribute("h", ((int)visual.Size.Height).ToString()));
            }

            return xmlthing;
        }

        public virtual void Write(IEnumerable<IVisual> visuals) {
            if (visuals == null) return;
            foreach (IVisual visual in visuals) {
                Write(visual);
            }
        }

        public override void Write(System.IO.Stream s) {
            Write(VisualsCollection);
            if (VisualsCollection.Count > 0) {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create (s)) {
                    Document.Save (writer);
                    writer.Flush ();
                }
            }
        }
    }

}

