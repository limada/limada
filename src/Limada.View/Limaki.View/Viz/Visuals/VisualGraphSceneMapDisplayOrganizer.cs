using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;

namespace Limaki.View.Viz.Visuals {

    public class VisualGraphSceneMapDisplayOrganizer : GraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> {

        public override IGraphScene<IVisual, IVisualEdge> CreateSinkScene (IGraph<IVisual, IVisualEdge> sourceGraph) {
            var result = new Scene ();
            var sinkGraph = CreateSinkGraph (sourceGraph);
            if (sinkGraph != null)
                result.Graph = sinkGraph;
            result.CreateMarkers();
            return result;
        }
        
        protected override IGraphSceneDisplayEvents<IVisual, IVisualEdge> CreateSceneEvents () {
            return new VisualGraphSceneDisplayMeshEvents ();
        }
    }
}