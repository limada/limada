/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Data;
using System;
using System.IO;
using System.Linq;
using Limaki.Contents;

namespace Limaki.Contents.IO {

    /// <summary>
    /// manages core input-output-operations of 
    /// IPipe#TSource, TSink#-implementations
    /// uses ContentIoPool#TSource, TSink# to find the ContentIo's
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSink"></typeparam>
    public class IoManager<TSource, TSink> : IoManager<TSource, TSink, ContentIoPool<TSource, TSink>> { }

    /// <summary>
    /// manages core input-output-operations of 
    /// IPipe#TSource, TSink#-implementations
    /// uses Iori as resource identifier
    /// uses a specific TContentIoPool to find the ContentIo's
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSink"></typeparam>
    public class IoManager<TSource, TSink, TContentIoPool> : IProgress 
        where TContentIoPool : ContentIoPool<TSource, TSink> {

        /// <summary>
        /// called after sink is read
        /// </summary>
        public Action<TSink> SinkIn { get; set; }

        /// <summary>
        /// called to get the sink to be written
        /// </summary>
        public Func<TSink> SinkOut { get; set; }

        /// <summary>
        /// called after a ContentIo is found, and before using it
        /// </summary>
        public Action<IContentIo<TSource>> ConfigureSinkIo { get; set; }

        public string DefaultExtension { get; set; }
        public Action<string, int, int> Progress { get; set; }

        protected TContentIoPool _pool = null;
        public virtual TContentIoPool ContentIoPool { get { return _pool ?? (_pool = Registry.Pooled<TContentIoPool>()); } }

        #region providing ContentIo

        public virtual IContentIo<TSource> GetSinkIO(Iori iori, IoMode mode) {
            return GetSinkByExtension(iori.Extension, mode);
        }

        public virtual IContentIo<TSource> GetSinkIO(Uri uri, IoMode mode) {
            if (uri.IsFile) {
                return GetSinkIO(IoUtils.UriToFileName(uri), mode);
            }
            return null;
        }

        public virtual IContentIo<TSource> GetSinkIO (string filename, IoMode mode) {
            return GetSinkByExtension(Path.GetExtension(filename).Trim('.'), mode);
        }

        public virtual IContentIo<TSource> GetSinkByExtension(string extension, IoMode mode) {
            var result = ContentIoPool.Find(extension.ToLower(), mode);
            this.AttachProgress(result as IProgress);
            return result;
        }

        public IContentIo<TSource> GetSinkIO (long contentType, IoMode mode) {
            var result = ContentIoPool.Find(contentType, mode);
            this.AttachProgress(result as IProgress);
            return result;
        }

        public virtual ContentInfo GetContentInfo(Content content) {
            var sink = ContentIoPool.Find(content.ContentType);
            if (sink != null) {
                return sink.Detector.ContentSpecs.Where(e => e.ContentType == content.ContentType).First();
            }
            return null;
        }

        #endregion

        #region Dialog-Filter-Handling

        private string _saveFilter = null;
        public string WriteFilter { get { return _saveFilter ?? (_saveFilter = GetFilter(IoMode.Write)); } }

        private string _readFilter = null;
        public string ReadFilter { get { return _readFilter ?? (_readFilter = GetFilter(IoMode.Read)); } }

        public string GetFilter(ContentInfo info, out string ext) {
            ext = info.Extension;
            if (ext[0] != '.')
                ext = "." + ext;
            return info.Description + "|*" + ext + "|";
        }

        public string GetFilter(IoMode mode) {
            string filter = "";
            string defaultFilter = null;
            foreach (var sink in ContentIoPool) {
                if (sink.IoMode.HasFlag (mode)) {
                    foreach (var info in sink.Detector.ContentSpecs) {
                        string ext = null;
                        var f = GetFilter (info, out ext);

                        if (DefaultExtension != null && ext == "." + DefaultExtension)
                            defaultFilter = f;
                        else
                            filter += f;

                    }
                }
            }
            if (defaultFilter != null) {
                filter = defaultFilter + filter;
            }

            return filter;

        }

        #endregion
    }
}