using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Limaki.Model.Streams {
    public abstract class ContentProvider : IContentProvider {
        public abstract IEnumerable<ContentInfo> SupportedContents { get; }

        public abstract bool Saveable { get; }
        public abstract bool Readable { get; }

        public virtual Content<Stream> Open(Stream stream) {
            var info = Info (stream);
            if (info != null) {
                return new Content<Stream>(
                    stream,
                    info.Compression,
                    info.ContentType);
            }
            return null;
        }

        public virtual ContentInfo Info(Stream stream) {
            return SupportedContents.Where(info => info.Magics != null)
                .Where (info => info.Magics.Any (magic => HasMagic (stream, magic.Bytes, magic.Offset)))
                .FirstOrDefault();
        }

        public virtual ContentInfo Info (string extension) {
            extension = extension.ToLower ().TrimStart ('.');
            return SupportedContents.Where (type => type.Extension == extension).FirstOrDefault ();
        }

        public virtual ContentInfo Info ( long streamType) {
            return SupportedContents.Where (type => type.ContentType == streamType).FirstOrDefault();
        }

        public virtual bool Supports (string something) {
            return Info (something) != null;
        }

        public virtual bool Supports (Stream something) {
            return Info (something) != null;
        }

        public virtual bool Supports (long something) {
            return Info (something) != null;
        }


        public virtual void Save(Content<Stream> data, Uri uri) {
            if (Saveable && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Create);
                var target = new BufferedStream(file);
                var bufferSize = 1024 * 1024;
                var buffer = new byte[bufferSize];
                var source = data.Data;
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
        }


        public virtual Content<Stream> Open(Uri uri) {
            var result = default(Content<Stream>);
            if (Readable && uri.IsFile) {
                var filename = IOUtils.UriToFileName(uri);
                var file = new FileStream(filename, FileMode.Open);

                result = Open(file);

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

       

        protected virtual bool BuffersAreEqual(byte[] a, byte[] b) {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        protected virtual bool HasMagic(Stream stream, byte[] magic, int offset) {
            if (stream.Length <= magic.Length)
                return false;
            var buffer = new byte[magic.Length];
            var pos = stream.Position;
            stream.Position = offset;
            stream.Read(buffer, 0, buffer.Length);

            var result = BuffersAreEqual(magic, buffer);
            stream.Position = pos;
            return result;
        }
    }
}