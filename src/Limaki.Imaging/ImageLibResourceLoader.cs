using Limaki.Common;
using Limaki.Common.IOC;
using System.Collections.Generic;
using System.IO;
using Limaki.Contents;

namespace Limaki.ImageLibs {

    public class ImageLibResourceLoader : IContextResourceLoader {

        public void ApplyResources (IApplicationContext context) {
            var converterPool = context.Pooled<ConverterPool<Stream>>();
            converterPool.Add (new ImageConverter());

        }
    }
}