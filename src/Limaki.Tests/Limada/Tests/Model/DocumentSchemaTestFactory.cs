using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.IO;

namespace Limada.Tests.Model {
    public class DocumentSchemaTestFactory : GenericGraphFactory<IThing, ILink> {

        public override string Name {
            get { return "DocumentSchema Things"; }
        }

        private ThingFactory factory = new ThingFactory ();

        public override void Populate(IGraph<IThing, ILink> graph) {
            Node[1] = factory.CreateItem();

            Node[2] = factory.CreateItem("");
            Node[2].Data = "Document " + Node[2].Id.ToString ("X");

            Edge[1] = factory.CreateEdge(Node[1], Node[2], DocumentSchema.DocumentTitle);

            Node[3] = factory.CreateItem<Stream>(null);
            Edge[2] = factory.CreateEdge(Node[1], Node[3], DocumentSchema.DocumentPage);

            Node[4] = factory.CreateItem(1);
            Edge[3] = factory.CreateEdge(Edge[2], Node[4], DocumentSchema.PageNumber);

            AddSamplesToGraph (graph);
        }
    }
}