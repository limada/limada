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


using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;
using System.Linq;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Modelling {

    public class GraphItemModeller<TItem,TEdge> : ICommandModeller<TItem> 
        where TEdge:TItem,IEdge<TItem> {

        public virtual IGraphScene<TItem,TEdge> Data { get; set; }
        public virtual IGraphSceneLayout<TItem, TEdge> Layout { get; set; }

        public Action<ICommand<TItem>> BeforePerform { get; set; }
        public Action<ICommand<TItem>> AfterPerform { get; set; }

        public virtual void Perform(ICommand<TItem> request) {

            if (deleted.Contains (request.Subject))
                return;

            if (request is LayoutCommand<TItem>) {
                var layoutCommand = (LayoutCommand<TItem>)request;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Refresh(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Perform(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.BoundsChanged(layoutCommand.Subject);
                }
            } else if (request is LayoutCommand<TItem, IShape>) {
                var layoutCommand = (LayoutCommand<TItem, IShape>)request;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Subject, layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Refresh(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Perform(layoutCommand.Subject, layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.BoundsChanged(layoutCommand.Subject);
                }
            } else if (request is DeleteCommand<TItem,TEdge>) {
                deleted.Add (request.Subject);
                request.Execute();
            } else {
                var invalid = Rectangle.Zero;
                var shape = Data.ItemShape (request.Subject);

                if (shape != null) {
                    invalid = shape.BoundsRect;
                }

                request.Execute();
                
                if (invalid != Rectangle.Zero)
                    Data.UpdateBounds(request.Subject, invalid);
                else
                    Data.AddBounds(request.Subject);
            }
            if (request is IDirtyCommand) {
                Data.State.Dirty = true;
            }
        }

        protected int tolerance = 5;
        protected HashSet<TItem> deleted = new HashSet<TItem> ();
        public virtual void Perform(ICollection<ICommand<TItem>> requests) {
            if (Data != null && Data.Requests.Count != 0) {
                var clipChanged = false;
                deleted.Clear ();
                foreach (var command in requests.ToArray()) {
                    if (command != null && command.Subject != null) {

                        if (BeforePerform != null) {
                            BeforePerform(command);
                        }

                        Perform(command);

                        if (AfterPerform != null) {
                            AfterPerform(command);
                        }

                        clipChanged = true;
                    }
                }
                deleted.Clear ();
            }
        }
        }
}