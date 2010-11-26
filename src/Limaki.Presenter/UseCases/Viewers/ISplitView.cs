/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limaki.Common;

namespace Limaki.UseCases.Viewers {
    public interface ISplitView {

        SplitViewMode ViewMode { get; set; }
        void ToggleView();
        
        bool CanGoBackOrForward ( bool forward );
        void GoBackOrForward(bool forward);
        void GoHome();

        void NewSheet();
        void NewNote();
        void SaveDocument();

        event EventHandler ViewChanged;
    }

    public enum SplitViewMode {
        /// <summary>
        /// shows two WidgetDisplays
        /// </summary>
        GraphGraph,
        /// <summary>
        /// shows one WidgetDisplay and one StreamViewer
        /// </summary>
        GraphStream
    }
}