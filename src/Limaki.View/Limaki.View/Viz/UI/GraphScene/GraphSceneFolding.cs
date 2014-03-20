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
using System.Diagnostics;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;
using Xwt;

namespace Limaki.View.Viz.UI.GraphScene {

    public interface IGraphSceneFolding<TItem, TEdge> : IKeyAction
        where TItem: class 
        where TEdge: TItem, IEdge<TItem> {

        IBackendRenderer BackendRenderer { get; set; }
        ISelectionRenderer MoveResizeRenderer { get; set; }
        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        DataComparer<TItem> OrderBy { get; set; }
        bool RemoveOrphans { get; set; }
        void Clear ();
        bool Check ();
    }

    public class GraphSceneFolding<TItem, TEdge> : KeyActionBase, ICheckable, IGraphSceneFolding<TItem, TEdge> 
        where TItem:class where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFolding(): base() {}

        public IBackendRenderer BackendRenderer {get;set;}
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public Func<IGraphSceneLayout<TItem, TEdge>> Layout { get;set;}
        public Func<IGraphScene<TItem, TEdge>> SceneHandler {get;set;}

        private GraphSceneFacade<TItem,TEdge> _folder = null;
        public virtual GraphSceneFacade<TItem, TEdge> Folder {
            get {
                if (_folder == null) {
                    _folder = new GraphSceneFacade<TItem, TEdge>(this.SceneHandler, Layout());
                }
                _folder.Layout = Layout();
                _folder.OrderBy = this.OrderBy;
                _folder.RemoveOrphans = this.RemoveOrphans;
                return _folder;
            }
            set { _folder = value; }
        }

        public DataComparer<TItem> OrderBy { get; set; }

        public bool RemoveOrphans { get; set; }

        public virtual void Clear() {
            Folder.Clear ();
        }

        public override void OnKeyPressed( KeyActionEventArgs e ) {
            Trace.WriteLine (string.Format ("{0} {1}", e.Key, e.Modifiers));

            base.OnKeyPressed(e);
            bool act = !(Folder.Scene.Focused is TEdge);

            bool wasFiltered = Folder.IsFiltered;

            if (e.Key == Key.Delete && e.Modifiers == ModifierKeys.None) {
                if (MoveResizeRenderer != null) {
                    MoveResizeRenderer.Shape = null;
                    MoveResizeRenderer.ShowGrips = false;
                }
                Folder.Hide();
            } else
                if (act) { // don't handle edges, as this has errors

                    if ((e.Key == Key.NumPadAdd || e.Key == Key.Plus) && e.Modifiers == ModifierKeys.None) {
                        Folder.Expand(false);

                    } else if ((e.Key == Key.NumPadSubtract || e.Key == Key.Minus) && e.Modifiers == ModifierKeys.None) {
                        Folder.Collapse();

                    } else if ((e.Key == Key.NumPadDivide || e.Key == Key.Slash)) {
                        Folder.CollapseToFocused();

                    } else if ((e.Key == Key.NumPadMultiply || e.Key == Key.Asterisk) && e.Modifiers == ModifierKeys.Control) {
                        Folder.ShowAllData();

                    } else if (((e.Key == Key.NumPadMultiply || e.Key == Key.Asterisk) && e.Modifiers == ModifierKeys.None)
                                || (e.Key == Key.Plus && e.Modifiers == ModifierKeys.Shift)) {
                        Folder.Expand(true);

                    } else if ((e.Key == Key.Space && e.Modifiers == ModifierKeys.None)) {
                        Folder.Toggle();
                    }

                }

            if (wasFiltered != Folder.IsFiltered)
                BackendRenderer.Render ();

        }

        #region ICheckable Member

        public bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
            }
            if (this.Layout == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneLayout<TItem, TEdge>));
            }
            if (this.BackendRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IBackendRenderer));
            }
            return true;

        }

        #endregion
    }
}