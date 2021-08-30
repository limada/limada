/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.DragDrop {

    /// <summary>
    /// handles dragdrop 
    /// <remarks>see: Xwt.IWidgetBackend / Xwt.Widget</remarks>
    /// </summary>
    public interface IDragDropBackendHandler : IDropHandler {

        void SetDragSource (DragDropAction dragAction, params TransferDataType[] types);
        Func<TransferDataSource> DragDataSource { get; set; }

        /// <summary>
        /// drag is finished (with success?)
        /// </summary>
        Action<DragFinishedEventArgs> DragFinished { get; set; }

        TransferDataType[] SourceTypes { get; }
       
        void DragStart (DragStartData data);

        Action<DragEventArgs> Dropped { get; set; }
        void SetDragTarget (DragDropAction dragAction, params TransferDataType[] types);
        TransferDataType[] TargetTypes { get; }

        void DragOverCheck (DragOverCheckEventArgs args);
        void DropCheck(DragCheckEventArgs args);
        
        WidgetEvent EnabledEvents { get; set; }

    }

    public interface IDragDropMouseBackendHandler : IDragDropBackendHandler {
        void MouseUp (MouseActionEventArgs e);
        void MouseMove (MouseActionEventArgs e);
        DragDropAction DragDropActionFromKeyState (int keyState, DragDropAction allowedEffect);
    }  
}