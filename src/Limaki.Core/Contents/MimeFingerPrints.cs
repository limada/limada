using System.Collections.Generic;

namespace Limaki.Contents {

    /// <summary>
    /// class to register fingerprints of ContentInfo.MimeTypes
    /// key = <see cref="TransferDataType.Id"/>, value = <see cref="Content.ContentType"/>
    /// </summary>
    public class MimeFingerPrints {
        /// <summary>
        /// order is important, the first wins!
        /// 
        /// PreferedFormats({"Star Object Descriptor (XML)"}, {"Rich Text Format"}) // if OpenOffice, then take Rtf 
        /// PreferedFormats({"text/x-moz-url"}, {"text/uri-list"})
        /// PreferedFormats({"text/_moz_htmlinfo"}, {"HTML Format"}) // if Firefox, then take HTML
        /// </summary>
        /// <param name="transferId"></param>
        /// <param name="allowedIds"></param>
        public virtual void PreferedFormats (IEnumerable<string> fingerprints, IEnumerable<string> allowedIds) {

        }

        public virtual IEnumerable<string> Prefer (IEnumerable<string> transferIds) {
            // if (transferIds.Contains(fingerprints)) return allowedId
            return transferIds;
        }

        private IDictionary<string, string> _synonyms = new Dictionary<string, string> ();

        /// <summary>
        /// eg. SynonymFormats("CF_DIB", "DeviceIndependentBitmap") means: translate all CF_DIB to DeviceIndependentBitmap
        /// </summary>
        /// <param name="source"></param>
        /// <param name="synonym"></param>
        public virtual void SynonymFormats (string source, string synonym) {
            _synonyms[source]= synonym;
        }

        public virtual string Synonym (string source) {
            string result = null;
            if (_synonyms.TryGetValue (source, out result))
                return result;
            return source;
        }

    }
}