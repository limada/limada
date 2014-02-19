using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Common;

namespace Limada.Tests.Model {

    public class DescriptionTestFactory:TestGraphFactory<IThing,ILink> {

        public override string Name {
            get { return "Things with Description"; }
        }

        public IThing Root = null;
        public IThing TestMarker = null;

        private IThingFactory _factory = null;
        public IThingFactory factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        public override void Populate(IGraph<IThing, ILink> graph) {
            
            Nodes[1] = factory.CreateItem ();
            Nodes[2] = factory.CreateItem ("a thing with simple description");
            Edges[1] = factory.CreateEdge(Nodes[1], Nodes[2], CommonSchema.DescriptionMarker);

            Nodes[3] = factory.CreateItem ();
            Nodes[4] = factory.CreateItem ("a thing with a metaschema description");
            Nodes[5] = factory.CreateItem ("this marker is a description");
            Nodes[6] = factory.CreateItem ("this marker is a dummy");
            Edges[2] = factory.CreateEdge(Nodes[3], Nodes[4], Nodes[5]);
            Edges[3] = factory.CreateEdge(Nodes[6], Nodes[5], MetaSchema.DescriptionMarker);
            
            
            AddSamplesToGraph(graph);
        }

        public override void Populate() {
            Root = factory.CreateItem("");
            Root.Data = "DescriptionTest " + Root.Id.ToString ("X")+ "Count:"+Count.ToString();
            this.Graph.Add (Root);
            this.Graph.Add (TopicSchema.Topics);
            this.Graph.Add (new Link (TopicSchema.Topics, Root, TestMarker));

            for (int i = 0; i < this.Count; i++) {
                Populate(this.Graph);
                Graph.Add(new Link(Root, Nodes[1], CommonSchema.CommonMarker));
                Nodes[2].Data = "Node " + i.ToString();
                Graph.Add(Nodes[2]);
                IThing subRoot = Nodes[1];
                for (int j = 0; j < this.Count; j++) {
                    Populate(this.Graph);
                    Graph.Add(new Link(subRoot, Nodes[1], CommonSchema.CommonMarker));
                    Nodes[2].Data = "SubNode " + i.ToString();
                    Graph.Add(Nodes[2]);
                }

            }

        }
    }
}