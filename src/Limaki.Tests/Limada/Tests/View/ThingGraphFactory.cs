using Limaki.Model;
using Limada.Model;
using Limaki.Widgets;
using Limada.View;
using Limaki.Tests.Graph.Model;

namespace Limada.Tests.Model {
    public class ThingSceneFactoryNew<T> : GenericBiGraphFactory<IWidget, IThing, IEdgeWidget, ILink>
        where T : IGraphFactory<IThing, ILink>, new() {
        public ThingSceneFactoryNew() : base(new T(), new WidgetThingAdapter().ReverseAdapter()) { }
    }


    public class ThingGraphFactory<T> : GenericBiGraphFactory<IThing, IGraphItem, ILink, IGraphEdge> 
    where T : IGraphFactory<IGraphItem,IGraphEdge>, new() {
        public ThingGraphFactory():base(new T(),new GraphItem2ThingAdapter()) {}
        public override IGraphFactory<IGraphItem, IGraphEdge> Factory {
            get {
                if (_factory == null) {
                    _factory = new T();
                }
                return _factory;
            }
        }
    }

    //public class ThingGraphFactory<F>:ThingGraphFactory 
    //    where F:IGraphFactory<IGraphItem,IGraphEdge>, new() {

        
    //    public override string Name {
    //        get { return new F ().Name; }
    //    }

    //    public override void Populate() {
            
    //    }
    //    public override void Populate(IGraph<IThing, ILink> graph) {
            
    //    }
    //} 
}
