/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Limaki.Contents;
using Limaki.Model.Content;

namespace Limaki.Compression {
    public class CompressionWorker : ICompressionWorker {
        public virtual Stream Compress(Stream stream, CompressionType compression) {
            stream.Position = 0;
            MemoryStream result = null;
            if (IsCompressed(compression)) {
                int size = 0;
                result = new MemoryStream();
                if (compression == CompressionType.bZip2) {
                    size = BZip2Compress(stream, result);
                } else if (compression == CompressionType.Zip) {
                    size = ZipCompress(stream, result);
                }
                result.SetLength(size);

                return result;
            } else {
                return stream;
            }

        }
        public virtual Stream DeCompress(Stream stream, CompressionType compression) {
            stream.Position = 0;

            if (IsCompressed(compression)) {
                MemoryStream result = new MemoryStream();
                try {
                    if (compression == CompressionType.bZip2) {

                        BZip2Decompress(stream, result);

                    } else if (compression == CompressionType.Zip) {

                        ZipDecompress(stream, result);

                    }
                } catch (Exception e) {
                    throw e;
                }

                return result;
            } else {
                return stream;
            }
        }

        #region Compression


        public virtual int BZip2Compress(Stream uncompressed, Stream compressed) {
            uncompressed.Seek(0, SeekOrigin.Begin);
            BZip2OutputStream bZip2Stream =
                new BZip2OutputStream(compressed, 9);
            bZip2Stream.IsStreamOwner = false;
            int bufferSize = 1024 * 8;
            byte[] buffer = new byte[bufferSize];
            int readByte = 0; int position = 0;
            long length = uncompressed.Length;
            while (position < length - 1) {
                readByte = uncompressed.Read(buffer, 0, bufferSize);
                bZip2Stream.Write(buffer, 0, readByte);
                position += readByte;
            }

            bZip2Stream.Close();
            return bZip2Stream.BytesWritten;
        }

        public virtual void BZip2Decompress(Stream compressed, Stream decompressed) {
            compressed.Seek(0, SeekOrigin.Begin);
            BZip2InputStream bZip2Stream =
                new BZip2InputStream(compressed);
            bZip2Stream.IsStreamOwner = false;

            int bufferSize = 1024 * 8;
            byte[] buffer = new byte[bufferSize];
            int readByte = 0; int position = 0;
            while (true) {
                readByte = bZip2Stream.Read(buffer, 0, bufferSize);
                if (readByte <= 0) {
                    break;
                }
                decompressed.Write(buffer, 0, readByte);
                position += readByte;
            }


            decompressed.Flush();
            bZip2Stream.Close();
        }

        public virtual int ZipCompress(Stream uncompressed, Stream compressed) {
            uncompressed.Seek(0, SeekOrigin.Begin);
            DeflaterOutputStream
                zipStream =
                new DeflaterOutputStream(compressed);
            zipStream.IsStreamOwner = false; // compress wants to have compressed Stream open

            int bufferSize = 1024 * 8;
            byte[] buffer = new byte[bufferSize];
            int readByte = 0; int position = 0;
            long length = uncompressed.Length;
            while (position < length - 1) {
                readByte = uncompressed.Read(buffer, 0, bufferSize);
                zipStream.Write(buffer, 0, readByte);
                position += readByte;
            }


            zipStream.Finish();
            zipStream.Flush();
            int result = (int)compressed.Length;
            zipStream.Close();
            return result;
        }

        public virtual void ZipDecompress(Stream compressed, Stream decompressed) {
            compressed.Seek(0, SeekOrigin.Begin);

            InflaterInputStream zipStream =
                new InflaterInputStream(compressed);

            zipStream.IsStreamOwner = false;
            int b = zipStream.ReadByte();
            while (b != -1) {
                decompressed.WriteByte((byte)b);
                b = zipStream.ReadByte();
            }

            decompressed.Flush();
            zipStream.Close();
        }

        #endregion


        bool ICompressionWorker.Compressable(CompressionType compression) {
            return IsCompressed (compression);
        }

        public static bool IsCompressed(CompressionType compression) {
            return !((compression == CompressionType.None) ||
                     (compression == CompressionType.neverCompress));
        }
    }
}