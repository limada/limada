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

using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Layout;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// Adds a widget (but not a linkWidget)
    /// </summary>
    public class GraphItemAddAction<TItem,TEdge> : GraphItemMoveResizeAction<TItem,TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public GraphItemAddAction() : base() {
            this.Priority = ActionPriorities.SelectionPriority - 20;
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            this.BaseMouseDown (e);
            Resolved = e.Button == MouseActionButtons.Left;
        }

        TItem _newWidget = default(TItem);
        public virtual TItem NewWidget {
            get { return _newWidget; }
            set { _newWidget = value; }
        }

        TItem _last = default(TItem);
        public virtual TItem last {
            get { return _last; }
            set { _last = value; }
        }

        public IGraphModelFactory<TItem,TEdge> ModelFactory {get;set;}

        private int newCounter = 1;
        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            if (NewWidget == null) {

                NewWidget = ModelFactory.CreateItem(newCounter + ". Label");

                newCounter++;
                Scene.Add(NewWidget);
                // see: OnMouseUp Scene.Graph.OnGraphChanged (NewWidget, GraphChangeType.Add);
                Scene.Requests.Add(new LayoutCommand<TItem>(NewWidget, LayoutActionType.Invoke));
                if (Scene.Focused != null) {
                    Scene.Requests.Add (new LayoutCommand<TItem> (Scene.Focused, LayoutActionType.Perform));
                    Scene.Selected.Remove (Scene.Focused);
                }
                this.MouseDownPos = e.Location;
                last = Scene.Focused;
                Scene.Focused = NewWidget;
                resizing = true;
 
            }
            base.OnMouseMoveResolved (e);
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            base.BaseMouseMove (e);
            //Resolved = Resolved && ( Widget != null ) && !(Widget is ILinkWidget);
            if (Resolved) {
                OnMouseMoveResolved (e);
            } else {
                base.OnMouseMoveNotResolved (e);
            }
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            if (NewWidget != null) {
                var shape = Scene.ItemShape (NewWidget);
                SizeI newSize = shape.Size;
                if (newSize.Height<10 || newSize.Width < 10) {
                    Scene.Remove (NewWidget);
                    newCounter--;
                    Scene.Focused = default(TItem);
                    if (last != null) {
                        Scene.Requests.Add (new LayoutCommand<TItem> (last, LayoutActionType.Perform));
                    }
                    Scene.Requests.Add(new Command<TItem>(NewWidget));
                } else {
                    Scene.Graph.OnGraphChanged(NewWidget, GraphChangeType.Add);
                }
            }
            NewWidget = default(TItem);
            base.OnMouseUp(e);
        }
        }
}