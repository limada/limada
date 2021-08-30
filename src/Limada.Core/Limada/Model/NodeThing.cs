using Id = System.Int64;

namespace Limada.Model {

    public interface INodeThing : IThing {}

    public class NodeThing : Thing, INodeThing {
        public NodeThing () : base () { }
        public NodeThing (Id id) : base (id) { }
            
    }

}