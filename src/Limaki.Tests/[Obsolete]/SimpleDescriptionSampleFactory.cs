using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Common;

namespace Limada.Tests.Model {

    public class SimpleDescriptionSampleFactory:SampleGraphFactoryBase<IThing,ILink> {

        public override string Name {
            get { return "Things with Description"; }
        }

        public IThing Root = null;
        public IThing TestMarker = null;

        public override void Populate(IGraph<IThing, ILink> graph) {
            var factory = Registry.Factory.Create<IThingFactory>();;
            Nodes[1] = factory.CreateItem();
            Nodes[2] = factory.CreateItem("a thing with description");
            Edges[1] = factory.CreateEdge (Nodes[1], Nodes[2], CommonSchema.DescriptionMarker);

            AddSamplesToGraph(graph);
        }

        public override void Populate() {

            for(int i=0;i<this.Count;i++) {
                Populate (this.Graph);
            }

        }
    }
}