
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Basic;
using Limaki.Tests.Graph.GraphPair;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;

namespace Limaki.Tests.View.Visuals {

    // View-Model-Tests where View is IVisual and Model is IGraphEntity

    public class EntitySampleSceneFactory<T> : SampleSceneFactory<IGraphEntity, IGraphEdge, T> 
        where T : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () { }

    public class EntityProgrammingSceneTest : ProgrammingSceneTest<IGraphEntity, IGraphEdge> { }

    public class EntityBinarySceneTest : BinarySceneTest<IGraphEntity, IGraphEdge> { }

    public class EntityGCJohnBostonSceneTest : GCJohnBostonSceneTest<IGraphEntity, IGraphEdge> { }

    public class EntitySceneMeshTest : SceneMeshTest<IGraphEntity, IGraphEdge> { }

    public class VisualEntityBasicGraphPairTest : BasicGraphPairTest<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> {

        public override IGraph<IVisual, IVisualEdge> Graph {
            get {
                if (!(base.Graph is IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)) {
                    var one = new Graph<IVisual, IVisualEdge> ();
                    var two = new Graph<IGraphEntity, IGraphEdge> ();

                    base.Graph = new GraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> (
                        one, two, new VisualGraphEntityTransformer ());
                }
                return base.Graph;
            }
            set { base.Graph = value; }
        }

        public override BasicGraphTestDataFactory<IVisual, IVisualEdge> GetFactory () {
            return new VisualGraphTestDataFactory ();
        }
    }
}