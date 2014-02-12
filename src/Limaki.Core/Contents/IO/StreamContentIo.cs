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
using Limaki.Model.Content;

namespace Limaki.Contents.IO {
    /// <summary>
    /// reads and writes Content<Stream> based on a Uri
    /// uses ContentInfo to fill the Content
    /// uses IoMode
    /// </summary>
    public class StreamContentIo : StreamIo, IPipe<Uri, Content<Stream>>, IPipe<Content<Stream>, Uri> {

        protected StreamContentIo (ContentDetector detector) : base(detector) { } 

        public virtual Content<Stream> Read (Stream stream, ContentInfo info) {
            if (info != null) {
                return new Content<Stream>(
                    stream,
                    info.Compression,
                    info.ContentType);
            }
            return null;
        }
        
        public virtual Content<Stream> Read (Uri source, Content<Stream> sink) {
            var result = default(Content<Stream>);
            if (IoMode.HasFlag(IO.IoMode.Read) && source.IsFile) {
                var filename = IoUtils.UriToFileName(source);
                var file = new FileStream(filename, FileMode.Open);

                result = Read(file, Detector.Use(file));

                // test if there is a provider with file's extension:
                if (result == null) {
                    var info = Detector.Find(Path.GetExtension(filename).Replace(".", ""));
                    if (info.Magics == null || info.Magics.Length == 0) {
                        result = Read(file, info);
                    }
                }

                if (result != null) {
                    if (result.Source == null)
                        result.Source = source.AbsoluteUri;
                    if (result.Description == null)
                        result.Description = source.Segments[source.Segments.Length - 1];
                } else {
                    file.Close();
                }
            }
            return result;
        }

        public virtual Content<Stream> ReadContent (Uri source) {
            return Read(source, null);
        }

        public virtual Uri Write (Content<Stream> content, Uri uri) {
            var result = base.Write(content.Data, uri);
            return result;
        }

        Content<Stream> IPipe<Uri, Content<Stream>>.Use (Uri source) {
            return ReadContent(source);
        }

        public Content<Stream> Use (Uri source, Content<Stream> sink) {
            return Read(source, sink);
        }

        public Uri Use (Content<Stream> source) {
            //TODO: use source.Source to make a Uri
            return Use(source, null);
        }

        public Uri Use (Content<Stream> source, Uri sink) {
            return Write(source, sink);
        }
    }

   
}