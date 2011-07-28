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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Visuals.Layout;
using Limaki.Visuals;
using Limaki.Presenter.Visuals.UI;
using Limaki.Presenter.UI;

namespace Limada.Presenter {
    
    public class Sheet:IDisposable {
        public Sheet(IGraphScene<IVisual,IVisualEdge> scene) {
            this._scene = scene;
        }

        public Sheet(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout): this(scene) {
            this._layout = layout;
        }

        IGraphLayout<IVisual,IVisualEdge> _layout = null;
        public virtual IGraphLayout<IVisual,IVisualEdge> Layout {
            get {
                if (_layout == null) {
                    _layout = new ArrangerLayout<IVisual, IVisualEdge>(
                        () => { return this.Scene; },
                        Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet);
                }
                return _layout;
            }
            protected set { _layout = value; }
        }

        private IGraphScene<IVisual, IVisualEdge> _scene = null;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get { return _scene; }
            protected set { _scene = value; }
        }

        public virtual void Save(Stream s) {
            var graph = Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                VisualThingSerializer serializer = new VisualThingSerializer(); 
                serializer.VisualThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.VisualsCollection = Scene.Graph;
                serializer.Write (s);
            }
        }

        public virtual void Read(Stream s) {
            var graph = Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                var serializer = new VisualThingSerializer { VisualThingGraph = graph, Layout = this.Layout };
                serializer.Read(s);

                new GraphSceneFacade<IVisual, IVisualEdge>(()=>this.Scene, this.Layout)
                    .Add (serializer.VisualsCollection, false, false);

                Scene.ClearSpatialIndex ();
            }
        }

        #region IDisposable Member

        public void Dispose() {
            Scene = null;
            Layout = null;
        }

        #endregion
    }

}

