/*
 * Limaki 
 * Version 0.08
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
using Limaki.Actions;

namespace Limaki.Drawing.UI {
    public class EventControlerBase : ActionBase, IDisposable, IPaintAction {
        public Dictionary<Type, IAction> Actions = new Dictionary<Type, IAction>();
        public List<IPaintAction> PaintActions = new List<IPaintAction>();
        public List<ILayoutControler> CommandActions = new List<ILayoutControler>();
        public List<IMouseAction> MouseActions = new List<IMouseAction>();

        protected int ActionsSort(IAction x, IAction y) {
            return x.Priority.CompareTo(y.Priority);
        }

        public virtual void Add(IAction action) {
            Actions.Add(action.GetType(), action);
            
            if (action is IMouseAction) {
                MouseActions.Add((IMouseAction)action);
                MouseActions.Sort(ActionsSort);
            }

            if (action is IPaintAction) {
                PaintActions.Add((IPaintAction)action);
                PaintActions.Sort(ActionsSort);
            }
            if (action is ILayoutControler) {
                CommandActions.Add((ILayoutControler)action);
                CommandActions.Sort(ActionsSort);
            }
        }

        public virtual void Remove(IAction action) {
            Actions.Remove(action.GetType());
            
            if (action is IMouseAction) {
                MouseActions.Remove((IMouseAction)action);
            }

            if (action is IPaintAction) {
                PaintActions.Remove((IPaintAction)action);
            }

            if (action is ILayoutControler) {
                CommandActions.Remove((ILayoutControler)action);
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
            else
                return default(T);
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
            foreach (IMouseAction mouseAction in MouseActions) {
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
            CommandsExecute();
        }

        public void OnMouseMove(MouseActionEventArgs e) {
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
            CommandsExecute();
        }

        public void OnMouseHover(MouseActionEventArgs e) {
            foreach (IMouseAction mouseAction in MouseActions) {
                if (mouseAction.Enabled) {
                    mouseAction.OnMouseHover(e);
                }
            }
            CommandsExecute();
        }

        public void OnMouseUp(MouseActionEventArgs e) {
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
            CommandsExecute();
        }

        #endregion

        #region IPaintAction Member

        public void OnPaint(IPaintActionEventArgs e) {
            foreach (IPaintAction paintAction in PaintActions) {
                if (paintAction.Enabled)
                    paintAction.OnPaint(e);
            }
        }

        #endregion

        #region CommandActions
        public void CommandsExecute() {
            foreach (ILayoutControler action in CommandActions) {
                if (action.Enabled)
                    action.Execute();
            }
            CommandsDone();
        }

        public void CommandsDone() {
            foreach (ILayoutControler action in CommandActions) {
                if (action.Enabled)
                    action.Done();
            }
        }

        public void CommandsInvoke() {
            foreach (ILayoutControler action in CommandActions) {
                if (action.Enabled)
                    action.Invoke();
            }
            CommandsExecute();
        }

        #endregion

    }
}