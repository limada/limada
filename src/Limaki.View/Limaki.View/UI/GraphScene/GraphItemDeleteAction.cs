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
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.UI.GraphScene {
    /// <summary>
    /// Deletes an item
    /// </summary>
    public class GraphItemDeleteAction<TItem,TEdge>:KeyActionBase 
        where TEdge:TItem,IEdge<TItem> {
        
        public Get<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public void Delete ( TItem deleteRoot, Set<TItem> done ) {
            if ( !done.Contains(deleteRoot) ) {
                foreach (TItem delete in Scene.Graph.PostorderTwig(deleteRoot)) {
                    if (!done.Contains(delete)) {
                        Scene.Requests.Add(new DeleteEdgeCommand<TItem,TEdge>(delete, Scene));
                        done.Add(delete);
                    }
                }
                
                Scene.Requests.Add(new DeleteCommand<TItem,TEdge>(deleteRoot, Scene));
                
                done.Add(deleteRoot);
            }
        }

        public override void OnKeyDown( KeyActionEventArgs e ) {
            base.OnKeyDown(e);
            if (e.Key == Key.Delete && (e.ModifierKeys==ModifierKeys.Control)) {
                if (Scene.Selected.Count >0) {
                    Set<TItem> done = new Set<TItem>();
                    foreach(TItem item in Scene.Selected.Elements) {
                        Delete (item, done);
                    }
                    if (Scene.Focused != null) {
                        Scene.Requests.Add (
                            new StateChangeCommand<TItem> (Scene.Focused,
                                                           new Pair<UiState> (
                                                               UiState.Focus, UiState.None))
                            );
                    }
                    Scene.Focused = default(TItem);
                    if (done.Contains(Scene.Hovered) ) {
                        Scene.Requests.Add(
                            new StateChangeCommand<TItem>(Scene.Hovered,
                                                           new Pair<UiState>(
                                                               UiState.Hovered, UiState.None))
                            );
                        Scene.Hovered = default(TItem);
                    }              
                }
                if (MoveResizeRenderer != null) {
                    MoveResizeRenderer.Shape = null;
                    MoveResizeRenderer.ShowGrips = false;
                }
            }
        }
        }
}