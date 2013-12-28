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
			
			var streamProviders = Registry.Pool.TryGetCreate<IoProvider<Stream, Content<Stream>>>();
            streamProviders.Add(new HtmlContentStreamIo()); 
            streamProviders.Add(new RtfContentStreamIo()); 
            streamProviders.Add(new ImageContentStreamIo());
            streamProviders.Add(new PdfContentStreamIo());

            var diggProvider = Registry.Pool.TryGetCreate<ContentDiggProvider>();
            diggProvider.Add(new HtmlContentDigger());
        }

    }
}