using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;

namespace Limada.Tests.Data.db4o {
    public class DocumentSchemaTestFactory : GenericGraphFactory<IThing, ILink> {

        public override string Name {
            get { return "DocumentSchema Things"; }
        }

        public override void Populate(IGraph<IThing, ILink> graph) {
            Node[1] = new Thing();
            
            Node[2] = new Thing<string>("");
            Node[2].Data = "Document " + Node[2].Id.ToString ("X");
            
            Edge[1] = new Link(Node[1], Node[2], DocumentSchema.DocumentTitle);
            
            Node[3] = new Thing();
            Edge[2] = new Link(Node[1], Node[3], DocumentSchema.DocumentPage);

            Node[4] = new Thing<string>("1");
            Edge[3] = new Link(Edge[2], Node[4], DocumentSchema.PageNumber);

            AddSamplesToGraph (graph);
        }
    }
}