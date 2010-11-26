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


using System;

using Limaki.Actions;


namespace Limaki.Presenter.UI.DragDrop {
    public interface IDragDopControl:IControl {
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
    }
}
