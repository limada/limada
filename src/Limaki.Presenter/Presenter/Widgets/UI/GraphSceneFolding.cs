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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Widgets;

namespace Limaki.Presenter.Widgets.UI {
    public class GraphSceneFolding : KeyActionBase, ICheckable {
        public GraphSceneFolding(): base() {}

        public IDeviceRenderer DeviceRenderer {get;set;}
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }

        public Get<IGraphLayout<IWidget, IEdgeWidget>> Layout { get;set;}
        public Get<IGraphScene<IWidget, IEdgeWidget>> SceneHandler {get;set;}

        private GraphSceneFacade<IWidget,IEdgeWidget> _folder = null;
        public virtual GraphSceneFacade<IWidget, IEdgeWidget> folder {
            get {
                if (_folder == null) {
                    _folder = new GraphSceneFacade<IWidget, IEdgeWidget>(this.SceneHandler, Layout());
                }
                _folder.Layout = Layout();
                return _folder;
            }
            set { _folder = value; }
        }

        public virtual void Clear() {
            folder.Clear ();
        }

        

        public override void OnKeyDown( KeyActionEventArgs e ) {
            base.OnKeyDown(e);

            bool wasFiltered = folder.IsFiltered;

            if ((e.Key == Key.Add)) {
                folder.Expand (false);
            }

            if ((e.Key == Key.Subtract)) {
                folder.Collapse ();
            }

            if ((e.Key == Key.Divide)) {
                folder.CollapseToFocused ();
            }

            if (((e.Key == Key.Multiply))&& e.ModifierKeys == ModifierKeys.Control){
                folder.ShowAllData ();
            }

            if (((e.Key == Key.Multiply))
                && e.ModifierKeys==ModifierKeys.None){
                folder.Expand(true);
            }

            if ((e.Key == Key.Space)) {
                folder.Toggle ();
            }

            if (e.Key == Key.Delete && e.ModifierKeys == ModifierKeys.None) {
                if (MoveResizeRenderer != null) {
                    MoveResizeRenderer.Shape = null;
                    MoveResizeRenderer.ShowGrips = false;
                }
                folder.Hide();
            }

            if (wasFiltered != folder.IsFiltered)
                DeviceRenderer.Render ();

        }



        #region ICheckable Member

        public bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<IWidget, IEdgeWidget>));
            }
            if (this.Layout == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphLayout<IWidget, IEdgeWidget>));
            }
            if (this.DeviceRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IDeviceRenderer));
            }
            return true;

        }

        #endregion
    }
}