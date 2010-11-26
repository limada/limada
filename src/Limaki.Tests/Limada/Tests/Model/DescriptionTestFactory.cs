using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;

namespace Limada.Tests.Model {
    public class DescriptionTestFactory:GenericGraphFactory<IThing,ILink> {

        public override string Name {
            get { return "Things with Description"; }
        }

        public IThing Root = null;
        public IThing TestMarker = null;

        public override void Populate(IGraph<IThing, ILink> graph) {
            Node[1] = new Thing ();
            Node[2] = new Thing<string> ("a thing with simple description");
            Edge[1] = new Link (Node[1], Node[2], CommonSchema.DescriptionMarker);

            Node[3] = new Thing();
            Node[4] = new Thing<string>("a thing with a metaschema description");
            Node[5] = new Thing<string>("this marker is a description");
            Node[6] = new Thing<string>("this marker is a dummy");
            Edge[2] = new Link(Node[3], Node[4], Node[5]);
            Edge[3] = new Link (Node[6], Node[5], MetaSchema.DescriptionMarker);
            
            
            AddSamplesToGraph(graph);
        }

        public override void Populate() {
            Root = new Thing<string> ("");
            Root.Data = "DescriptionTest " + Root.Id.ToString ("X")+ "Count:"+Count.ToString();
            this.Graph.Add (Root);
            this.Graph.Add (TopicSchema.Topics);
            this.Graph.Add (new Link (TopicSchema.Topics, Root, TestMarker));

            for (int i = 0; i < this.Count; i++) {
                Populate(this.Graph);
                Graph.Add(new Link(Root, Node[1], CommonSchema.CommonMarker));
                Node[2].Data = "Node " + i.ToString();
                Graph.Add(Node[2]);
                IThing subRoot = Node[1];
                for (int j = 0; j < this.Count; j++) {
                    Populate(this.Graph);
                    Graph.Add(new Link(subRoot, Node[1], CommonSchema.CommonMarker));
                    Node[2].Data = "SubNode " + i.ToString();
                    Graph.Add(Node[2]);
                }

            }

        }
    }
}