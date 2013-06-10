/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Usecases;
using Limaki.Graphs;
using Limaki.View;
using Limaki.Viewers;
using Limaki.Visuals;
using System.Collections.Generic;

namespace Limaki.Viewers.StreamViewers {

    public class DocumentSchemaContentViewer : ContentVisualViewer, IVidget {
        DocumentSchemaViewer _documentViewer = null;

        public DocumentSchemaViewer DocumentViewer {
            get {
                if (_documentViewer == null) {
                    _documentViewer = new DocumentSchemaViewer();
                }
                return _documentViewer;
            }

            protected set { _documentViewer = value; }
        }

        public override IVidget Frontend {
            get { return DocumentViewer; }
        }

        public override IVidgetBackend Backend {
            get { return DocumentViewer.Backend; }
        }
        
        public override bool Supports (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DocumentSchemaManager();
            return docManager.HasPages(graph, visual);
        }

        public override void SetContent (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            DocumentViewer.SetDocument(new GraphCursor<IVisual, IVisualEdge>(graph, visual));
        }

        public override void Dispose () {
            DocumentViewer.Dispose();
            DocumentViewer = null;
        }
    }
}