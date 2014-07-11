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

using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Contents;
using Limaki.Contents.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xwt;

namespace Limaki.View.DragDrop {
    /// <summary>
    /// this class handles clipboard and drag-drop
    /// </summary>
    public class TransferDataManager {

        private StreamContentIoPool _contentPool;
        public StreamContentIoPool ContentIoPool { get { return _contentPool ?? (_contentPool = Registry.Pooled<StreamContentIoPool>()); } }

        private MimeFingerPrints _mimeFingerPrints;

        /// <summary>
        /// registered fingerprints of ContentInfo.MimeTypes
        /// register with <see cref=" Registry.Pool.TryGetCreate{MimeFingerPrints}()"/>
        /// </summary>
        public MimeFingerPrints MimeFingerPrints { get { return _mimeFingerPrints ?? (_mimeFingerPrints = Registry.Pooled<MimeFingerPrints>()); } }

        private TransferContentTypes _transferContentTypes;

        /// <summary>
        /// register the type values for Clipboard and DragDrop operations here
        /// </summary>
        public TransferContentTypes TransferContentTypes { get { return _transferContentTypes ?? (_transferContentTypes = new TransferContentTypes()); } }

        public virtual IEnumerable<TransferDataType> DataTypes {
            get {
                foreach (var t in this.TransferContentTypes.DataTypes) {
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
            foreach (var source in sources) {
                var sourceId = MimeFingerPrints.Synonym (source.Id);
                long contentType = 0;
                Func<IContentIo<Stream>, IEnumerable<ContentInfo>> lookUp = null;
                if (TransferContentTypes.TryGetValue (sourceId.ToLower(), out contentType)) {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                                           .Where (info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                                           .Where (info => info.MimeType == sourceId);
                }
                var done = new Set<long> ();

                return lookUp(io);
                   
            }
            return null;
        }

        public virtual IEnumerable<Tuple<TransferDataType, IContentIo<Stream>>> SinksOf (IEnumerable<TransferDataType> sources) {
            var result = new List<Tuple<TransferDataType, IContentIo<Stream>>> ();
            foreach (var source in sources) {
                var sourceId = MimeFingerPrints.Synonym (source.Id);
                long contentType = 0;
                Func<IContentIo<Stream>, bool> lookUp = null;
                if (TransferContentTypes.TryGetValue (sourceId.ToLower(), out contentType)) {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                                           .Any (info => info.ContentType == contentType);
                } else {
                    lookUp = sinkIo => sinkIo.Detector.ContentSpecs
                                           .Any (info => info.MimeType == sourceId);
                }
                var done = new Set<long>();

                foreach (var sinkIo in ContentIoPool.Where (lookUp)
                    .Where (io => ! io.Detector.ContentSpecs.Any (info => done.Contains (info.ContentType)))) {
                    result.Add(Tuple.Create (source, sinkIo));
                }
            }
            return result.OrderBy (r => ContentIoPool.Priority (r.Item2));
        }



        // move this to Resourceloader of OS:
        public void RegisterSome() {
            TransferContentTypes.Add ("html", ContentTypes.HTML);
            TransferContentTypes.Add ("text", ContentTypes.Text);
            TransferContentTypes.Add ("rtf", ContentTypes.RTF);
            //...
        }

        #region not used

        private TransferContentPool _transferContentPool;

        /// <summary>
        /// register special ContentIo's for Clipboard and DragDrop operations here
        /// they override 
        /// </summary>
        private TransferContentPool _TransferContentPool { get { return _transferContentPool ?? (_transferContentPool = new TransferContentPool()); } }

        /// <summary>
        /// class to register special ContentIo's for Clipboard and DragDrop operations
        /// they override the common <see cref="StreamContentIoPool"/>
        /// </summary>
        public class TransferContentPool : ContentIoPool<Stream, Content<Stream>> {
        }

        #endregion

    }



    /// <summary>
    /// class to register the type values for Clipboard and DragDrop operations
    /// key = <see cref="TransferDataType.Id"/>, value = <see cref="Content.ContentType"/>
    /// </summary>
    public class TransferContentTypes : Dictionary<string, long> {
        public virtual IEnumerable<TransferDataType> DataTypes {
            get {
                foreach (var c in this.Keys)
                    yield return TransferDataType.FromId (c);
            }
        }
    }

    
}