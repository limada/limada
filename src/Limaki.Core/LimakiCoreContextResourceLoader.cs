using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Common.IOC;
using Limada.Model;

using Limaki.Contents.IO;
using System.IO;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki {

    public class LimakiCoreContextResourceLoader : ContextResourceLoader {
        
        public override void ApplyResources(IApplicationContext context) {

            context.Factory.Add<ICompressionWorker, CompressionWorker> ();

            context.Factory.Add<IGraphModelFactory<IGraphEntity, IGraphEdge>, GraphEntityFactory> ();
            
            var streamContentIoPool = Registry.Pooled<StreamContentIoPool>();
            streamContentIoPool.Add (new ImageStreamContentIo ());
            streamContentIoPool.Add(new TextStreamContentIo());
            streamContentIoPool.Add (new RtfStreamContentIo ()); 
            streamContentIoPool.Add(new HtmlStreamContentIo()); 
            streamContentIoPool.Add(new PdfStreamContentIo());

            var contentDiggPool = Registry.Pooled<ContentDiggPool>();
            contentDiggPool.Add(new TextContentDigger());
            contentDiggPool.Add(new HtmlContentDigger());

        }

    }
}