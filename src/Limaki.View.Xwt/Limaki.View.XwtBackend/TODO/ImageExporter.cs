using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Limaki.Visuals;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {
    class ImageExporter {
        private IGraphScene<IVisual, IVisualEdge> graphScene;
        private IGraphSceneLayout<IVisual, IVisualEdge> graphSceneLayout;
        public IStyleSheet StyleSheet { get; set; }

        public ImageExporter (IGraphScene<IVisual, IVisualEdge> graphScene, IGraphSceneLayout<IVisual, IVisualEdge> graphSceneLayout) {
            // TODO: Complete member initialization
            this.graphScene = graphScene;
            this.graphSceneLayout = graphSceneLayout;
        }

        internal Image ExportImage () {
            throw new NotImplementedException();
        }

        
    }
}
