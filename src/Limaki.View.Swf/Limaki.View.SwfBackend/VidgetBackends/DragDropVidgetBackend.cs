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
using System;
using System.Diagnostics;
using Limaki.View.SwfBackend.DragDrop;
using Xwt;
using Xwt.SwfBackend;

using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;

namespace Limaki.View.SwfBackend.VidgetBackends {

    /// <summary>
    /// example how to implement DragDrop on Windows.Forms.Controls 
    /// </summary>
    public class DragDropVidgetBackend:CanvasVidgetBackend {


        protected override void Compose () {
            base.Compose ();

            // remove this, just for debug:
            BackendHandler.SetDragSource (DragDropAction.All, TransferDataType.Text);
            BackendHandler.SetDragTarget (DragDropAction.All, TransferDataType.Text);
            
            Control.DragOver += (s, e) => {
                var ev = e.ToXwtDragOver (Control);
                ev.AllowedAction = BackendHandler.DragDropActionFromKeyState (e.KeyState, ev.Action);
                BackendHandler.DragOver (ev);
                e.Effect = ev.AllowedAction.ToSwf ();
            };

            Control.DragDrop += (s, e) => {
                var ev = e.ToXwt (Control);
                BackendHandler.OnDrop (ev);
            };

            Control.DragLeave += (s, e) => {
                BackendHandler.DragLeave (e);
            };

            Control.GiveFeedback += (s, e) => {

            };
            Control.QueryContinueDrag += (s, e) => {

            };
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

    }
}