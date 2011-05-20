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
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.Winform {
    /// <summary>
    /// </summary>
    public class DragDropCatcher<T> : ActionBase,IDragDropAction 
    where T:IMouseAction {
        public DragDropCatcher():base() {}

        public DragDropCatcher(T baseAction, IControl control): base() {
            this.Priority = baseAction.Priority;
            this.control = control;
            this.baseAction = baseAction;
        }
        IControl control = null;
        T baseAction = default(T);

        #region IDragDropAction Member
        bool _dragging = true;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }

        public void OnGiveFeedback(GiveFeedbackEventArgs e) { }

        public void OnQueryContinueDrag(QueryContinueDragEventArgs e) { }

        public void OnDragOver(DragEventArgs e) {
            if (baseAction.Enabled) {
                PointI pt =
                    control.PointToClient (new PointI (e.X, e.Y))
                    ;

                MouseActionEventArgs em =
                    new MouseActionEventArgs (MouseActionButtons.None,
                                              Converter.ConvertModifiers (e.KeyState), 0, pt.X, pt.Y, 0);

                baseAction.OnMouseMove (em);
            }
        }

        public void OnDragDrop(DragEventArgs e) { }
        public void OnDragLeave(EventArgs e) { }
        public void Copy(){}
        public void Paste() { }
        #endregion
    }
}