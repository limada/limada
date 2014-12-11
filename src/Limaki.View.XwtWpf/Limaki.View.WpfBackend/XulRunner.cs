/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using Gecko;
using Limaki.Common;
using Limaki.Usecases;

namespace Limaki.View.WpfBackend {

    public class XulRunner : PluginLocator {


        string _ProfileDirectory;

        protected string ProfileDirectory {
            get {
                if (_ProfileDirectory == null) {
                    _ProfileDirectory = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData), Path.Combine ("Limada", "GeckoDefaultProfile"));

                    if (!Directory.Exists (_ProfileDirectory)) {
                        Directory.CreateDirectory (_ProfileDirectory);
                    }
                }
                return _ProfileDirectory;
            }
        }

        protected static bool Running = false;

        public bool Initialize () {
            if (!Running) {
                string xulDir = PluginDir ("xulrunner29.0-" + (OS.IsWin64Process ? "64" : "32"));
                if (xulDir == null)
                    throw new ArgumentException ("xulrunner is missing");

                Xpcom.ProfileDirectory = this.ProfileDirectory;
                Xpcom.Initialize (xulDir);
                // this makes troubles:
                //Application.ApplicationExit += (sender, e) => {
                //    Xpcom.Shutdown();
                //};
                Running = true;
            }
            return Running;
        }
    }
}