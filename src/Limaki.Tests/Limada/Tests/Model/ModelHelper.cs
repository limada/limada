using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Visuals;

namespace Limada.Tests.Model {
    public static class ModelHelper {
        public static VisualThingGraph GetSourceGraph<TFactory>()
            where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            return GetSourceGraph<TFactory>(1);
        }

        public static VisualThingGraph GetSourceGraph<TFactory>(int count)
            where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            var mock = new Mock<ProgrammingLanguageFactory>();

            mock.Factory.Count = count;

            mock.Display.Invoke();
            mock.SceneFacade.ShowAllData();
            mock.Display.Execute();

            var visualGraph = (mock.Scene.Graph as GraphView<IVisual, IVisualEdge>).One;
            visualGraph.ChangeData = null;
            visualGraph.DataChanged = null;
            visualGraph.GraphChanged = null;


            var sourceGraph = new VisualThingGraph(visualGraph, new ThingGraph());
            sourceGraph.Mapper.ConvertOneTwo();
            return sourceGraph;
        }
    }
}