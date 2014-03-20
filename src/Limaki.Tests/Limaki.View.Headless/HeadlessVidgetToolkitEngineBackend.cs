using Limaki.View;
using Limaki.View.Headless.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend;

namespace Limaki.View.Headless {

    public class HeadlessVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {

            base.InitializeBackends ();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend> ();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend> ();

        }
    }
}