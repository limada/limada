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

    public class DigidocContentViewer : ContentVisualViewer {
        DigidocViewer _digidocViewer = null;

        public DigidocViewer DigidocViewer {
            get {
                if (_digidocViewer == null) {
                    _digidocViewer = new DigidocViewer();
                }
                return _digidocViewer;
            }

            protected set { _digidocViewer = value; }
        }

        public override IVidget Frontend {
            get { return DigidocViewer; }
        }

        public override IVidgetBackend Backend {
            get { return DigidocViewer.Backend; }
        }
        
        public override bool Supports (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DigidocViz();
            return docManager.HasPages(graph, visual);
        }

        public override void SetContent (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            DigidocViewer.SetDocument(new GraphCursor<IVisual, IVisualEdge>(graph, visual));
        }

        public override void Dispose () {
            DigidocViewer.Dispose();
            DigidocViewer = null;
        }
    }
}