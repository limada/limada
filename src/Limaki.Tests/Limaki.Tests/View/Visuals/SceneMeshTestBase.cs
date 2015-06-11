/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mesh;

namespace Limaki.Tests.View.Visuals {

    public class SceneMeshTestBase<TItem, TEdge> : DomainTest where TEdge : IEdge<TItem>, TItem {
        
        IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;
        protected IGraphSceneMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>> ()); } }

        public IEnumerable<SceneTestEnvironment<TItem, TEdge>> MeshTests (params SceneTestEnvironment<TItem, TEdge>[] sources) {

            var source = sources[0];
            Mesh.AddDisplay (source.Display);

            foreach (var sink in sources.Skip (1)) {

                sink.Scene = Mesh.CreateSinkScene (source.Scene.Graph);

                Mesh.AddDisplay (sink.Display);

                ((SampleGraphPairFactory<IVisual, TItem, IVisualEdge, TEdge>)
                 sink.SampleFactory).GraphPair =
                    // no, its not sink.Graph, and not sink.Scene.... but:
                    sink.Scene.Graph.Source<IVisual, IVisualEdge, TItem, TEdge> ();
                ;

                // take the inner factorys:
                var sourceFactory = Innerfactory (source.SampleFactory);
                var sinkFactory = Innerfactory (sink.SampleFactory);

                // copy Nodes and Edges
                var i = 0;
                foreach (var n in sourceFactory.Nodes) sinkFactory.Nodes[i++] = n;
                i = 0;
                foreach (var n in sourceFactory.Edges) sinkFactory.Edges[i++] = n;

                sink.EnsureScene ();
            }

            return sources;
        }

        protected ISampleGraphFactory<TItem, TEdge> Innerfactory (ISampleGraphFactory<IVisual, IVisualEdge> factory) {
            var p = factory as SampleGraphPairFactory<IVisual, TItem, IVisualEdge, TEdge>;
            if (p != null)
                return p.Factory;
            return null;
        }

        /// <summary>
        /// this tests if a link is added, changed and removed 
        /// </summary>

        public void EdgeAddChangeRemove<TFactory> (SceneTestEnvironment<TItem, TEdge, TFactory> source, int iOne, int iTwo, int iThree)
            where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {

            source.EnsureShape (source.Nodes[iThree]);

            source.SetFocused (source.Nodes[iOne]);
            source.SceneFacade.CollapseToFocused ();

            source.Expand (source.Nodes[iTwo], false);

            var sourceEdge = source.AddEdge (source.Nodes[iOne], source.Nodes[iTwo]);

            source.ChangeLink (sourceEdge, source.Nodes[iThree], true);

            source.RemoveEdge (sourceEdge);

            source.Expand (source.Nodes[iTwo], true);

            source.ProveViewNotContains (sourceEdge);

        }
  
    }
}