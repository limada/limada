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
using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing.UI;

namespace Limaki.Winform {
    /// <summary>
    /// EventControler decouples Windows.Forms.Control-Events
    /// it provides events over ActionLists
    /// ActionLists are sorted with Action.Priority
    /// </summary>
    public class EventControler : EventControlerBase, IMouseAction, IKeyAction, IDragDropAction {


        
        public List<IDragDropAction> DragDropActions = new List<IDragDropAction>();
        
        public override void Add(IAction action)  {
            base.Add (action);


            if (action is IDragDropAction) {
                DragDropActions.Add((IDragDropAction)action);
                DragDropActions.Sort(ActionsSort);
            }
            
        }
        
        public override void Remove(IAction action) {
            base.Remove (action);

            
            if (action is IDragDropAction) {
                DragDropActions.Remove((IDragDropAction)action);
            }
            
        }




        #region IDragDropAction Member
        bool _dragging = false;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }

        public void OnGiveFeedback( GiveFeedbackEventArgs gfbevent ) {
            Resolved = false;
            foreach (IDragDropAction dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnGiveFeedback(gfbevent);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            CommandsExecute();
        }

        public void OnQueryContinueDrag( QueryContinueDragEventArgs qcdevent ) {
            Resolved = false;
            foreach (IDragDropAction dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnQueryContinueDrag(qcdevent);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            CommandsExecute();
        }

        public void OnDragOver( DragEventArgs drgevent ) {
            Resolved = false;
            foreach (IDragDropAction dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnDragOver(drgevent);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive){//} || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            CommandsExecute();
        }

        public void OnDragDrop( DragEventArgs drgevent ) {
            Resolved = false;
            foreach (IDragDropAction dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnDragDrop(drgevent);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive) {//} || ! dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            CommandsExecute();
        }

        public void OnDragLeave( EventArgs e ) {
            Resolved = false;
            foreach (IDragDropAction dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnDragLeave(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            CommandsExecute();
        }

        #endregion
    }
}
