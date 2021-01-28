/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Contents;
using Limaki.Contents.IO;
using Xwt;

namespace Limaki.View.DragDrop {

    /// <summary>
    /// this class handles clipboard and drag-drop
    /// </summary>
    public class TransferDataManager {

        private StreamContentIoPool _contentPool;
        public StreamContentIoPool ContentIoPool => _contentPool ??= Registry.Pooled<StreamContentIoPool> ();

        private MimeFingerPrints _mimeFingerPrints;

        /// <summary>
        /// registered fingerprints of ContentInfo.MimeTypes
        /// register with <see cref=" Registry.Pool.TryGetCreate{MimeFingerPrints}()"/>
        /// </summary>
        public MimeFingerPrints MimeFingerPrints => _mimeFingerPrints ??= Registry.Pooled<MimeFingerPrints> ();

        private TransferContentTypes _transferContentTypes;

        /// <summary>
        /// register the type values for Clipboard and DragDrop operations here
        /// </summary>
        public TransferContentTypes TransferContentTypes => _transferContentTypes ??= new TransferContentTypes ();

        public virtual IEnumerable<TransferDataType> DataTypes {
            get {
                foreach (var t in TransferContentTypes.DataTypes) {
                    var syn = MimeFingerPrints.Synonym (t.Id);
                    var result = t;
                    //if (syn != t.Id) {
                    //    result = TransferDataType.FromId (syn);
                    //}
                    yield return result;
                    //if (result.Id != result.Id.ToUpper ())
                    //    yield return TransferDataType.FromId (result.Id.ToUpper ());
                }
            }
        }

        public virtual IEnumerable<ContentInfo> InfoOf (IContentIo<Stream> io, IEnumerable<TransferDataType> sources) {
            var result = new List<ContentInfo> ();
            foreach (var source in sources) {
                var sourceId = MimeFingerPrints.Synonym (source.Id);
                Func<IContentIo<Stream>, IEnumerable<ContentInfo>> lookUp = null;

                if (TransferContentTypes.TryGetValue (sourceId.ToLower (), out var contentType)) {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Where (info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Where (info => info.MimeType == sourceId);
                }

                result.AddRange (lookUp (io));
            }

            return result;
        }

        public virtual IEnumerable<Tuple<TransferDataType, IContentIo<Stream>>> SinksOf (IEnumerable<TransferDataType> sources) {
            var result = new List<Tuple<TransferDataType, IContentIo<Stream>>> ();
            var prefered = MimeFingerPrints.Prefer (sources.Select (s => s.Id))
                .ToArray ();
            sources = sources.Where (s => prefered.Contains (MimeFingerPrints.Synonym (s.Id).ToLower ()));
            foreach (var source in sources) {
                var sourceId = MimeFingerPrints.Synonym (source.Id.ToLower ());
                Func<IContentIo<Stream>, bool> lookUp = null;
                if (TransferContentTypes.TryGetValue (sourceId.ToLower (), out var contentType)) {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Any (info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                        .Any (info => info.MimeType == sourceId);
                }

                var done = new Set<long> ();

                foreach (var sinkIo in ContentIoPool.Where (lookUp)
                    .Where (io => !io.Detector.ContentSpecs.Any (info => done.Contains (info.ContentType)))) {
                    result.Add (Tuple.Create (source, sinkIo));
                }
            }

            return result.OrderBy (r => ContentIoPool.Priority (r.Item2));
        }

        public void RegisterSome () {
            TransferContentTypes["html"] = ContentTypes.HTML;
            TransferContentTypes["text"] = ContentTypes.Text;
            TransferContentTypes["rtf"] = ContentTypes.RTF;
            TransferContentTypes[TransferDataType.Uri.Id] = ContentTypes.Uri;

            var pool = Registry.Pooled<StreamContentIoPool> ();
            foreach (var f in pool.ContentInfos) {
                TransferContentTypes[f.MimeType] = f.ContentType;
            }
        }

        public IContentIo<Stream> SinkOf (string extension) =>
            ContentIoPool.FirstOrDefault (sinkIo => sinkIo.Detector.ContentSpecs
                .Any (info => info.Extension == extension));

        public IContentIo<Stream> SinkOf (Stream stream) => ContentIoPool.FirstOrDefault (s => s.Use (stream) != null);
    }

    /// <summary>
    /// class to register the type values for Clipboard and DragDrop operations
    /// key = <see cref="TransferDataType.Id"/>, value = <see cref="Content.ContentType"/>
    /// </summary>
    public class TransferContentTypes : Dictionary<string, long> {
        public virtual IEnumerable<TransferDataType> DataTypes => Keys.Select (TransferDataType.FromId);
    }

}