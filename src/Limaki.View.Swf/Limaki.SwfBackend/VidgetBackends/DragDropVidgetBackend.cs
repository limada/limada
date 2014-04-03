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

using Limaki.SwfBackend.DragDrop;
using Limaki.View.DragDrop;
using System;
using System.Diagnostics;
using Xwt;
using Xwt.SwfBackend;

using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;

namespace Limaki.SwfBackend.VidgetBackends {
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
        protected override void OnGiveFeedback (System.Windows.Forms.GiveFeedbackEventArgs e) {
            base.OnGiveFeedback(e);
        }

        //this is called by Control.DoDragDrop
        protected override void OnQueryContinueDrag (System.Windows.Forms.QueryContinueDragEventArgs e) {
            base.OnQueryContinueDrag(e);
        }

        protected override void OnDragOver (System.Windows.Forms.DragEventArgs e) {
            var ev = e.ToXwtDragOver(this);
            ev.AllowedAction = BackendHandler.DragDropActionFromKeyState(e.KeyState, ev.Action);
            BackendHandler.DragOver(ev);
            e.Effect = ev.AllowedAction.ToSwf();
            base.OnDragOver(e);

        }

        protected override void OnDragDrop (System.Windows.Forms.DragEventArgs e) {
            var ev = e.ToXwt(this);
            BackendHandler.OnDrop(ev);
            base.OnDragDrop(e);

        }

        protected override void OnDragLeave (EventArgs e) {
            BackendHandler.DragLeave(e);
            base.OnDragLeave(e);
        }


    }
}