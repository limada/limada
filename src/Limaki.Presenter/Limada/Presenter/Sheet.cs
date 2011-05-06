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
using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Widgets.Layout;
using Limaki.Widgets;
using Limaki.Presenter.Widgets.UI;

namespace Limada.Presenter {
    
    public class Sheet:IDisposable {
        public Sheet(IGraphScene<IWidget,IEdgeWidget> scene) {
            this._scene = scene;
        }

        public Sheet(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout): this(scene) {
            this._layout = layout;
        }

        IGraphLayout<IWidget,IEdgeWidget> _layout = null;
        public virtual IGraphLayout<IWidget,IEdgeWidget> Layout {
            get {
                if (_layout == null) {
                    _layout = new ArrangerLayout<IWidget, IEdgeWidget>(
                        () => { return this.Scene; },
                        Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet);
                }
                return _layout;
            }
            protected set { _layout = value; }
        }

        private IGraphScene<IWidget, IEdgeWidget> _scene = null;
        public virtual IGraphScene<IWidget, IEdgeWidget> Scene {
            get { return _scene; }
            protected set { _scene = value; }
        }

        public virtual void Save(Stream s) {
            var graph = GraphPairExtension<IWidget, IEdgeWidget>
                .Source<IThing, ILink>(Scene.Graph);

            if (graph != null) {
                WidgetThingSerializer serializer = new WidgetThingSerializer(); 
                serializer.WidgetThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.WidgetCollection = Scene.Graph;
                serializer.Write (s);
            }
        }

        public virtual void Read(Stream s) {
            var graph = GraphPairExtension<IWidget, IEdgeWidget>.Source<IThing, ILink>(Scene.Graph);

            if (graph != null) {
                WidgetThingSerializer serializer = new WidgetThingSerializer();
                serializer.WidgetThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.Read(s);

                new GraphSceneFacade<IWidget, IEdgeWidget>(delegate() { return this.Scene; }, this.Layout)
                    .Add (serializer.WidgetCollection, false, false);

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

