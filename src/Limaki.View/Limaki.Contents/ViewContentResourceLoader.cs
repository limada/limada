using System.Linq;
using Limaki.Common.IOC;
using Limaki.Contents.IO;

namespace Limaki.Contents {

    public class ViewContentResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {

            var mimeFingerPrints = context.Pooled<MimeFingerPrints> ();
            mimeFingerPrints.SynonymFormats ("DeviceIndependentBitmap", new ImageContentSpot ().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);
            mimeFingerPrints.SynonymFormats ("CF_DIB", new ImageContentSpot ().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);

            var contentDiggPool = context.Pooled<ContentDiggPool> ();
            contentDiggPool.Add (new ImageContentDigger ());
        }
    }
}