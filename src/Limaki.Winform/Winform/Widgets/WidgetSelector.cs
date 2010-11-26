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
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.UI;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Selects a widget on mouse down or mouse move
    /// Sets scene.focused and scene.selected or scene.hovered
    /// </summary>
    public class WidgetSelector : WidgetSelectorBase, IDragDropAction {
        public WidgetSelector():base() {}
        
        public WidgetSelector(Func<Scene> sceneHandler, IWinControl control, ICamera camera):base(sceneHandler,camera) {
            this.control = control;
        }
        IWinControl control = null;

        #region IDragDropAction Member
        bool _dragging = true;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }

        public void OnGiveFeedback(GiveFeedbackEventArgs e) { }

        public void OnQueryContinueDrag(QueryContinueDragEventArgs e) { }

        public void OnDragOver(DragEventArgs e) {
            PointI pt = GDIConverter.Convert(
                control.PointToClient(new System.Drawing.Point(e.X, e.Y))
                );

            MouseActionEventArgs em =
                 new MouseActionEventArgs(MouseActionButtons.None,
                     Converter.ConvertModifiers(e.KeyState), 0, pt.X, pt.Y, 0);

            this.OnMouseMove(em);
        }

        public void OnDragDrop(DragEventArgs e) { }
        public void OnDragLeave(EventArgs e) { }

        #endregion
    }
}