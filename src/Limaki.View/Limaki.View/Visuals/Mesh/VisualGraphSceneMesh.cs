using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Mesh;
using Limaki.Visuals;
using Limaki.View.GraphScene;

namespace Limaki.View.Visuals.UI {

    public class VisualGraphSceneMesh : GraphSceneMesh<IVisual, IVisualEdge> {

        public override IGraphScene<IVisual, IVisualEdge> CreateSinkScene (IGraph<IVisual, IVisualEdge> sourceGraph) {
            var result = new Scene ();
            var sinkGraph = CreateSinkGraph (sourceGraph);
            if (sinkGraph != null)
                result.Graph = sinkGraph;
            result.CreateMarkers();
            return result;
        }

        public override IGraph<IVisual, IVisualEdge> CreateSinkGraph (IGraph<IVisual, IVisualEdge> source) {

            IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> sourceGraph = source as SubGraph<IVisual, IVisualEdge>;

            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource ();

                var result = GraphMapping.Mapping.CloneGraphPair<IVisual, IVisualEdge> (sourceGraph.Source);

                if (result != null) {
                    // souround with a view
                    return new SubGraph<IVisual, IVisualEdge> (result, new VisualGraph ());
                }
            }
            return null;
        }

        protected override IGraphSceneEvents<IVisual, IVisualEdge> CreateSceneEvents () {
            return new VisualGraphSceneEvents ();
        }
    }
}