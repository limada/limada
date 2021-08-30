/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Limaki.Common {

    public class IoUtils {

        public static string FindSubDirInRootPath (string subdirToFind) {
            return FindSubDirInRootPath (Directory.GetCurrentDirectory (), subdirToFind);
        }

        public static string FindSubDirInRootPath (string directory, string subdirToFind) {

            var parent = Directory.GetParent (directory);


            while (parent != null) {

                foreach (var dir in Directory.GetDirectories (directory)) {
                    var subdir = dir.Split (Path.DirectorySeparatorChar).LastOrDefault ();
                    if (subdir == subdirToFind) {
                        return dir;
                    }

                }
                directory = parent.FullName;
                parent = Directory.GetParent (directory);

            }
            return null;
        }

        public static string UriToFileName (Uri uri) {
            if (uri.IsFile) {
                return Uri.UnescapeDataString (uri.AbsolutePath);
            }
            return null;
        }

        [TODO ("handle relative filenames")]
        public static Uri UriFromFileName (string fileName) {
            var uriKind = UriKind.Absolute;

            return new Uri (fileName, uriKind);
        }

        public static string NiceFileName (string fileName) {

            if (string.IsNullOrEmpty (fileName))
                return fileName;
            var b = new StringBuilder (Path.GetFileName (fileName));
            var path = Path.GetDirectoryName (fileName);
            foreach (var s in Path.GetInvalidFileNameChars ())
                b.Replace (s, '_');
            var sep = Path.DirectorySeparatorChar.ToString ();
            if (!string.IsNullOrEmpty (path) && (!path.EndsWith (sep)))
                b.Insert (0, sep);
            return path + b.ToString ();
        }
    }
}
