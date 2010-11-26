/*
 * Limaki 
 * Version 0.071
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


using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Winform.Widgets {
    public class WidgetFolding : KeyActionBase {
        public WidgetFolding(Handler<Scene> sceneHandler, IControl control, ILayout<Scene, IWidget> layout)
            : base() {
            this.SceneHandler = sceneHandler;
            this.control = control;
            this.Layout = layout;
        }

        public IControl control = null;
        
        private ILayout<Scene,IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        Handler<Scene> SceneHandler = null;
        Scene _scene = null;
        public virtual Scene Scene {
            get {
                Scene scene = SceneHandler();
                if (scene != _scene) {
                    Clear();
                    _scene = scene;
                    folder.Scene = _scene;
                }
                return _scene;
            }
        }

        private FoldingControler _folder = null;
        public FoldingControler folder {
            get {
                if (_folder == null) {
                    _folder = new FoldingControler();
                    _folder.Scene = SceneHandler();
                    _folder.Layout = _layout;
                }
                return _folder;
            }
            set { _folder = value; }
        }

        void Clear() {
            _scene = null;
            folder.Clear ();
        }

        

        public override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            folder.Scene = this.Scene;
            folder.Layout = this.Layout;

            bool wasFiltered = folder.IsFiltered;

            if ((e.KeyCode == Keys.Add) || (e.KeyData == Keys.Oemplus)) {
                folder.Expand ();
                //control.Invalidate ();
            }

            if ((e.KeyCode == Keys.Subtract) || (e.KeyData == Keys.OemMinus)) {
                folder.Collapse ();
                //control.Invalidate();
            }

            if ((e.KeyCode == Keys.Divide)) {
                folder.CollapseToFocused ();
                //control.Invalidate();
            }


            if ((e.KeyCode == Keys.Multiply) ||
                (e.KeyCode == Keys.OemQuestion)) {
                folder.ShowAllData ();
                //control.Invalidate();
            }

            if (wasFiltered != folder.IsFiltered)
                control.Invalidate ();

        }


    }

}