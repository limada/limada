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
using System.Diagnostics;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.View.Swf.Visualizers;
using Xwt;
using SWF = System.Windows.Forms;
using Limaki.View.DragDrop;

namespace Limaki.View.Swf.Backends {

    /// <summary>
    /// this is for prototyping; should be moved to VisualsDisplayBackend if done
    /// </summary>
    public class VisualsDisplayDragDopBackend : VisualsDisplayBackend {

        public VisualsDisplayDragDopBackend () {
            // remove this, just for debug:
            Registry.Factory.Add<IDragDropBackendHandler>(args => new DragDropBackendHandler(args[0] as IVidgetBackend));
        
        }

        //this is called by Control.DoDragDrop
        protected override void OnGiveFeedback (GiveFeedbackEventArgs e) {


            base.OnGiveFeedback(e);

        }

        //this is called by Control.DoDragDrop
        protected override void OnQueryContinueDrag (QueryContinueDragEventArgs e) {


            base.OnQueryContinueDrag(e);

        }

        DragDropAction DragDropActionFromKeyState (int keyState, DragDropAction allowedEffect) {

            // Set the effect based upon the KeyState.
            if ((keyState & (8 + 32)) == (8 + 32) &&
                (allowedEffect & DragDropAction.Link) == DragDropAction.Link) {
                // KeyState 8 + 32 = CTL + ALT

                // Link drag and drop effect.
                return DragDropAction.Link;

            } else if ((keyState & 32) == 32 &&
                       (allowedEffect & DragDropAction.Link) == DragDropAction.Link) {

                // ALT KeyState for link.
                return DragDropAction.Link;

            } else if ((keyState & 4) == 4 &&
                       (allowedEffect & DragDropAction.Move) == DragDropAction.Move) {

                // SHIFT KeyState for move.
                return DragDropAction.Move;

            } else if ((keyState & 8) == 8 &&
                       (allowedEffect & DragDropAction.Copy) == DragDropAction.Copy) {

                // CTL KeyState for copy.
                return DragDropAction.Copy;

            } else if ((allowedEffect & DragDropAction.Move) == DragDropAction.Move) {

                // By default, the drop action should be copy, if allowed.
                return DragDropAction.Copy;

            }
            return DragDropAction.Copy;
        }

        protected override void OnDragOver (SWF.DragEventArgs e) {
            var dropHandler = Display.EventControler as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                var ev = e.ToXwtDragOver();
                ev.AllowedAction = DragDropActionFromKeyState(e.KeyState, ev.Action);
                dropHandler.DragOver(ev);
                e.Effect = ev.AllowedAction.ToSwf();
            }
            base.OnDragOver(e);

        }

        protected override void OnDragDrop (SWF.DragEventArgs e) {
            var dropHandler = Display.EventControler as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.OnDrop(e.ToXwt());
            }
            base.OnDragDrop(e);

        }

        protected override void OnDragLeave (EventArgs e) {
            var dropHandler = Display.EventControler as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.DragLeave(e);
            }
            base.OnDragLeave(e);
        }


    }
}