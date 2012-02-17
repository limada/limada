using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Limaki.Visuals;
using Limada.View;
using Limada.Model;
using Limaki.Model.Streams;
using Limaki.Presenter.Display;

namespace Limaki.Tests.Presenter {
    public class WebProxyTest {
        public void CircleFocusToHtml(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var scene = display.Data;
            if (scene != null ) {
                foreach(var item in scene.Elements
                    .Select(v=>scene.Graph.ThingOf(v)).OfType<IStreamThing>()
                    .Where(s=>s.StreamType==StreamTypes.HTML)) {
                    scene.Focused = scene.Graph.VisualOf(item);
                    display.OnSceneFocusChanged();
                }
                scene.Selected.Clear();
                scene.Focused = null;
            }
        }
    }
}
