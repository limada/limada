/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 20012-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class SceneTestEnvironment<TItem, TEdge, TFactory>
        where TEdge : IEdge<TItem>, TItem
        where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {

        protected ISampleGraphSceneFactory _factory;
        public virtual ISampleGraphSceneFactory SampleFactory {
            get {
                if (_factory == null) {
                    _factory = new SampleSceneFactory<TItem, TEdge, TFactory> ();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        protected IGraphScene<IVisual, IVisualEdge> _scene;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                if (_scene == null) {
                    var g = this.SampleFactory.Scene.Graph;
                    g = new SubGraph<IVisual, IVisualEdge> (
                        ((SampleGraphPairFactory<IVisual, TItem, IVisualEdge, TEdge>) this.SampleFactory).GraphPair,
                        new VisualGraph ());
                    _scene = new Scene ();
                    _scene.Graph = g;
                }
                return _scene;
            }
            set {
                _scene = value;
                if (_display != null) {
                    _display.Data = value;
                }
            }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _display = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display {
            get { return _display ?? (_display = new VisualsDisplay { Data = this.Scene }); }
            set { _display = value; }
        }

        protected GraphSceneFacade<IVisual, IVisualEdge> _sceneFacade;
        public virtual GraphSceneFacade<IVisual, IVisualEdge> SceneFacade {
            get { return _sceneFacade ?? (_sceneFacade = new GraphSceneFacade<IVisual, IVisualEdge> (() => this.Scene, Display.Layout)); }
        }

        public virtual void Reset () {
            _scene = null;
            _factory = null;
            _sceneFacade = null;
            _display = null;
        }

        /// <summary>
        /// sets Scene.Focus to item and 
        /// calls Layout.Perform and Layout.AdjustSize
        /// item is added if not in view
        /// </summary>
        public void SetFocused (IVisual item) {
            this.Scene.Focused = item;
            EnsureShape (item);
            this.Scene.AddBounds (item);
        }

        public void EnsureShape (IVisual item) {
            this.Display.Layout.Perform (item);
            this.Display.Layout.AdjustSize (item);
        }
    }
}