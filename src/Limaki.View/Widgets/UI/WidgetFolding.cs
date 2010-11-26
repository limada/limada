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


using System;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Widgets.Layout;

namespace Limaki.Widgets.UI {
    public class WidgetFolding : KeyActionBase {
        public WidgetFolding(Func<Scene> sceneHandler, IControl control, ILayout<Scene, IWidget> layout)
            : base() {
            this.SceneHandler = sceneHandler;
            this.control = control;
            this.Layout = layout;
        }

        public IControl control = null;

        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        Func<Scene> SceneHandler = null;


        private SceneFacade _folder = null;
        public virtual SceneFacade folder {
            get {
                if (_folder == null) {
                    _folder = new SceneFacade(this.SceneHandler,Layout);
                }
                _folder.Layout = Layout;
                return _folder;
            }
            set { _folder = value; }
        }

        public virtual void Clear() {
            folder.Clear ();
        }

        

        public override void OnKeyDown( KeyActionEventArgs e ) {
            base.OnKeyDown(e);

            folder.Layout = this.Layout;

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
                folder.Hide();
            }

            if (wasFiltered != folder.IsFiltered)
                control.Invalidate ();

        }


    }
}