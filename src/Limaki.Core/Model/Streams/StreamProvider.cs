using System;
using System.Collections.Generic;
using System.IO;

namespace Limaki.Model.Streams {
    public abstract class StreamProvider : IStreamProvider {
        public abstract IEnumerable<StreamTypeInfo> SupportedStreamTypes { get; }

        public abstract bool Saveable { get; }


        public virtual StreamInfo<Stream> Open(Stream stream) {
            var info = SupportingInfo(stream);
            if (info != null) {

                return new StreamInfo<Stream>(
                    stream,
                    info.Compression,
                    info.StreamType);
            }
            return null;
        }

        public virtual StreamTypeInfo SupportingInfo(Stream stream) {
            foreach (var info in SupportedStreamTypes) {
                if (info.Magics != null) {
                    foreach (var magic in info.Magics) {
                        if (HasMagic(stream, magic.Bytes, magic.Offset))
                            return info;
                    }
                }
            }
            return null;
        }

        public virtual bool Supports(Stream stream) {
            return SupportingInfo(stream) != null;
        }

        public virtual void Save(StreamInfo<Stream> data, Uri uri) {
            if (uri.IsFile) {
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


        public virtual StreamInfo<Stream> Open(Uri uri) {
            var result = default(StreamInfo<Stream>);
            if (uri.IsFile) {
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

        public virtual bool Supports(string extension) {
            foreach (var type in SupportedStreamTypes) {
                if (type.Extension == extension) {
                    return true;
                }
            }
            return false;
        }


        public bool Supports(long streamType) {
            foreach (var type in SupportedStreamTypes) {
                if (type.StreamType == streamType) {
                    return true;
                }
            }
            return false;
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