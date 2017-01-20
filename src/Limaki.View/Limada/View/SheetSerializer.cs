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
using System.IO;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;

namespace Limada.View {

    public class SheetSerializer {

        public virtual void Save (Stream s, IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout) {

            var graph = source.Source<IVisual, IVisualEdge, IThing, ILink> ();

            if (graph != null) {
                var serializer = new VisualThingXmlSerializer { VisualThingGraph = graph, Layout = layout, VisualsCollection = source};
                serializer.Write (s);
            }
        }

        public virtual ICollection<IVisual> Read (Stream stream, IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout) {

            var graph = source.Source<IVisual, IVisualEdge, IThing, ILink> ();

            if (graph == null)
                return null;
            
            var serializer = new VisualThingXmlSerializer { VisualThingGraph = graph, Layout = layout };
            serializer.Read (stream);
            return serializer.VisualsCollection;
        }

    }

    

}

