using Limaki.View.Visuals.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {
    public class HedlessVisualsTextEditAction: VisualsTextEditActionBase {
        private Func<Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge>> func;
        private Visualizers.Display<Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge>> display1;
        private Drawing.IGraphSceneLayout<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge> graphSceneLayout;

        public HedlessVisualsTextEditAction (Func<Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge>> func, Visualizers.Display<Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge>> display1, Drawing.ICamera camera, Drawing.IGraphSceneLayout<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge> graphSceneLayout) {
            // TODO: Complete member initialization
            this.func = func;
            this.display1 = display1;
            this.camera = camera;
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

        public override void OnKeyPressed (UI.KeyActionEventArgs e) {
            throw new NotImplementedException ();
        }
    }
}
