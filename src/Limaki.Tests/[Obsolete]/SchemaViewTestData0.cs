using Limada.Model;
using Limada.Schemata;

namespace Limada.Tests.View {
    public class SchemaViewTestData0<T>:ThingSceneFactory0
        where T:Schema, new() {
        public override IThingGraph ThingGraph {
            get {
                
                if (_thingGraph==null) {
                    _thingGraph = new T().SchemaGraph;
                }
                return _thingGraph;
            }
            set {
                _thingGraph = value;
            }
        }
    }
}