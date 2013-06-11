using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Common;

namespace Limada.Tests.Model {
    

    public class DigidocTestFactory : GenericGraphFactory<IThing, ILink> {

        public override string Name {
            get { return "DigidocSchema Things"; }
        }

        private IThingFactory _factory = null;
        public IThingFactory factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        public override void Populate(IGraph<IThing, ILink> graph) {
            Node[1] = factory.CreateItem();

            Node[2] = factory.CreateItem("");
            Node[2].Data = "Document " + Node[2].Id.ToString ("X");

            Edge[1] = factory.CreateEdge(Node[1], Node[2], DigidocSchema.DocumentTitle);

            Node[3] = factory.CreateItem<Stream>(null);
            Edge[2] = factory.CreateEdge(Node[1], Node[3], DigidocSchema.DocumentPage);

            Node[4] = factory.CreateItem<int>(1);
            Edge[3] = factory.CreateEdge(Edge[2], Node[4], DigidocSchema.PageNumber);

            AddSamplesToGraph (graph);
        }
    }
}