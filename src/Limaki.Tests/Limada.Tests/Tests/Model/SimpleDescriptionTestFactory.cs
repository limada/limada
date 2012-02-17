using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Common;

namespace Limada.Tests.Model {
    public class SimpleDescriptionTestFactory:GenericGraphFactory<IThing,ILink> {

        public override string Name {
            get { return "Things with Description"; }
        }

        public IThing Root = null;
        public IThing TestMarker = null;

        public override void Populate(IGraph<IThing, ILink> graph) {
            var factory = Registry.Factory.Create<IThingFactory>();;
            Node[1] = factory.CreateItem();
            Node[2] = factory.CreateItem("a thing with description");
            Edge[1] = factory.CreateEdge (Node[1], Node[2], CommonSchema.DescriptionMarker);

            AddSamplesToGraph(graph);
        }

        public override void Populate() {

            for(int i=0;i<this.Count;i++) {
                Populate (this.Graph);
            }

        }
    }
}