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
 * http://limada.sourceforge.net
 */


using Limaki.UseCases.Viewers;
using System.Windows.Forms;
using System.Threading;
using System;
using Limaki.Presenter;

namespace Limaki.Winform.Controls {
    public class WebBrowser:System.Windows.Forms.WebBrowser,IWebBrowser, INavigateTarget {
        public void MakeReady() {
            if (base.Document == null) {
                Action navigate = () => base.Navigate("about:blank");
                if (base.InvokeRequired)
                    base.Invoke(navigate);
                else
                    navigate();

                if (!Commons.Mono)
                    for (int i = 0; i < 200 && base.IsBusy; i++) {
                        Application.DoEvents();
                        Thread.Sleep(5);
                    }
            }
        }
    }
}
