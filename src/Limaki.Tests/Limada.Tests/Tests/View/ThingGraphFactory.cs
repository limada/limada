using Limada.View;
using Limada.VisualThings;
using Limaki.Model;
using Limada.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Model;

namespace Limada.Tests.Model {
    public class ThingSceneFactoryNew<T> : TestGraphPairFactory<IVisual, IThing, IVisualEdge, ILink>
        where T : ITestGraphFactory<IThing, ILink>, new() {
        public ThingSceneFactoryNew() : 
            base(new T(), 
            new VisualThingTransformer().Reverted()) { }
    }


    public class ThingGraphFactory<T> : TestGraphPairFactory<IThing, IGraphEntity, ILink, IGraphEdge> 
    where T : ITestGraphFactory<IGraphEntity,IGraphEdge>, new() {
        public ThingGraphFactory():base(new T(),new GraphItem2ThingTransformer()) {}
        public override ITestGraphFactory<IGraphEntity, IGraphEdge> Factory {
            get {
                if (_factory == null) {
                    _factory = new T();
                }
                return _factory;
            }
        }
    }

}
