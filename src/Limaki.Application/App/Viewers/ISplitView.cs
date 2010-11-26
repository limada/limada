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

using System.Windows.Forms;
using Limaki.Winform.Displays;
using System;
using Limaki.Common;

namespace Limaki.Winform.Viewers {
    public interface ISplitView {
        event EventHandler<EventArgs<ISplitView>> RefreshToolViewer;

        SplitViewMode ViewMode { get; set; }
        void ToggleView();
        
        bool CanGoBackOrForward ( bool forward );
        void GoBackOrForward(bool forward);
        void GoHome();

        void NewSheet();
        void NewNote();
        void SaveDocument();
    }
}