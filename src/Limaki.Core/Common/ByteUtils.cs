/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.IO;
using System.Text;

namespace Limaki.Common {

    public static class ByteUtils {

        public static byte[] BytesOfArray (this Array data) {
            var len = Buffer.ByteLength (data);
            var result = new byte[len];
            for (int i = 0; i < len; i++) {
                result[i] = Buffer.GetByte (data, i);
            }
            return result;
        }

        public static Stream AsUnicodeStream (this string source) {
            var buffer = Encoding.Unicode.GetBytes (source);
            return new MemoryStream (buffer, 0, buffer.Length, true);
        }

        public static Stream AsAsciiStream (this string source) {
            var buffer = Encoding.Convert (Encoding.Unicode, Encoding.ASCII,
                                           Encoding.Unicode.GetBytes (source));
            return new MemoryStream (buffer, 0, buffer.Length, true);
        }

        public static Stream AsAnsiStream (this string source) {
            var buffer = Encoding.Convert (Encoding.Unicode, Encoding.GetEncoding (1252),
                                           Encoding.Unicode.GetBytes (source));
            return new MemoryStream (buffer, 0, buffer.Length, true);
        }

        public static String AsUnicodeSting (this byte[] source) => Encoding.Unicode.GetString (source);

        public static byte[] GetBuffer (this Stream stream, int buflen = -1) {
            if (stream == null)
                return new byte[0];

            if (buflen >= 0)
                buflen = Math.Min (buflen, (int)stream.Length);
            else
                buflen = (int)stream.Length;
            if (stream is MemoryStream memory && buflen == (int)stream.Length) {
                return memory.ToArray ();
            }
            var buffer = new byte[buflen];
            var oldPos = stream.Position;
            stream.Read (buffer, 0, buflen);
            stream.Position = oldPos;
            return buffer;
        }

        public static bool BuffersAreEqual (byte[] a, byte[] b) {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }
}
