using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Widgets;

namespace Limada.Tests.Model {
    public static class ModelHelper {
        public static WidgetThingGraph GetSourceGraph<TFactory>()
            where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            return GetSourceGraph<TFactory>(1);
        }

        public static WidgetThingGraph GetSourceGraph<TFactory>(int count)
            where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            Mock<ProgrammingLanguageFactory> mock = new Mock<ProgrammingLanguageFactory>();

            mock.Factory.Count = count;

            mock.Display.Invoke();
            mock.SceneFacade.ShowAllData();
            mock.Display.Execute();

            IGraph<IWidget, IEdgeWidget> widgetGraph = (mock.Scene.Graph as GraphView<IWidget, IEdgeWidget>).One;
            widgetGraph.ChangeData = null;
            widgetGraph.DataChanged = null;
            widgetGraph.GraphChanged = null;


            WidgetThingGraph sourceGraph = new WidgetThingGraph(widgetGraph, new ThingGraph());
            sourceGraph.Mapper.ConvertOneTwo();
            return sourceGraph;
        }
    }
}