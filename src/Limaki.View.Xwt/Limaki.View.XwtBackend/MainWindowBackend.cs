/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Diagnostics;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class MainWindowBackend : VindowBackend {

       protected StatusIcon StatusIcon{get;set;}

        public MainWindowBackend () {
            try {
                StatusIcon = Application.CreateStatusIcon();
                StatusIcon.Menu = new Menu();
                StatusIcon.Menu.Items.Add(new MenuItem("Test"));

            } catch {
                Trace.WriteLine("Status icon could not be shown");
            }
        }
    }
}