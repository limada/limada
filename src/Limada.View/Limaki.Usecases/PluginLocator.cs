/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Diagnostics;
using System.IO;

namespace Limaki.Usecases {

    public class PluginLocator  {

        public string PluginDir (string basedir, string pluginDir) {
            foreach (var dir in new string[] { @"Plugins" + Path.DirectorySeparatorChar, $"..{Path.DirectorySeparatorChar}3rdParty{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"}) {
                var s = dir;
                for (int i = 0; i <= 10; i++) {
                    var path = basedir + s + pluginDir;
                    if (Directory.Exists (path))
                        return path;
                    s = @".." + Path.DirectorySeparatorChar + s;
                }
            }
            return null;
        }

        public string PluginDir (string pluginDir) {
            return PluginDir (AppDomain.CurrentDomain.BaseDirectory, pluginDir) ?? PluginDir ( Path.GetDirectoryName (Process.GetCurrentProcess ().MainModule.FileName), pluginDir);
        }
    }
}