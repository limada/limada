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

using Limada.View.Vidgets;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;

namespace Limada.View.ContentViewers {

    public class DigidocContentViewer : ContentVisualViewer {
        DigidocVidget _digidocVidget = null;

        public DigidocVidget DigidocVidget {
            get {
                if (_digidocVidget == null) {
                    _digidocVidget = new DigidocVidget();
                }
                return _digidocVidget;
            }

            protected set { _digidocVidget = value; }
        }

        public override IVidget Frontend {
            get { return DigidocVidget; }
        }

        public override IVidgetBackend Backend {
            get { return DigidocVidget.Backend; }
        }
        
        public override bool Supports (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var docManager = new DigidocViz();
            return docManager.HasPages(graph, visual);
        }

        public override void SetContent (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            DigidocVidget.SetDocument (GraphCursor.Create (graph, visual));
        }

        public override void OnShow () {
            base.OnShow();
            DigidocVidget.OnShow();
        }
        public override void Dispose () {
            DigidocVidget.Dispose();
            DigidocVidget = null;
        }
    }
}