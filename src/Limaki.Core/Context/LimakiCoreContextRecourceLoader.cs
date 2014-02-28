using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limada.Model;

using Limaki.Contents.IO;
using System.IO;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.IOC {

    public class LimakiCoreContextRecourceLoader : ContextRecourceLoader {
        
        public override void ApplyResources(IApplicationContext context) {

            context.Factory.Add<ICompressionWorker, Limaki.Compression.CompressionWorker> ();

            context.Factory.Add<IThingFactory, ThingFactory> ();
            context.Factory.Add<IGraphModelFactory<IThing, ILink>, ThingFactory> ();

            context.Factory.Add<IGraphModelFactory<IGraphEntity, IGraphEdge>, GraphEntityFactory> ();
            
            var streamContentIoPool = Registry.Pool.TryGetCreate<StreamContentIoPool>();
            streamContentIoPool.Add(new TextStreamContentIo());
            streamContentIoPool.Add (new RtfStreamContentIo ()); 
            streamContentIoPool.Add(new HtmlStreamContentIo()); 
            streamContentIoPool.Add(new ImageStreamContentIo());
            streamContentIoPool.Add(new PdfStreamContentIo());

            var contentDiggPool = Registry.Pool.TryGetCreate<ContentDiggPool>();
            contentDiggPool.Add(new TextContentDigger());
            contentDiggPool.Add(new HtmlContentDigger());

        }

    }
}