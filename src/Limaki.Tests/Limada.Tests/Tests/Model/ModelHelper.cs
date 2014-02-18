using Limada.Model;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Visuals;

namespace Limada.Tests.Model {
    public static class ModelHelper {
        public static VisualThingGraph GetSourceGraph<TFactory>()
            where TFactory : GenericGraphFactory<IGraphEntity, IGraphEdge>, new() {
            return GetSourceGraph<TFactory>(1);
        }

        public static VisualThingGraph GetSourceGraph<TFactory>(int count)
            where TFactory : GenericGraphFactory<IGraphEntity, IGraphEdge>, new() {
            var mock = new Mock008<ProgrammingLanguageFactory>();

            mock.Factory.Count = count;

            mock.Display.Reset();
            mock.SceneFacade.ShowAllData();
            mock.Display.Perform();

            var visualGraph = (mock.Scene.Graph as SubGraph<IVisual, IVisualEdge>).Sink;
            visualGraph.ChangeData = null;
            visualGraph.DataChanged = null;
            visualGraph.GraphChanged = null;


            var sourceGraph = new VisualThingGraph(visualGraph, new ThingGraph());
            sourceGraph.Mapper.ConvertSinkSource();
            return sourceGraph;
        }
    }
}