using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limada.Model;
using Limaki.Model.Content.Providers;

namespace Limaki.IOC {
    public class LimakiCoreContextRecourceLoader : ContextRecourceLoader {
        
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<ICompressionWorker, Limaki.Compression.CompressionWorker> ();
            context.Factory.Add<IThingFactory, ThingFactory>();
			
			var providers = Registry.Pool.TryGetCreate<ContentProviders>();
			providers.Add(new RtfContentProvider());
			providers.Add(new HtmlContentProvider());
			providers.Add(new ImageContentProvider());
        }

        public virtual void LoadCompression(IApplicationContext context) {
            Assembly ass = Assembly.Load("Limaki.Compression");
            foreach (Type type in ass.GetTypes()) {
                if (Reflector.Implements(type, typeof(ICompressionWorker))) {
                    context.Factory.Add(typeof(ICompressionWorker), type);
                    break;
                }
            }
        }
    }
}