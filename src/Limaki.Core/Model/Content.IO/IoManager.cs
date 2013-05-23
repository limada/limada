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

namespace Limaki.Model.Content.IO {

    public class IoManager<TSource, TSink> {

        public string DefaultExtension { get; set; }
        public Action<string, int, int> Progress { get; set; }

        public virtual ISinkIo<TSource> GetSinkIO(IoInfo info, InOutMode mode) {
            return GetSinkByExtension(info.Extension, mode);
        }

        public virtual ISinkIo<TSource> GetSinkIO (Uri uri, InOutMode mode) {
            if (uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                return GetSinkByExtension(Path.GetExtension(filename).Trim('.'), mode);
            }
            return null;
        }

        public virtual ISinkIo<TSource> GetSinkByExtension (string extension, InOutMode mode) {
            var result = Provider.Find(extension, mode);
            if (result != null)
                result.Progress = this.Progress;
            return result;
        }

        public virtual ContentInfo GetContentInfo (Content stream) {
            var sink = Provider.Find(stream.StreamType);
            if (sink != null) {
                return sink.InfoSink.SupportedContents.Where(e => e.ContentType == stream.StreamType).First();
            }
            return null;
        }

        private string _saveFilter = null;
        public string WriteFilter { get { return _saveFilter ?? (_saveFilter = GetFilter(InOutMode.Write)); } }

        private string _readFilter = null;
        public string ReadFilter { get { return _readFilter ?? (_readFilter = GetFilter(InOutMode.Read)); } }

        private IoProvider<TSource,TSink> _provider = null;
        public IoProvider<TSource,TSink> Provider { get { return _provider ?? (_provider = Registry.Pool.TryGetCreate<IoProvider<TSource,TSink>>()); } }

        public string GetFilter (ContentInfo info, out string ext) {
            ext = info.Extension;
            if (ext[0] != '.')
                ext = "." + ext;
            return info.Description + "|*" + ext + "|";
        }

        public string GetFilter(InOutMode mode) {
            string filter = "";
            string defaultFilter = null;
            foreach (var sink in Provider) {
                if (sink.IoMode == mode) {
                    foreach (var info in sink.InfoSink.SupportedContents) {
                        string ext = null;
                        var f = GetFilter(info, out ext);
                       
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


        /// <summary>
        /// called after sink is read
        /// </summary>
        public Action<TSink> SinkIn { get; set; }

        /// <summary>
        /// called to get the sink to be written
        /// </summary>
        public Func<TSink> SinkOut { get; set; }

        public Action<ISinkIo<TSource>> ConfigureSinkIo { get; set; }

        public virtual TSink ReadSink (Uri uri) {
            var sinkIo = GetSinkIO(uri, InOutMode.Read);
            TSink result = default(TSink);
            if (sinkIo != null && sinkIo.IoMode.HasFlag(InOutMode.Read)) {
                OnClose();
                try {
                    var streamSink = sinkIo as ISink<Uri, TSink>;
                    if (streamSink != null) {
                        result = streamSink.Use(uri);
                        if (result != null && SinkIn != null) {
                            SinkIn(result);
                        }
                        OnClose();
                    }
                } catch (Exception e) {
                    result = default(TSink);
                }
            }
            return result;
        }

        public virtual void WriteSink (Uri uri) {
            WriteSink(SinkOut(),uri);
        }

        public virtual bool WriteSink (TSink sink, Uri uri) {
            if (sink == null)
                return false;

            var sinkIo = GetSinkIO(uri, InOutMode.Write);
            
            if (ConfigureSinkIo != null)
                ConfigureSinkIo(sinkIo);

            bool result = false;
            if (sinkIo != null && sinkIo.IoMode.HasFlag(InOutMode.Write)) {
                try {
                    var uriSink = sinkIo as ISink<TSink, Uri>;
                    if (uriSink !=null) {
                        uriSink.Use(sink, uri);
                        OnClose();
                        result = true;
                    } else {
                        var streamSink = sinkIo as ISink<TSink, Stream>;
                        if (streamSink != null) {
                            var filename = IOUtils.UriToFileName(uri);
                            var file = new FileStream(filename, FileMode.Create);
                            streamSink.Use(sink, file);
                            file.Close();
                            result = true;
                        }
                    }
                } catch (Exception e) {
                    result = false;
                }
            }
            return result;
        }

        public Action Close { get; set; }
        public virtual void OnClose () {
            if (Close != null) {
                Close();
                Close = null;
            }
        }

        
    }
}