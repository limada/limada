using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limada.Model;

using Limaki.Model.Content.IO;
using System.IO;

namespace Limaki.IOC {

    public class LimakiCoreContextRecourceLoader : ContextRecourceLoader {
        
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<ICompressionWorker, Limaki.Compression.CompressionWorker> ();
            context.Factory.Add<IThingFactory, ThingFactory>();
			
			var contentIoPool = Registry.Pool.TryGetCreate<ContentIoPool<Stream, Content<Stream>>>();
            contentIoPool.Add(new HtmlContentStreamIo()); 
            contentIoPool.Add(new RtfContentStreamIo()); 
            contentIoPool.Add(new ImageContentStreamIo());
            contentIoPool.Add(new PdfContentStreamIo());

            var diggProvider = Registry.Pool.TryGetCreate<ContentDiggPool>();
            diggProvider.Add(new HtmlContentDigger());
        }

    }
}