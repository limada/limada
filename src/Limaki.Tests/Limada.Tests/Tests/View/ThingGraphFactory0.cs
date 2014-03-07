using Limada.View;
using Limaki.Model;
using Limada.Model;
using Limaki.Tests.Graph.Model;
using System;

namespace Limada.Tests.Model {

    [Obsolete]
    public class ThingGraphFactory0<T> : SampleGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> 
    where T : ISampleGraphFactory<IGraphEntity,IGraphEdge>, new() {
        public ThingGraphFactory0():base(new T(),new GraphItem2ThingTransformer().Reverted()) {}
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
