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
using System.Linq;
using System.Text;

namespace Limaki.Common.Text {

    public static class TextHelper {

        public static bool Contains (this StringBuilder haystack, string needle) {
            return haystack.IndexOf(needle) != -1;
        }

        /// <summary>
        /// Simple implementation of the Knuth–Morris–Pratt algorithm that only cares about ordinal matches 
        /// (no case-folding, no culture-related collation, just a plain codepoint to codepoint match). 
        /// It has some initial Θ(m) overhead where m is the length of the word sought, and then finds in Θ(n) 
        /// where n is the distance to the word sought, or the length of the whole string-builder if it isn't there. 
        /// This beats the simple char-by-char compare which is Θ((n-m+1) m) 
        /// (Where O() notation describes upper-bounds, Θ() describes both upper and lower bounds).
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/12261344/fastest-search-method-in-stringbuilder</remarks>
        /// <param name="haystack"></param>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static int IndexOf (this StringBuilder haystack, string needle) {
            return IndexOf(haystack, needle, 0);
        }

        public static int IndexOf (this StringBuilder haystack, string needle, int startIndex) {
            if (haystack == null || needle == null)
                throw new ArgumentNullException();
            if (needle.Length == 0)
                return 0; //empty strings are everywhere!
            if (needle.Length == 1) { //can't beat just spinning through for it
    
                char c = needle[0];
                for (int idx = startIndex; idx != haystack.Length; ++idx)
                    if (haystack[idx] == c)
                        return idx;
                return -1;
            }
            int m = startIndex;
            int i = 0;
            int[] T = KMPTable(needle);
            while (m < haystack.Length - 1) {
                if (needle[i] == haystack[m + i]) {
                    if (i == needle.Length - 1)
                        return m == needle.Length ? -1 : m; //match -1 = failure to find conventional in .NET
                    ++i;
                } else {
                    m = m + i - T[i];
                    i = T[i] > -1 ? T[i] : 0;
                }
            }
            return -1;
        }

        private static int[] KMPTable (string sought) {
            int[] table = new int[sought.Length];
            int pos = 2;
            int cnd = 0;
            table[0] = -1;
            table[1] = 0;
            while (pos < table.Length)
                if (sought[pos - 1] == sought[cnd])
                    table[pos++] = ++cnd;
                else if (cnd > 0)
                    cnd = table[cnd];
                else
                    table[pos++] = 0;
            return table;
        }

        public static string Between (this StringBuilder text, string start, string end, int startIndex) {
            int posStart = text.IndexOf(start, startIndex);
            if (posStart == -1) return null;
            posStart += start.Length;
            int posEnd = text.IndexOf(end, posStart);
            if (posEnd == -1) return null;
            return text.ToString(posStart, posEnd - posStart);
        }

        public static string Between (this string text, string start, string end, int startIndex) {
            int posStart = text.IndexOf(start, startIndex);
            if (posStart == -1) return null;
            posStart += start.Length;
            int posEnd = text.IndexOf(end, posStart);
            if (posEnd == -1) return null;
            return text.Substring(posStart, posEnd - posStart);
        }


        public static bool IsUnicode(byte[] buffer) {
            var isUnicode = 0;
            for (int i = 1; i < buffer.Length; i += 2) {
                if (buffer[i] == 0)
                    isUnicode ++;
            }
            return isUnicode > buffer.Length/10;

        }
    }
}
