using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Visuals {

    public abstract class GraphSceneFactory<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge, TFactory> : SampleGraphPairFactory<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem
        where TFactory : ISampleGraphFactory<TSourceItem, TSourceEdge>, new() {

        public GraphSceneFactory () : base (new TFactory (), null) { }

        public override ISampleGraphFactory<TSourceItem, TSourceEdge> Factory {
            get { return _factory ?? (_factory = new TFactory ()); }
        }

        protected abstract IGraphScene<TSinkItem, TSinkEdge> CreateScene();

        /// <summary>
        /// Creates a new scene and populates it
        /// </summary>
        public virtual IGraphScene<TSinkItem, TSinkEdge> Scene {
            get {
                var result = CreateScene ();
                Populate ();
                result.Graph = this.Graph;
                return result;
            }
        }

        public virtual void PopulateScene (IGraphScene<TSinkItem, TSinkEdge> scene) {
            Populate (scene.Graph);
            scene.ClearSpatialIndex ();
        }

    }
}