using Limaki.Widgets;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Widget {
    public class BinaryGraph : SceneTestData {
        public override GenericGraphFactory<IGraphItem, IGraphEdge> Data {
            get {
                if (_data == null) {
                    _data = new BinaryGraphFactory();
                }
                return _data;
            }
        }
    }

}