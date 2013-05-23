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
using System.Collections.Generic;
using System.IO;
using Limaki.Model.Content;

namespace Limaki.Model.Content.IO {

    public class StreamSink : SinkIo<Stream> {

        protected StreamSink(ContentInfoSink supportedContents)
            : base(supportedContents) {
        }

        public override ContentInfo Use(Stream source) {
            return InfoSink.Use(source);
        }

        public override ContentInfo Use(Stream source, ContentInfo sink) {
            return InfoSink.Use(source, sink);
        }

        public override bool Supports(Stream source) {
            return InfoSink.Supports(source);
        }

    }

    public class StreamOutSink : StreamSink, ISink<Stream, Uri> {
        protected StreamOutSink(ContentInfoSink supportedContents)
            : base(supportedContents) {
            this.IoMode = InOutMode.Write;
        }

        public Uri Use(Stream source, Uri sink) {
            {
                if (sink == null)
                    throw new ArgumentException("Uri must not be null");

                if (IoMode.HasFlag(InOutMode.Write) && sink.IsFile) {
                    var filename = IOUtils.UriToFileName(sink);
                    var file = new FileStream(filename, FileMode.Create);
                    var target = new BufferedStream(file);
                    var bufferSize = 1024 * 1024;
                    var buffer = new byte[bufferSize];
                    var oldPos = source.Position;
                    int readByte = 0;
                    int position = 0;

                    long endpos = source.Length - 1;
                    while (position < endpos) {
                        readByte = source.Read(buffer, 0, bufferSize);
                        target.Write(buffer, 0, readByte);
                        position += readByte;
                    }

                    target.Flush();
                    target.Close();
                    source.Position = oldPos;
                }
                return sink;
            }
        }

        public new Uri Use(Stream source) {
            return Use(source, null);
        }
    }


    public class StreamInSink : StreamSink, ISink<Uri, Stream>, ISink<Stream, ContentInfo> {

        protected StreamInSink(ContentInfoSink supportedContents)
            : base(supportedContents) {
            this.IoMode = InOutMode.Read;
        }

        public Stream Use(Uri uri) {
            var result = default(Stream);
            if (IoMode.HasFlag(InOutMode.Read) && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Open);
            }
            return result;
        }

        public Stream Use(Uri source, Stream sink) {
            return sink;
        }

        ContentInfo ISink<Stream, ContentInfo>.Use(Stream source) {
            return this.InfoSink.Use(source);
        }

        ContentInfo ISink<Stream, ContentInfo>.Use(Stream source, ContentInfo sink) {
            return this.InfoSink.Use(source, sink);
        }
    }

}