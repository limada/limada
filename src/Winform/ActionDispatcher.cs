/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Winform {
    public class ActionDispatcher : ActionBase, IDisposable, IMouseAction, IPaintAction, IKeyAction, IDragDropAction {
        public List<IMouseAction> MouseActions = new List<IMouseAction>();
        public List<IKeyAction> KeyActions = new List<IKeyAction>();
        public List<IPaintAction> PaintActions = new List<IPaintAction>();
        public List<ICommandAction> CommandActions = new List<ICommandAction>();
        public List<IDragDropAction> DragDropActions = new List<IDragDropAction>();
        public Dictionary<Type, IAction> Actions = new Dictionary<Type, IAction> ();

        int ActionsSort(IAction x, IAction y) {
           return x.Priority.CompareTo(y.Priority);
        }

        public void Add(IAction action)  {
            if (action is IMouseAction) {
                MouseActions.Add((IMouseAction)action);
                MouseActions.Sort(ActionsSort);
            }
            if (action is IKeyAction) {
                KeyActions.Add((IKeyAction)action);
                KeyActions.Sort(ActionsSort);
            }
            if (action is IPaintAction) {
                PaintActions.Add((IPaintAction)action);
                PaintActions.Sort (ActionsSort);
            }
            if (action is ICommandAction) {
                CommandActions.Add((ICommandAction)action);
                CommandActions.Sort(ActionsSort);
            }
            if (action is IDragDropAction) {
                DragDropActions.Add((IDragDropAction)action);
                DragDropActions.Sort(ActionsSort);
            }
            Actions.Add(action.GetType(), action);
        }
        
        public void Remove(IAction action) {
            if (action is IMouseAction) {
                MouseActions.Remove((IMouseAction)action);
            }
            if (action is IPaintAction) {
                PaintActions.Remove((IPaintAction)action);
            }
            if (action is IKeyAction) {
                KeyActions.Remove((IKeyAction)action);
            }
            if (action is ICommandAction) {
                CommandActions.Remove((ICommandAction)action);
            }
            if (action is IDragDropAction) {
                DragDropActions.Remove((IDragDropAction)action);
            }
            Actions.Remove(action.GetType());
        }

        public void Add<T>(T value, ref T action) where T:class,IAction {
            if ((action != null) && (action != value)) {
                Remove(action);
                action.Dispose();
                
            }
            action = value;
            if (action != null) {
                Add(action);
            }

        }

        public T GetAction<T>() {
            return (T) Actions[typeof(T)];
        }

        #region IDisposable Member

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IMouseAction Member
        
        public void OnMouseDown(MouseEventArgs e) {
            Resolved = false;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseDown (e);
                    Resolved = mouseAction.Resolved || Resolved;
                    if (mouseAction.Exclusive) {
                        break;
                    }
                    //if (mouseAction is IDragDropAction &&
                    //    !( (IDragDropAction)mouseAction ).Dragging) {
                    //    break;
                    //}
                }
            }
        }

        public void OnMouseMove(MouseEventArgs e) {
            Resolved = false;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseMove(e);
                    Resolved = mouseAction.Resolved || Resolved;
                    if (mouseAction.Exclusive) {
                        break;
                    }
                }
            }
        }

        public void OnMouseHover(MouseEventArgs e) {
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseHover(e);
                }
            }
        }

        public void OnMouseUp(MouseEventArgs e) {
            Resolved = false;
            foreach (MouseActionBase mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    Resolved = mouseAction.Resolved || Resolved;
                    bool exclusive = mouseAction.Exclusive;
                    mouseAction.OnMouseUp(e);
                    if (exclusive||mouseAction.Exclusive) {
                        break;
                    }
                }
            }
        }

        #endregion

        #region IPaintAction Member

        public void OnPaint(PaintActionEventArgs e) {
            foreach (IPaintAction paintAction in PaintActions) {
                if (paintAction.Enabled)
                    paintAction.OnPaint(e);
            }
        }

        #endregion

        #region CommandActions
        public void CommandsExecute() {
            foreach (ICommandAction action in CommandActions) {
                if (action.Enabled)
                    action.Execute();
            }
            CommandsDone ();
        }

        public void CommandsDone() {
            foreach (ICommandAction action in CommandActions) {
                if (action.Enabled)
                    action.Done();
            }
        }

        public void CommandsInvoke() {
            foreach (ICommandAction action in CommandActions) {
                if (action.Enabled)
                    action.Invoke();
            }
        }
        #endregion

        #region IKeyAction Member

        public void OnKeyDown(KeyEventArgs e) {
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    keyAction.OnKeyDown(e);
                    Resolved = keyAction.Resolved || Resolved;
                    if (keyAction.Exclusive) {
                        break;
                    }
                }
            }
        }

        public void OnKeyPress(KeyPressEventArgs e) {
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    keyAction.OnKeyPress(e);
                    Resolved = keyAction.Resolved || Resolved;
                    if (keyAction.Exclusive || e.Handled) {
                        break;
                    }
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e) {
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    Resolved = keyAction.Resolved || Resolved;
                    bool exclusive = keyAction.Exclusive;
                    keyAction.OnKeyUp(e);
                    if (exclusive || e.Handled) {
                        break;
                    }
                }
            }
        }

        #endregion

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
        }

        #endregion
    }
}
