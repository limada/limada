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
using Limaki.View.Swf;
using Limaki.View.UI;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.Swf.Backends {

    public class DragDropMouseBackendHandler : DragDropBackendHandler, IDragDropMouseBackendHandler {

        public DragDropMouseBackendHandler(IVidgetBackend backend) : base(backend) {
        }

        public override void SetDragSource(DragDropAction dragAction, params TransferDataType[] types) {
            base.SetDragSource(dragAction, types);
            Backend.MouseUp += (s, e) => {
                                   var ev = Converter.Convert(e);
                                   MouseUp(ev);
                               };
            Backend.MouseMove += (s, e) => {
                                     var ev = Converter.Convert(e);
                                     MouseMove(ev);
                                 };
        }

        public virtual void MouseUp(MouseActionEventArgs e) {
            DragDropInfo.DragRect = Rectangle.Zero;
        }

        public virtual void MouseMove(MouseActionEventArgs e) {

            if (EnabledEvents.HasFlag(WidgetEvent.DragStarted))
                return;

            if (e.Button != MouseActionButtons.Left)
                return;

            if (DragDropInfo.DragRect.IsEmpty)
                SetupDragRect(e);

            if (DragDropInfo.DragRect.Contains(e.Location))
                return;

            var dragData = GetDragStartData();

            if (dragData != null)
                DragStart(dragData);

            DragDropInfo.DragRect = Rectangle.Zero;
        }

        public virtual DragDropAction DragDropActionFromKeyState(int keyState, DragDropAction allowedEffect) {

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
    }
}