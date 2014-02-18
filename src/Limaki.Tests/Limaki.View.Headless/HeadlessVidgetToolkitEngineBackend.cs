using Limaki.View;
using Limaki.View.Headless.VidgetBackends;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.View.XwtBackend;
using Limaki.Visuals;

namespace Limaki.View.HeadlessBackends {
    public class HeadlessVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {

            base.InitializeBackends ();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend> ();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend> ();

        }
    }
}