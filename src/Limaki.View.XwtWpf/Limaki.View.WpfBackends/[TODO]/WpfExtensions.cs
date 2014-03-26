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

using System;
using System.Windows;
using System.Windows.Threading;

namespace Limaki.View.WpfBackends {

    public static class WpfExtensions {

        public static void DoEvents() {
            Application.Current.Dispatcher.Invoke (DispatcherPriority.Background,
                new Action (delegate { }));
        }
    }
}