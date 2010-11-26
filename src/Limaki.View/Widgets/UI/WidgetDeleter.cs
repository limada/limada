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
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Deletes a widget
    /// </summary>
    public class WidgetDeleter:KeyActionBase {
        ICamera camera = null;
        IControl control = null;
        public WidgetDeleter():base() {}

        public WidgetDeleter(Func<Scene> sceneHandler, IControl control, ICamera camera)
            : this() {
            this.control = control;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            
        }

        ///<directed>True</directed>
        Func<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        public void Delete ( IWidget deleteRoot, Set<IWidget> done ) {
            if ( !done.Contains(deleteRoot) ) {
                foreach (IWidget delete in Scene.Graph.PostorderTwig(deleteRoot)) {
                    if (!done.Contains(delete)) {
                        Scene.Commands.Add(new DeleteEdgeCommand(delete, Scene));
                        done.Add(delete);
                    }
                }
                
                Scene.Commands.Add(new DeleteCommand(deleteRoot, Scene));
                
                done.Add(deleteRoot);
            }
        }

        public override void OnKeyDown( KeyActionEventArgs e ) {
            base.OnKeyDown(e);
            if (e.Key == Key.Delete && (e.ModifierKeys==ModifierKeys.Control)) {
                if (Scene.Selected.Count >0) {
                    Set<IWidget> done = new Set<IWidget>();
                    foreach(IWidget widget in Scene.Selected.Elements) {
                        Delete (widget, done);
                    }
                    Scene.Focused = null;
                    if (done.Contains(Scene.Hovered) ) {
                        Scene.Hovered = null;
                    }              
                }
            }
        }
    }
}