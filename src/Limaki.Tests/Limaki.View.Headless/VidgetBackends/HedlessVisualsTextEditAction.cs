using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;

namespace Limaki.View.Headless.VidgetBackends {
    public class HedlessVisualsTextEditAction: VisualsTextEditActionBase {
        private Func<IGraphScene<IVisual, IVisualEdge>> func;
        private Display<IGraphScene<IVisual, IVisualEdge>> display1;
        private IGraphSceneLayout<IVisual, IVisualEdge> graphSceneLayout;

        public HedlessVisualsTextEditAction (Func<IGraphScene<IVisual, IVisualEdge>> func, Display<IGraphScene<IVisual, IVisualEdge>> display1, Drawing.ICamera camera, IGraphSceneLayout<IVisual, IVisualEdge> graphSceneLayout) {
            // TODO: Complete member initialization
            this.func = func;
            this.display1 = display1;
            this.Camera = camera;
            this.graphSceneLayout = graphSceneLayout;
        }
        protected override void AttachEditor () {
            throw new NotImplementedException ();
        }

        protected override void DetachEditor (bool writeData) {
            throw new NotImplementedException ();
        }

        protected override void ActivateMarkers () {
            throw new NotImplementedException ();
        }

        protected override Xwt.Point CursorPosition () {
            throw new NotImplementedException ();
        }
    }
}
