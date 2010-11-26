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
 */

using System;
using System.Windows.Forms;

using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.UI;

namespace Limaki.Winform.Widgets {
    public class EdgeWidgetChanger : EdgeWidgetChangerBase, IDragDropAction {
        public EdgeWidgetChanger(Func<Scene> sceneHandler, IControl control, ICamera camera):
            base(sceneHandler,control,camera){}
        
        #region IDragDropAction Member
        public void OnGiveFeedback(GiveFeedbackEventArgs e) { }

        public void OnQueryContinueDrag(QueryContinueDragEventArgs e) {
            if (Resolved) {
                e.Action = DragAction.Cancel;
                Dragging = false;
            }
        }

        public void OnDragOver(DragEventArgs e) {
            //Dragging = !Resolved; 
        }

        public void OnDragDrop(DragEventArgs e) { }
        public void OnDragLeave(EventArgs e) { }

        #endregion
    }
}
