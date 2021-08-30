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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Limaki.Common.Text.HTML {

    public class HtmlHelper {

        private static Regex tags =
            new Regex(@"<A[^>]*?HREF\s*=\s*[""']?" + "|" +
                      @"<IMG[^>]*?SRC\s*=\s*[""']?" + "|" +
                      @"[ '""].*?>",
                      RegexOptions.IgnoreCase
#if!SILVERLIGHT
                      | RegexOptions.Compiled
#endif
                );

        private static Regex links =
            new Regex(
                @"<A[^>]*?HREF\s*=\s*[""']?([^'"" >]+?)[ '""].*?>"
                + "|" + @"<IMG[^>]*?SRC\s*=\s*[""']?([^'"" >]+?)[ '""].*?>"
                , RegexOptions.IgnoreCase
#if!SILVERLIGHT
                  | RegexOptions.Compiled
#endif
                );

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

        public const string HtmUtf8Begin = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">"
                                      + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />"
                                      + "<html xmlns=\"http://www.w3.org/1999/xhtml\" >";
    }
}