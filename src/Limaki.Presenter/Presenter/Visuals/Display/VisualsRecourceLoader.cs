using Limaki.Graphs;
using Limaki.Visuals;
using Limaki.Common.IOC;

namespace Limaki.Presenter.Visuals {
    public class VisualsRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IGraphModelFactory<IVisual, IVisualEdge>, VisualFactory>();
        }
    }
}