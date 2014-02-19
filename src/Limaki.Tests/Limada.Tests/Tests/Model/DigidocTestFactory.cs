using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Common;

namespace Limada.Tests.Model {
    

    public class DigidocTestFactory : TestGraphFactory<IThing, ILink> {

        public override string Name {
            get { return "DigidocSchema Things"; }
        }

        private IThingFactory _factory = null;
        public IThingFactory factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        public override void Populate(IGraph<IThing, ILink> graph) {
            Nodes[1] = factory.CreateItem();

            Nodes[2] = factory.CreateItem("");
            Nodes[2].Data = "Document " + Nodes[2].Id.ToString ("X");

            Edges[1] = factory.CreateEdge(Nodes[1], Nodes[2], DigidocSchema.DocumentTitle);

            Nodes[3] = factory.CreateItem<Stream>(null);
            Edges[2] = factory.CreateEdge(Nodes[1], Nodes[3], DigidocSchema.DocumentPage);

            Nodes[4] = factory.CreateItem<int>(1);
            Edges[3] = factory.CreateEdge(Edges[2], Nodes[4], DigidocSchema.PageNumber);

            AddSamplesToGraph (graph);
        }
    }
}