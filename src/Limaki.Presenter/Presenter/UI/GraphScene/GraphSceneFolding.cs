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
using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Graphs;


namespace Limaki.Presenter.UI {
    public class GraphSceneFolding<TItem, TEdge> : KeyActionBase, ICheckable
        where TItem:class where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFolding(): base() {}

        public IDeviceRenderer DeviceRenderer {get;set;}
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public Get<IGraphLayout<TItem, TEdge>> Layout { get;set;}
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

        public override void OnKeyDown( KeyActionEventArgs e ) {
            base.OnKeyDown(e);

            bool wasFiltered = Folder.IsFiltered;

            if ((e.Key == Key.Add && e.ModifierKeys==ModifierKeys.None)) {
                Folder.Expand (false);
            }

            if ((e.Key == Key.Subtract && e.ModifierKeys == ModifierKeys.None)) {
                Folder.Collapse ();
            }

            if ((e.Key == Key.Divide )) {
                Folder.CollapseToFocused ();
            }

            if (e.Key == Key.Multiply && e.ModifierKeys == ModifierKeys.Control){
                Folder.ShowAllData ();
            }

            if (e.Key == Key.Multiply&& e.ModifierKeys==ModifierKeys.None){
                Folder.Expand(true);
            }

            if ((e.Key == Key.Space && e.ModifierKeys == ModifierKeys.None)) {
                Folder.Toggle ();
            }

            if (e.Key == Key.Delete && e.ModifierKeys == ModifierKeys.None) {
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
                throw new CheckFailedException(this.GetType(), typeof(IGraphLayout<TItem, TEdge>));
            }
            if (this.DeviceRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IDeviceRenderer));
            }
            return true;

        }

        #endregion
    }
}