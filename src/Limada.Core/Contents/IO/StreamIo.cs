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
using Limaki.Common;
using Limaki.Contents;

namespace Limaki.Contents.IO {

    /// <summary>
    /// a pipe that returns the ContentInfo of a Stream
    /// and reads and writes streams based on a Uri
    /// uses IoMode
    /// </summary>
    public class StreamIo : ContentIo<Stream>, IPipe<Uri, Stream>, IPipe<Stream, Uri> {

        protected StreamIo(ContentDetector detector): base(detector) {}

        public override ContentInfo Use(Stream source) {
            return Detector.Use(source);
        }

        public override ContentInfo Use(Stream source, ContentInfo sink) {
            return Detector.Use(source, sink);
        }

        public override bool Supports(Stream source) {
            return Detector.Supports(source);
        }

        public virtual Stream Read (Uri uri) {
            var result = default(Stream);
            if (IoMode.HasFlag(IO.IoMode.Read) && uri.IsFile) {
                var filename = IoUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Open);
            }
            return result;
        }

        public virtual Uri Write (Stream source, Uri sink) {

            if (sink == null)
                throw new ArgumentException("Uri must not be null");

            if (IoMode.HasFlag(IO.IoMode.Write) && sink.IsFile) {
                var filename = IoUtils.UriToFileName(sink);
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

        public virtual Uri Use (Stream source, Uri sink) {
            return Write(source, sink);
        }

        Uri IPipe<Stream, Uri>.Use (Stream source) {
            return Write(source, null);
        }

        public virtual Stream Use (Uri source) {
            return Read(source);
        }

        public virtual Stream Use (Uri source, Stream sink) {
            throw new NotImplementedException("reading into existing stream");
            return sink;
        }

    }

}