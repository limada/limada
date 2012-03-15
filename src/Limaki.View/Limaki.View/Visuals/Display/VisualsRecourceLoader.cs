using Limaki.Graphs;
using Limaki.Common.IOC;
using Limaki.Visuals;

namespace Limaki.View.Visuals.Display {
    public class VisualsRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IGraphModelFactory<IVisual, IVisualEdge>, VisualFactory>();
        }
    }
}