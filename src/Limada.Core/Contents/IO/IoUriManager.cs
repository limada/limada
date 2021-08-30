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

using System;
using System.IO;
using Limaki.Common;

namespace Limaki.Contents.IO {

    /// <summary>
    /// manages core input-output-operations of 
    /// IPipe#TSource, TSink#-implementations
    /// uses Uri as resource identifier
    /// </summary>
    public class IoUriManager<TSource, TSink> : IoUriManager<TSource, TSink, ContentIoPool<TSource, TSink>> { }

    /// <summary>
    /// manages core input-output-operations of 
    /// IPipe#TSource, TSink#-implementations
    /// uses Uri as resource identifier
    /// </summary>
    public class IoUriManager<TSource, TSink, TIoPool> : IoManager<TSource, TSink, TIoPool>
           where TIoPool : ContentIoPool<TSource, TSink> {

        public Action Close { get; set; }

        public virtual TSink ReadSink (Uri uri, TSink sink) {
            var sinkIo = GetSinkIO(uri, IoMode.Read);
            if (sinkIo != null && sinkIo.IoMode.HasFlag(IoMode.Read)) {
                OnClose();
                try {
                    ConfigureSinkIo?.Invoke (sinkIo);
                    this.AttachProgress(sinkIo as IProgress);

                    var uriSink = sinkIo as IPipe<Uri, TSink>;
                    if (uriSink != null) {
                        sink = uriSink.Use(uri, sink);
                        if (sink != null && SinkIn != null) {
                            SinkIn(sink);
                        }
                        OnClose();
                    } else {
                        var streamSink = sinkIo as IPipe<Stream,TSink>;
                        if (streamSink != null) {
                            var filename = IoUtils.UriToFileName(uri);
                            var file = new FileStream (filename, FileMode.Open);
                            try {
                                sink = streamSink.Use (file, sink);
                                if (sink != null && SinkIn != null) {
                                    SinkIn (sink);
                                }
                            } finally {
                                file.Close ();
                            }
                        }
                    }
                } catch (Exception e) {
                    sink = default(TSink);
                }
            }
            return sink;
        }

        public virtual TSink ReadSink (Uri uri) {
            return ReadSink(uri, default(TSink));
        }

        public virtual void WriteSink (Uri uri) {
            WriteSink(SinkOut(), uri);
        }

        public virtual bool WriteSink (TSink sink, Uri uri) {
            if (sink == null)
                return false;

            var sinkIo = GetSinkIO(uri, IoMode.Write);
  
            bool result = false;
            if (sinkIo != null && sinkIo.IoMode.HasFlag(IoMode.Write)) {
                try {
                    this.AttachProgress(sinkIo as IProgress);
                    if (ConfigureSinkIo != null)
                        ConfigureSinkIo(sinkIo);

                    var uriSink = sinkIo as IPipe<TSink, Uri>;
                    if (uriSink !=null) {
                        uriSink.Use(sink, uri);
                        OnClose();
                        result = true;
                    } else {
                        var streamSink = sinkIo as IPipe<TSink, Stream>;
                        if (streamSink != null) {
                            var filename = IoUtils.UriToFileName(uri);
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


        public virtual void OnClose () {
            if (Close != null) {
                Close();
                Close = null;
            }
        }

    }
}