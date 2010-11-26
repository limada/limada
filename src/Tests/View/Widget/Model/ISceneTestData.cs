using Limaki.Widgets;
using Limaki.Graphs;

namespace Limaki.Tests.Widget {
    public interface ISceneTestData {
        IGraph<IWidget, IEdgeWidget> Graph { get;set;}
        Scene Scene { get; }
        int Count { get; set; }
        string Name { get; }
        void Populate ( Scene scene );
    }
}