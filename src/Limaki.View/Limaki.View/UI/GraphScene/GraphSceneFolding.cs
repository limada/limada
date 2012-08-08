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


using System.ComponentModel;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Rendering;
using Xwt;

namespace Limaki.View.UI.GraphScene {

    public interface IGraphSceneFolding<TItem, TEdge> : IKeyAction
        where TItem: class 
        where TEdge: TItem, IEdge<TItem> {

        IBackendRenderer DeviceRenderer { get; set; }
        ISelectionRenderer MoveResizeRenderer { get; set; }
        Get<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        Get<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        GraphSceneFacade<TItem, TEdge> Folder { get; set; }

        void Clear ();
        bool Check ();

    }

    public class GraphSceneFolding<TItem, TEdge> : KeyActionBase, ICheckable, IGraphSceneFolding<TItem, TEdge> 
        where TItem:class where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFolding(): base() {}

        public IBackendRenderer DeviceRenderer {get;set;}
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public Get<IGraphSceneLayout<TItem, TEdge>> Layout { get;set;}
        public Get<IGraphScene<TItem, TEdge>> SceneHandler {get;set;}

        private GraphSceneFacade<TItem,TEdge> _folder = null;
        public virtual GraphSceneFacade<TItem, TEdge> Folder {
            get {
                if (_folder == null) {
                    _folder = new GraphSceneFacade<TItem, TEdge>(this.SceneHandler, Layout());
                }
                _folder.Layout = Layout();
                return _folder;
            }
            set { _folder = value; }
        }

        public virtual void Clear() {
            Folder.Clear ();
        }

        public override void OnKeyPressed( KeyActionEventArgs e ) {
            base.OnKeyPressed(e);

            bool wasFiltered = Folder.IsFiltered;

            if ((e.Key == Key.NumPadAdd && e.Modifiers==ModifierKeys.None)) {
                Folder.Expand (false);
            }

            if ((e.Key == Key.NumPadSubtract && e.Modifiers == ModifierKeys.None)) {
                Folder.Collapse ();
            }

            if ((e.Key == Key.NumPadDivide )) {
                Folder.CollapseToFocused ();
            }

            if (e.Key == Key.NumPadMultiply && e.Modifiers == ModifierKeys.Control){
                Folder.ShowAllData ();
            }

            if (e.Key == Key.NumPadMultiply && e.Modifiers == ModifierKeys.None) {
                Folder.Expand(true);
            }

            if ((e.Key == Key.Space && e.Modifiers == ModifierKeys.None)) {
                Folder.Toggle ();
            }

            if (e.Key == Key.Delete && e.Modifiers == ModifierKeys.None) {
                if (MoveResizeRenderer != null) {
                    MoveResizeRenderer.Shape = null;
                    MoveResizeRenderer.ShowGrips = false;
                }
                Folder.Hide();
            }

            if (wasFiltered != Folder.IsFiltered)
                DeviceRenderer.Render ();

        }

        #region ICheckable Member

        public bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
            }
            if (this.Layout == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneLayout<TItem, TEdge>));
            }
            if (this.DeviceRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IBackendRenderer));
            }
            return true;

        }

        #endregion
    }
}