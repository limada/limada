using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limada.Model;

namespace Limaki.Context {
    public class LimakiCoreContextRecourceLoader : Common.ContextRecourceLoader {
        
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<ICompressionWorker, Limaki.Compression.CompressionWorker> ();
            context.Factory.Add<IThingFactory, ThingFactory>();
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