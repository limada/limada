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
            Iori input = null;
            ISink<Iori, Stream> file = null;
            ISink<Stream, IThingGraph> thingGraphProvider = null;
            ISink<IThingGraph, IGraphScene<IVisual, IVisualEdge>> sceneProvider = null;

            var scene = sceneProvider.Use(thingGraphProvider.Use(file.Use(input)));

        }

        public void ContentProviders () {

            Iori input = null;
            ISink<Iori, Stream> file = null;
            ISink<Stream, Content<Stream>> contentProvider = null;
            ISink<Iori, Content<Stream>> contentIoProvider = null;

            ISink<Content<Stream>, IThing> contentThingProvider = null;
        }

        public void DataProviders () {
            ISink<Content<Stream>, GraphCursor<IThing,ILink>> contentThingGraphProvider = null;
            ISink<IEnumerable<IThing>, IThingGraph> mergeGraph = null;
        }

        public void ContentDiggProviders () {
            var provider = new ContentDiggProvider();
            var digger = new ContentDigger((so,si)=> si);
            provider.Add(digger);
            var content = new Content<Stream>();
            provider.Use(content, content);
        }
    }
}