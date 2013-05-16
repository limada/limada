using Limada.View;
using Limada.VisualThings;
using Limaki.Model;
using Limada.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Model;

namespace Limada.Tests.Model {
    public class ThingSceneFactoryNew<T> : GenericBiGraphFactory<IVisual, IThing, IVisualEdge, ILink>
        where T : IGraphFactory<IThing, ILink>, new() {
        public ThingSceneFactoryNew() : 
            base(new T(), 
            new VisualThingAdapter().ReverseAdapter()) { }
    }


    public class ThingGraphFactory<T> : GenericBiGraphFactory<IThing, IGraphEntity, ILink, IGraphEdge> 
    where T : IGraphFactory<IGraphEntity,IGraphEdge>, new() {
        public ThingGraphFactory():base(new T(),new GraphItem2ThingAdapter()) {}
        public override IGraphFactory<IGraphEntity, IGraphEdge> Factory {
            get {
                if (_factory == null) {
                    _factory = new T();
                }
                return _factory;
            }
        }
    }

}
