using System;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.Contents {

    /// <summary>
    /// class to register fingerprints of ContentInfo.MimeTypes
    /// key = <see cref="TransferDataType.Id"/>, value = <see cref="Content.ContentType"/>
    /// </summary>
    public class MimeFingerPrints {
        /// <summary>
        /// order is important, the first wins!
        /// 
        ///
        /// </summary>
        /// <param name="transferIds"></param>
        /// <param name="allowedIds"></param>
        public virtual void PreferedFormats (IEnumerable<string> transferIds, IEnumerable<string> allowedIds) {
            _preferedFormats.Add (Tuple.Create<IEnumerable<string>, IEnumerable<string>> (
                transferIds.Select (s => s.ToLower ()).ToArray (),
                allowedIds.Select (s => s.ToLower ()).ToArray ()
                ));
        }

        public virtual IEnumerable<string> Prefer (IEnumerable<string> transferIds) {
            var ids = transferIds.Select (s => s.ToLower ()).ToArray ();
            var prefered = _preferedFormats
                .Where (f => f.Item1.Intersect (ids).Any () && f.Item2.Intersect(ids).Any())
                .FirstOrDefault();
            if (prefered != null )
                return prefered.Item2;
            return transferIds;
        }

        private ICollection<Tuple<IEnumerable<string>, IEnumerable<string>>> _preferedFormats = new List<Tuple<IEnumerable<string>, IEnumerable<string>>> ();
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