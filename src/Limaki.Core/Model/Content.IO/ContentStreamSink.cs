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

    public interface IContentStreamSink : ISink<Stream, ContentInfo> {
        InOutMode IoMode { get; }
    }

    public class ContentStreamSink: ISink<Stream, ContentInfo> {
        protected ContentStreamSink (ContentInfoSink supportedContents) {
            this.ContentInfo = supportedContents;
        }
        public virtual InOutMode IoMode { get; protected set; }
        public virtual ContentInfoSink ContentInfo { get; protected set; }

        ContentInfo ISink<Stream, ContentInfo>.Sink (Stream source) {
            return ContentInfo.Sink(source);
        }

        ContentInfo ISink<Stream, ContentInfo>.Sink (Stream source, ContentInfo sink) {
            return ContentInfo.Sink(source, sink);
        }
    }

    public class ContentOutStreamSink : ContentStreamSink, ISink<Content<Stream>, Uri> {

        protected ContentOutStreamSink(ContentInfoSink supportedContents):base(supportedContents) {
            this.IoMode = InOutMode.Write;
        }

        public virtual Uri Sink (Content<Stream> content, Uri uri) {
            if (uri == null)
                throw new ArgumentException("Uri must not be null");

            if (IoMode.HasFlag(InOutMode.Write) && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Create);
                var target = new BufferedStream(file);
                var bufferSize = 1024 * 1024;
                var buffer = new byte[bufferSize];
                var source = content.Data;
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
            return uri;
        }

        public virtual Uri Sink (Content<Stream> source) {
            return Sink(source, null);
        }
    }

    public class ContentInStreamSink : ContentStreamSink,ISink<Stream, Content<Stream>>, ISink<Stream, ContentInfo> {

        protected ContentInStreamSink (ContentInfoSink supportedContents):base(supportedContents) {
            this.IoMode = InOutMode.Read;
        }

        public virtual Content<Stream> ContentOf (Stream stream, ContentInfo info) {
            if (info != null) {
                return new Content<Stream>(
                    stream,
                    info.Compression,
                    info.ContentType);
            }
            return null;
        }

        public virtual Content<Stream> ContentOf (Uri uri) {
            var result = default(Content<Stream>);
            if (IoMode.HasFlag(InOutMode.Read) && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Open);

                result = Sink(file);

                // test if there is a provider with file's extension:
                if (result == null) {
                    var info = ContentInfo.Info(Path.GetExtension(filename).Replace(".", ""));
                    if (info.Magics == null || info.Magics.Length == 0) {
                        result = ContentOf(file, info);
                    }
                }

                if (result != null) {
                    if (result.Source == null)
                        result.Source = uri.AbsoluteUri;
                    if (result.Description == null)
                        result.Description = uri.Segments[uri.Segments.Length - 1];
                } else {
                    file.Close();
                }
            }
            return result;
        }

        

        public virtual Content<Stream> Sink (Stream stream) {
            return ContentOf(stream, ContentInfo.Sink(stream));
        }

        public virtual Content<Stream> Sink (Stream source, Content<Stream> sink) {
            return SinkExtensions.Sink(source, sink, s => Sink(s));
        }


    }
}