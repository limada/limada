/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Actions;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Modelling;

namespace Limaki.View.Viz.UI.GraphScene {
    /// <summary>
    /// Adds an item (but not a link)
    /// </summary>
    public class GraphItemAddAction<TItem,TEdge> : GraphItemMoveResizeAction<TItem,TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphItemAddAction() : base() {
            this.Priority = ActionPriorities.SelectionPriority - 200;
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            this.BaseMouseDown (e);
            Resolved = e.Button == MouseActionButtons.Left;
        }

        
        public virtual TItem NewItem {get;set;}
        public virtual TItem Last { get; set; }
        public IGraphModelFactory<TItem,TEdge> ModelFactory {get;set;}

        public TItem AddItem () {
            if (NewItem == null) {

                NewItem = ModelFactory.CreateItem (newCounter + ". Label");

                newCounter++;
                Scene.Add (NewItem);
                // see: OnMouseUp Scene.Graph.OnGraphChanged (NewItem, GraphChangeType.Add);
                Scene.Requests.Add (new LayoutCommand<TItem> (NewItem, LayoutActionType.Invoke));
                if (Scene.Focused != null) {
                    Scene.Requests.Add (new LayoutCommand<TItem> (Scene.Focused, LayoutActionType.Perform));
                    Scene.Selected.Remove (Scene.Focused);
                }

                Last = Scene.Focused;
                Scene.Focused = NewItem;
                resizing = true;
            }

            return NewItem;
        }

        private int newCounter = 1;
        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            if (NewItem == null) {

                AddItem ();

                this.MouseDownPos = e.Location;
 
            }
            base.OnMouseMoveResolved (e);
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            base.BaseMouseMove (e);
            //Resolved = Resolved && ( visual != null ) && !(visual is IVisualLink);
            if (Resolved) {
                OnMouseMoveResolved (e);
            } else {
                base.OnMouseMoveNotResolved (e);
            }
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            if (NewItem != null) {
                var shape = Scene.ItemShape (NewItem);
                var newSize = shape.Size;
                if (newSize.Height<10 || newSize.Width < 10) {
                    Scene.Remove (NewItem);
                    newCounter--;
                    Scene.Focused = default(TItem);
                    if (Last != null) {
                        Scene.Requests.Add (new LayoutCommand<TItem> (Last, LayoutActionType.Perform));
                    }
                    Scene.Requests.Add(new Command<TItem>(NewItem));
                } else {
                    Scene.Graph.OnGraphChange(NewItem, GraphEventType.Add);
                }
            }
            NewItem = default(TItem);
            base.OnMouseUp(e);
        }
        }
}