using Limaki.Widgets;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Widget {
    public class SimpleGraph : SceneTestData {
        public class SimpleGraphFactory:BinaryGraphFactoryBase {
            public override void Populate(int start) {
                IGraphItem item = new GraphItem<int> (( start++ ));
                Graph.Add (item);
                Node1 = item;

                item = new GraphItem<int> (( start++ ));
                Graph.Add (item);
                Node2 = item;

                Link1 = new GraphEdge (Node1, Node2);
                Graph.Add (Link1);
            }

            public override string Name {
                get { return "SimpleGraph"; }
            }
        }
        public override GenericGraphFactory<IGraphItem, IGraphEdge> Data {
            get {
                if (_data == null) {
                    _data = new SimpleGraphFactory();
                }
                return _data;
            }
        }
    }

}