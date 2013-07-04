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

using Limaki.View.DragDrop;
using Limaki.View.Swf.Backends;
using System;
using System.Diagnostics;
using Xwt;
using SWF = System.Windows.Forms;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.Swf.Backends {
    /// <summary>
    /// example how to implement DragDrop on Windows.Forms.Controls 
    /// </summary>
    public class DragDropVidgetBackend:CanvasVidgetBackend {

        public DragDropVidgetBackend () {
            // remove this, just for debug:
            BackendHandler.SetDragSource(DragDropAction.All, TransferDataType.Text);
            BackendHandler.SetDragTarget(DragDropAction.All, TransferDataType.Text);
        }

        // remove this, just for debug:
        protected virtual TransferDataSource DragDataSource () {
            var result = new TransferDataSource();
            result.AddValue<string>("hello drag");
            return result;
        }

        protected virtual void Dropped (DragEventArgs args) {
            Trace.WriteLine(args.Data.GetValue(TransferDataType.Text));
        }


        private DragDropMouseBackendHandler _backendHandler = null;
        public IDragDropMouseBackendHandler BackendHandler {
            get {
                return _backendHandler ?? (_backendHandler =
                                           new DragDropMouseBackendHandler(this) {
                                               DragDataSource = () => DragDataSource(),
                                               Dropped = a => Dropped(a)
                                           });
            }
        }

        //this is called by Control.DoDragDrop
        protected override void OnGiveFeedback (SWF.GiveFeedbackEventArgs e) {
            base.OnGiveFeedback(e);
        }

        //this is called by Control.DoDragDrop
        protected override void OnQueryContinueDrag (SWF.QueryContinueDragEventArgs e) {
            base.OnQueryContinueDrag(e);
        }

        protected override void OnDragOver (SWF.DragEventArgs e) {
            var ev = e.ToXwtDragOver();
            ev.AllowedAction = BackendHandler.DragDropActionFromKeyState(e.KeyState, ev.Action);
            BackendHandler.DragOver(ev);
            e.Effect = ev.AllowedAction.ToSwf();
            base.OnDragOver(e);

        }

        protected override void OnDragDrop (SWF.DragEventArgs e) {
            var ev = e.ToXwt();
            BackendHandler.OnDrop(ev);
            base.OnDragDrop(e);

        }

        protected override void OnDragLeave (EventArgs e) {
            BackendHandler.DragLeave(e);
            base.OnDragLeave(e);
        }


    }
}