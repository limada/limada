/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Actions;
using Xwt;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.UI {
    /// <summary>
    /// delegates DragOver-Events to MouseOver
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DragDropCatcher<T> : ActionBase, IDropAction where T : IMouseAction {
        public DragDropCatcher(T baseAction, IVidgetBackend backend): base() {
            this.Priority = baseAction.Priority;
            this.backend = backend;
            this.baseAction = baseAction;
            this.Dragging = true;
        }

        private IVidgetBackend backend { get; set; }
        private IMouseAction baseAction { get; set; }

        public virtual bool Dragging { get; set; }

        public void DragOver(DragOverEventArgs e) { MouseActionBase.ForewardDragOver(baseAction, backend, e); }

        public void OnDrop(DragEventArgs e) {}

        public void DragLeave(EventArgs e) {}

    }
}