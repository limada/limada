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
using System.Globalization;
using System.Xml.Linq;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Graphs;
using System.Linq;
using Limaki.View;
using Limaki.View.Visuals;
using Xwt;

namespace Limada.View.VisualThings {

    public class VisualThingXmlSerializer : ThingXmlIdSerializer {
        
        public virtual IGraphPair<IVisual, IThing, IVisualEdge, ILink> VisualThingGraph { get; set; }

        public override IThingGraph Graph {
            get { 
                if (VisualThingGraph !=null) {
                    return VisualThingGraph.Source as IThingGraph;
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

        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }

		public override void ReadXThings() {
           ReadInto(VisualsCollection);
           foreach (var visual in VisualsCollection) {
                var thing = VisualThingGraph.Get (visual);
                if (thing != null && !Things.Contains (thing)) {
                    Things.Add(thing);
                }
            }
        }

        protected virtual void ReadInto(ICollection<IVisual> visuals) {
            foreach (var node in XThings.Elements()) {
                var visual = ReadVisual(node);
                if (visual != null && !visuals.Contains(visual)) {
                    visuals.Add(visual);
                }
            }
           
            foreach (var visual in visuals.OfType<IVisualEdge>()
                .Where(edge => !visuals.Contains(edge.Root) || !visuals.Contains(edge.Leaf)).ToArray())
                visuals.Remove (visual);
        }

        protected virtual IVisual ReadVisual(XElement node) {
            var thing = Read (node);
            
            this.Things.Add (thing);

            IVisual visual = null;
            if (thing != null) {
                visual = VisualThingGraph.Get (thing);
                if (visual!=null && Layout != null) {
                    Layout.Perform (visual);
                    var x = ReadDouble(node, "x");
                    var y = ReadDouble (node, "y");
                    var w = ReadDouble (node, "w");
                    var h = ReadDouble (node, "h");
                    visual.Shape.Location = new Point(x, y);
                    visual.Shape.DataSize = new Size(w, h);
                }
                
            }
            return visual;
        }

        public virtual XElement Write(IVisual visual) {
            XElement xmlthing = null;

            var thing = VisualThingGraph.Get (visual);
            if (thing != null) {
                xmlthing = Write (thing);
                xmlthing.Add (new XAttribute ("x", visual.Location.X.ToString (CultureInfo.InvariantCulture)));
                xmlthing.Add (new XAttribute ("y", visual.Location.Y.ToString (CultureInfo.InvariantCulture)));
                xmlthing.Add (new XAttribute ("w", visual.Shape.DataSize.Width.ToString (CultureInfo.InvariantCulture)));
                xmlthing.Add (new XAttribute ("h", visual.Shape.DataSize.Height.ToString (CultureInfo.InvariantCulture)));
            }

            return xmlthing;
        }

        public virtual void Write(IEnumerable<IVisual> visuals) {
            if (visuals == null) return;
            foreach (var visual in visuals) {
                Write(visual);
            }
        }

        public override void Write(System.IO.Stream s) {
            Write(VisualsCollection);
            if (VisualsCollection.Count > 0) {
                using (var writer = System.Xml.XmlWriter.Create (s)) {
                    Document.Save (writer);
                    writer.Flush ();
                }
            }
        }
    }

}

