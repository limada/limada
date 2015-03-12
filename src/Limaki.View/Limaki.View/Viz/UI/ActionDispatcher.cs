/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.View.DragDrop;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// a Chain of Responsibility
    /// to Invoke commands (Invoker of the Command Pattern)
    /// to Execute commands by the Actions stored in the ReceiverActions (Receiver of the Command Pattern)
    /// </summary>
    public class ActionDispatcher : ActionBase, IActionDispatcher, IDropAction, IDisposable, ICopyPasteAction {

        IDictionary<Type, IAction> _actions = null;
        public IDictionary<Type, IAction> Actions {
            get {
                if (_actions == null) {
                    _actions = new Dictionary<Type, IAction> ();
                }
                return _actions;
            }
        }

        public virtual bool UserEventsDisabled { get; set; }
        // Invokers:
        public List<IMouseAction> MouseActions = new List<IMouseAction>();
        public List<IKeyAction> KeyActions = new List<IKeyAction>();

        // Receivers:
        public List<IReceiver> ReceiverActions = new List<IReceiver>();
        public List<IRenderAction> RenderActions = new List<IRenderAction>();

        // DragDrop:
        public List<IDropAction> DragDropActions = new List<IDropAction>();

        // CopyPaste:
        public List<ICopyPasteAction> CopyPasteActions = new List<ICopyPasteAction> ();

        public int ActionsSort (IAction x, IAction y) {
            return x.Priority.CompareTo(y.Priority);
        }

        public virtual void Add(IAction action) {
            Actions.Add(action.GetType(), action);
            
            if (action is IMouseAction) {
                MouseActions.Add((IMouseAction)action);
                MouseActions.Sort(ActionsSort);
            }

            if (action is IRenderAction) {
                RenderActions.Add((IRenderAction)action);
                RenderActions.Sort(ActionsSort);
            }
            
            if (action is IKeyAction) {
                KeyActions.Add((IKeyAction)action);
                KeyActions.Sort(ActionsSort);
            }

            if (action is IReceiver) {
                ReceiverActions.Add((IReceiver)action);
                ReceiverActions.Sort(ActionsSort);
            }

            if (action is IDropAction) {
                DragDropActions.Add((IDropAction)action);
                DragDropActions.Sort(ActionsSort);
            }

            if (action is ICopyPasteAction) {
                CopyPasteActions.Add ((ICopyPasteAction) action);
                CopyPasteActions.Sort (ActionsSort);
            }
        }

        public virtual void Remove(IAction action) {
            if (action == null)
                return; 

            Actions.Remove(action.GetType());
            
            if (action is IMouseAction) {
                MouseActions.Remove((IMouseAction)action);
            }

            if (action is IRenderAction) {
                RenderActions.Remove((IRenderAction)action);
            }

            if (action is IKeyAction) {
                KeyActions.Remove((IKeyAction)action);
            }

            if (action is IReceiver) {
                ReceiverActions.Remove((IReceiver)action);
            }

            if (action is IDropAction) {
                DragDropActions.Remove((IDropAction)action);
            }
        }

        public virtual void Add<T>(T value, ref T action) where T : class, IAction {
            if ((action != null) && (action != value)) {
                Remove(action);
                action.Dispose();

            }
            action = value;
            if (action != null) {
                Add(action);
            }

        }

        public virtual T GetAction<T>() {
            IAction result = null;
            if (Actions.TryGetValue(typeof(T), out result))
                return (T)result;
            else { // try harder:
                T r = default(T);
                foreach (var pair in Actions) {
                    if (pair.Value is T) {
                        r = (T) pair.Value;
                        break;
                    }
                }
                return r;
            }
            
        }

        #region IDisposable Member

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IMouseAction Member

        public void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            if (UserEventsDisabled)
                return;
            foreach (var mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseDown(e);
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
            Perform();
        }

        public void OnMouseMove(MouseActionEventArgs e) {
            Resolved = false;
            if (UserEventsDisabled)
                return;
            foreach (var mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseMove(e);
                    Resolved = mouseAction.Resolved || Resolved;
                    if (mouseAction.Exclusive) {
                        break;
                    }
                }
            }
            Perform();
        }

        public void OnMouseHover(MouseActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseHover(e);
                }
            }
            Perform();
        }

        public void OnMouseUp(MouseActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            Resolved = false;
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    Resolved = mouseAction.Resolved || Resolved;
                    bool exclusive = mouseAction.Exclusive;
                    mouseAction.OnMouseUp(e);
                    if (exclusive || mouseAction.Exclusive) {
                        break;
                    }
                }
            }
            Perform();
        }

        #endregion

        #region IKeyAction Member

        public void OnKeyPressed(KeyActionEventArgs e) {
            if (UserEventsDisabled)
                return;
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    keyAction.OnKeyPressed(e);
                    Resolved = keyAction.Resolved || Resolved;
                    if (keyAction.Exclusive) {
                        break;
                    }
                }
            }
            Perform();
        }

        public void OnKeyReleased(KeyActionEventArgs e) {
            Resolved = false;
            foreach (IKeyAction keyAction in KeyActions) {
                if (keyAction.Enabled) {
                    Resolved = keyAction.Resolved || Resolved;
                    bool exclusive = keyAction.Exclusive;
                    keyAction.OnKeyReleased(e);
                    if (exclusive || e.Handled) {
                        break;
                    }
                }
            }
            Perform();
        }

        #endregion

        #region IRenderAction Member

        public void OnPaint(IRenderEventArgs e) {
            foreach (var renderAction in RenderActions) {
                if (renderAction.Enabled)
                    renderAction.OnPaint(e);
            }
        }

        #endregion

        #region IReceiver Member
        public void Perform() {
            foreach (var action in ReceiverActions) {
                if (action.Enabled)
                    action.Perform();
            }
            Finish();
        }

        public void Finish() {
            foreach (var action in ReceiverActions) {
                if (action.Enabled)
                    action.Finish();
            }
        }

        public void Reset() {
            foreach (var action in ReceiverActions) {
                if (action.Enabled)
                    action.Reset();
            }
            Perform();
        }

        #endregion

        #region DragDrop

        public virtual bool Dragging { get; set; }

        public virtual void DragOver (DragOverEventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.DragOver(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive) {//} || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Perform();
        }

        public virtual void OnDrop (DragEventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnDrop(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive) {//} || ! dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Perform();
        }

        public virtual void DragLeave (EventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.DragLeave(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Perform();
        }

        #endregion

        public virtual void Copy () {
            Resolved = false;
            foreach (var action in CopyPasteActions) {
                if (action.Enabled) {
                    action.Copy ();
                    Resolved = action.Resolved || Resolved;
                    if (action.Exclusive) {
                        break;
                    }
                }
            }
            Perform ();
        }

        public virtual void Paste () {
            Resolved = false;
            foreach (var action in CopyPasteActions) {
                if (action.Enabled) {
                    action.Paste ();
                    Resolved = action.Resolved || Resolved;
                    if (action.Exclusive) {
                        break;
                    }
                }
            }
            Perform ();
        }

        
    }
}