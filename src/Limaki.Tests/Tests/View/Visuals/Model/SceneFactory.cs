/*
 * Limaki 
 
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

using Limaki.Model;
using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;

namespace Limaki.Tests.Visuals {
    public class SceneFactory<T> : GenericBiGraphFactory<IVisual, IGraphItem, IVisualEdge, IGraphEdge>, ISceneFactory
    where T : IGraphFactory<IGraphItem, IGraphEdge>, new() {
        public SceneFactory() : base(new T(), new GraphItem2VisualAdapter()) { }

        public override IGraphFactory<IGraphItem, IGraphEdge> Factory {
            get {
                if (_factory == null) {
                    _factory = new T();
                }
                return _factory;
            }
        }

        #region ISceneTestData Member

        /// <summary>
        /// Creates a new scene and populates it
        /// </summary>
        public virtual Scene Scene {
            get {
                var result = new Scene();
                Populate ();
                result.Graph = this.Graph;
                return result;
            }
        }

        public virtual void Populate(Scene scene) {
            Populate (scene.Graph);
            scene.ClearSpatialIndex ();
        }

        #endregion
    }
}