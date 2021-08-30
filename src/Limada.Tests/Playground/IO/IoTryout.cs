using System.IO;
using Limada.Model;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Contents.IO;
using System.Collections.Generic;
using Limaki.View;
using Limaki.View.Visuals;

namespace Limaki.Playground.IO {

    public class IoTryout {
        public void GraphProviders () {
            Iori input = null;
            IPipe<Iori, Stream> file = null;
            IPipe<Stream, IThingGraph> thingGraphProvider = null;
            IPipe<IThingGraph, IGraphScene<IVisual, IVisualEdge>> sceneProvider = null;

            var scene = sceneProvider.Use(thingGraphProvider.Use(file.Use(input)));

        }

        public void ContentProviders () {

            Iori input = null;
            IPipe<Iori, Stream> file = null;
            IPipe<Stream, Content<Stream>> contentProvider = null;
            IPipe<Iori, Content<Stream>> contentIoProvider = null;

            IPipe<Content<Stream>, IThing> contentThingProvider = null;
        }

        public void DataProviders () {
            IPipe<Content<Stream>, GraphCursor<IThing,ILink>> contentThingGraphProvider = null;
            IPipe<IEnumerable<IThing>, IThingGraph> mergeGraph = null;
        }

        public void ContentDiggProviders () {
            var provider = new ContentDiggPool();
            var digger = new ContentDigger((so,si)=> si);
            provider.Add(digger);
            var content = new Content<Stream>();
            provider.Use(content, content);
        }
    }
}