/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Limaki.Common.Text.HTML {
    public class HTMLHelper {
        static Regex tags =
            new Regex(@"<A[^>]*?HREF\s*=\s*[""']?" + "|" +
                      @"<IMG[^>]*?SRC\s*=\s*[""']?" + "|" +
                      @"[ '""].*?>",
                      RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex links =
            new Regex(
                @"<A[^>]*?HREF\s*=\s*[""']?([^'"" >]+?)[ '""].*?>"
                + "|" + @"<IMG[^>]*?SRC\s*=\s*[""']?([^'"" >]+?)[ '""].*?>"


                , RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static IEnumerable<string> Links(string content) {

            //links = @"(?:(<A[^>]*?HREF\s*=\s*[""']?))(?:(([^'"" >]+?)[ '""].*?>))";
            //links = @"<a[^>]*?href\s*=\s*(?:""(?<1>[^""]*)""|(?<1>\S+))";

            int start = Environment.TickCount;
            int linkCount = 0;
            foreach (Match r in links.Matches(content)) {
                var result = r.Value;
                result = tags.Replace(result, "");
                yield return result;
                linkCount++;
            }

#if DEBUG
            var status = string.Format(
                "Extract links of:\tlen:\t{0}\tduration\t{1}\tlinks\t{2}",
                new object[] { content.Length, (Environment.TickCount - start), linkCount });

            Debug.WriteLine(status);
#endif
        }
    }
}