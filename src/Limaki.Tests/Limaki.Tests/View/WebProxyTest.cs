using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Contents;
using Limaki.Drawing;
using Limada.View;
using Limada.VisualThings;
using Limaki.Visuals;
using Limada.Model;
using Limaki.View.Visualizers;

namespace Limaki.Tests.View {

    public class WebProxyTest {
        /// <summary>
        /// tests if scene.Focused with a call to HtmlViewer is an infinite loop
        /// </summary>
        /// <param name="display"></param>
        public void TestInfinitLoopIfHtmlContentIsFocused(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var scene = display.Data;
            if (scene != null ) {
                foreach(var item in scene.Elements
                    .Select(v=>scene.Graph.ThingOf(v)).OfType<IStreamThing>()
                    .Where(s=>s.StreamType==ContentTypes.HTML)) {
                    scene.Focused = scene.Graph.VisualOf(item);
                    display.OnSceneFocusChanged();
                }
                scene.Selected.Clear();
                scene.Focused = null;
            }
        }
    }
}
