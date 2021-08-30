using System.IO;
using System.Linq;
using Limaki.Common.IOC;
using Limaki.Contents.IO;
using Limaki.Contents.Text;

namespace Limaki.Contents {

    public class ViewContentResourceLoader : IContextResourceLoader {

        public static bool Applied { get; set; }

        public virtual void ApplyResources (IApplicationContext context) {

            if (Applied)
                return;
            
            var mimeFingerPrints = context.Pooled<MimeFingerPrints> ();
            mimeFingerPrints.SynonymFormats ("DeviceIndependentBitmap", new ImageContentSpot ().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);
            mimeFingerPrints.SynonymFormats ("CF_DIB", new ImageContentSpot ().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);
            mimeFingerPrints.SynonymFormats ("text/x-markdown", new MarkdownContentSpot ().ContentSpecs.First().MimeType);
            mimeFingerPrints.SynonymFormats ("text/richtext", "application/rtf");
            mimeFingerPrints.PreferedFormats (new string[] { "Star Object Descriptor (XML)" }, new string[] { "Rich Text Format" }); // if OpenOffice, then take Rtf 
            mimeFingerPrints.PreferedFormats (new string[] { "text/x-moz-url" }, new string[] { "text/uri-list" });
            mimeFingerPrints.PreferedFormats (new string[] { "text/html" }, new string[] { "html format" }); 

        
            var contentDiggPool = context.Pooled<ContentDiggPool> ();
            contentDiggPool.Add (new ImageContentDigger ());

            var converterPool = context.Pooled<ConverterPool<Stream>> ();
            converterPool.Add (new TextHtmlConverter ());
            converterPool.Add (new MarkDownHtmlConverter ());
            converterPool.Add (new RtfHtmlConverter ());

            Applied = true;
        }
    }
}