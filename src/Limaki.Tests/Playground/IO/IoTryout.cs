using System.IO;
using Limada.Model;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Visuals;
using Limaki.Model.Content;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Model.Content.IO;
using System.Collections.Generic;

namespace Limaki.Playground.IO {

    public class IoTryout {
        public void GraphProviders () {
            IoInfo input = null;
            ISink<IoInfo, Stream> file = null;
            ISink<Stream, IThingGraph> thingGraphProvider = null;
            ISink<IThingGraph, IGraphScene<IVisual, IVisualEdge>> sceneProvider = null;

            var scene = sceneProvider.Sink(thingGraphProvider.Sink(file.Sink(input)));

        }

        public void ContentProviders () {

            IoInfo input = null;
            ISink<IoInfo, Stream> file = null;
            ISink<Stream, Content<Stream>> contentProvider = null;
            ISink<IoInfo, Content<Stream>> contentIoProvider = null;

            ISink<Content<Stream>, IThing> contentThingProvider = null;
        }

        public void DataProviders () {
            ISink<Content<Stream>, GraphFocus<IThing,ILink>> contentThingGraphProvider = null;
            ISink<IEnumerable<IThing>, IThingGraph> mergeGraph = null;
        }
    }
}