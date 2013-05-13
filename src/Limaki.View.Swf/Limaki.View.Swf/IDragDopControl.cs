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
 * http://www.limada.org
 * 
 */


using System;
using System.Windows.Forms;
using Limaki.Actions;


namespace Limaki.View.Swf {
    public interface IDragDopControl:IVidgetBackend {
        bool AllowDrop { get; set; }
        DragDropEffects DoDragDrop(Object data, DragDropEffects allowedEffects);
    }

    public interface IDragDropAction:IAction {
        bool Dragging { get;set; }
        void OnGiveFeedback( GiveFeedbackEventArgs e );
        void OnQueryContinueDrag( QueryContinueDragEventArgs e );
        void OnDragOver( DragEventArgs e );
        void OnDragDrop( DragEventArgs e );
        void OnDragLeave( EventArgs e );
        void Copy();
        void Paste();
    }
}
