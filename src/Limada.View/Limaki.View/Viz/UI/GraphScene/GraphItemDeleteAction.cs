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


using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;
using Xwt;
using System;
using Limaki.View.Common;

namespace Limaki.View.Viz.UI.GraphScene {
    /// <summary>
    /// Deletes an item
    /// </summary>
    public class GraphItemDeleteAction<TItem,TEdge>:KeyActionBase 
        where TEdge:TItem,IEdge<TItem> {
        
        public Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public virtual void Delete () {
            if (Scene.Selected.Count > 0) {
                var done = new Set<TItem> ();
                foreach (var item in Scene.Selected.Elements) {
                    Scene.Delete (item, done);
                }
                if (Scene.Focused != null) {
                    Scene.Requests.Add (
                        new StateChangeCommand<TItem> (Scene.Focused,
                            new Pair<UiState> (
                                UiState.Focus, UiState.None))
                    );
                }
                Scene.Focused = default (TItem);
                if (done.Contains (Scene.Hovered)) {
                    Scene.Requests.Add (
                        new StateChangeCommand<TItem> (Scene.Hovered,
                            new Pair<UiState> (
                                UiState.Hovered, UiState.None))
                    );
                    Scene.Hovered = default (TItem);
                }
            }
            if (MoveResizeRenderer != null) {
                MoveResizeRenderer.Shape = null;
                MoveResizeRenderer.ShowGrips = false;
            }
        }

        public override void OnKeyPressed( KeyActionEventArgs e ) {
            base.OnKeyPressed(e);
            if (e.Key == Key.Delete && (e.Modifiers == ModifierKeys.Control)) {
                Delete ();
            }
        }
   }
}