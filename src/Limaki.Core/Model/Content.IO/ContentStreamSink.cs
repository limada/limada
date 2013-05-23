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

namespace Limaki.Model.Content.IO {

    public class ContentInStreamSink : StreamSink, ISink<Stream, Content<Stream>>, ISink<Stream, ContentInfo>, ISink<Uri, Content<Stream>> {

        protected ContentInStreamSink(ContentInfoSink supportedContents): base(supportedContents) {
            this.IoMode = InOutMode.Read;
        }

        public virtual Content<Stream> Read(Stream stream, ContentInfo info) {
            if (info != null) {
                return new Content<Stream>(
                    stream,
                    info.Compression,
                    info.ContentType);
            }
            return null;
        }

        public virtual Content<Stream> Read(Uri uri) {
            return Read(uri, new Content<Stream>());
        }

        public virtual Content<Stream> Read(Uri uri, Content<Stream> sink) {
            var result = default(Content<Stream>);
            if (IoMode.HasFlag(InOutMode.Read) && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Open);

                result = Use(file, sink);

                // test if there is a provider with file's extension:
                if (result == null) {
                    var info = InfoSink.Info(Path.GetExtension(filename).Replace(".", ""));
                    if (info.Magics == null || info.Magics.Length == 0) {
                        result = Read(file, info);
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



        public virtual new Content<Stream> Use(Stream stream) {
            return Read(stream, InfoSink.Use(stream));
        }

        public virtual Content<Stream> Use(Stream source, Content<Stream> sink) {
            return Read(source, InfoSink.Use(source));
        }

        Content<Stream> ISink<Uri, Content<Stream>>.Use(Uri source) {
            return Read(source);
        }

        Content<Stream> ISink<Uri, Content<Stream>>.Use(Uri source, Content<Stream> sink) {
            return Read(source, sink);
        }
    }

    public class ContentOutStreamSink : StreamOutSink, ISink<Content<Stream>, Uri> {

        protected ContentOutStreamSink (ContentInfoSink supportedContents)
            : base(supportedContents) {
            this.IoMode = InOutMode.Write;
        }

        public virtual Uri Use (Content<Stream> content, Uri uri) {
            var result = base.Use(content.Data, uri);
            return result;
        }

        public virtual Uri Use (Content<Stream> source) {
            //TODO: use source.Source to make a Uri
            return Use(source, null);
        }
    }

}