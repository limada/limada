/*
 * Limaki 
 * Version 0.08
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
using System.Windows.Forms;
using ApplicationContextRecourceLoader=
    Limada.App.ApplicationContextRecourceLoader;
using Registry=Limaki.Common.Registry;
using Limaki.Common;

namespace Limaki.App {
    static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main() {
            var resourceLoader = new ApplicationContextRecourceLoader ();
            Registry.ConcreteContext = resourceLoader.CreateContext();
            resourceLoader.ApplyResources (Registry.ConcreteContext);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}