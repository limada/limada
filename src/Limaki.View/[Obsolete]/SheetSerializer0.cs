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
    
    [Obsolete]
    public class SheetSerializer0:IDisposable {
		
        public SheetSerializer0(IGraphScene<IVisual,IVisualEdge> scene) {
            this.Scene= scene;
        }

        public SheetSerializer0(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout): this(scene) {
            this._layout = layout;
        }

        IGraphSceneLayout<IVisual,IVisualEdge> _layout = null;
        public virtual IGraphSceneLayout<IVisual,IVisualEdge> Layout {
            get {
                if (_layout == null) {
                    Func<IGraphScene<IVisual, IVisualEdge>> fScene = () => this.Scene;
                    _layout = Registry.Factory.Create <IGraphSceneLayout<IVisual, IVisualEdge>>(
                        fScene,
                        Registry.Pooled<StyleSheets>().DefaultStyleSheet);
                }
                return _layout;
            }
            protected set { _layout = value; }
        }

        public virtual IGraphScene<IVisual, IVisualEdge> Scene { get; protected set; }

        public virtual void Save(Stream s) {

            var graph = Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                var serializer = new VisualThingXmlSerializer(); 
                serializer.VisualThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.VisualsCollection = Scene.Graph;
                serializer.Write (s);
            }
        }

        public virtual void Read(Stream stream) {

            var graph = Scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            if (graph != null) {
                var serializer = new VisualThingXmlSerializer { VisualThingGraph = graph, Layout = this.Layout };
                serializer.Read(stream);

                new GraphSceneFacade<IVisual, IVisualEdge>(() => this.Scene, this.Layout)
                    .Add(serializer.VisualsCollection, true, false);

                Scene.ClearSpatialIndex();
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
