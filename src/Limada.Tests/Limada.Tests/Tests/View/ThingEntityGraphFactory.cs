using Limaki.Model;
using Limada.Model;
using Limaki.Tests.Graph.Model;
using System;

namespace Limada.Tests.Model {

    public class ThingEntityGraphFactory<T> : SampleGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> 
    where T : ISampleGraphFactory<IGraphEntity,IGraphEdge>, new() {
        public ThingEntityGraphFactory():base(new T(),new GraphEntity2ThingTransformer().Reverted()) {}
        public override ISampleGraphFactory<IGraphEntity, IGraphEdge> Factory {
            get {
                if (_factory == null) {
                    _factory = new T();
                }
                return _factory;
            }
        }
    }

}
