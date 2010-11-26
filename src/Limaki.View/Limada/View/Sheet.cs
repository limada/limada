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
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Id = System.Int64;
using Limaki.Common;

namespace Limada.View {
    
    public class Sheet:IDisposable {
        public Sheet(Scene scene) {
            this._scene = scene;
        }

        public Sheet(Scene scene, ILayout<Scene, IWidget> layout):this(scene) {
            this._layout = layout;
        }

        ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get {
                if (_layout == null) {
                    _layout = new GraphLayout<Scene, IWidget>(
                        () => { return this.Scene; },
                        Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet);
                }
                return _layout;
            }
            protected set { _layout = value; }
        }

        private Scene _scene = null;
        public virtual Scene Scene {
            get { return _scene; }
            protected set { _scene = value; }
        }

        public virtual void Save(Stream s) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(Scene.Graph);

            if (graph != null) {
                WidgetThingSerializer serializer = new WidgetThingSerializer(); 
                serializer.WidgetThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.WidgetCollection = Scene.Graph;
                serializer.Write (s);
            }
        }

        public virtual void Read(Stream s) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(Scene.Graph);

            if (graph != null) {
                WidgetThingSerializer serializer = new WidgetThingSerializer();
                serializer.WidgetThingGraph = graph;
                serializer.Layout = this.Layout;
                serializer.Read(s);

                new SceneFacade (delegate() { return this.Scene; }, this.Layout)
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

