using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Actions;
using Limaki.Common.Collections;

namespace Limaki.Winform.Widgets {
    /// <summary>
    /// Deletes a widget
    /// </summary>
    public class WidgetDeleter:KeyActionBase {
        ICamera camera = null;
        IControl control = null;
        public WidgetDeleter():base() {}

        public WidgetDeleter(Handler<Scene> sceneHandler, IControl control, ICamera camera)
            : this() {
            this.control = control;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        public void Delete ( IWidget deleteRoot, Set<IWidget> done ) {
            if ( !done.Contains(deleteRoot) ) {
                foreach (IWidget delete in Scene.Twig(deleteRoot)) {
                    if (!done.Contains(delete)) {
                        Scene.Commands.Add(new DeleteCommand(delete, Scene));
                        done.Add(delete);
                    }
                }
                
                Scene.Commands.Add(new DeleteCommand(deleteRoot, Scene));
                
                done.Add(deleteRoot);
            }
        }
        public override void OnKeyDown(System.Windows.Forms.KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.KeyCode == System.Windows.Forms.Keys.Delete) {
                if (Scene.Selected.Count >0) {
                    Set<IWidget> done = new Set<IWidget>();
                    foreach(IWidget widget in Scene.Selected.Elements) {
                        Delete (widget, done);
                    }
                    Scene.Focused = null;
                    if (done.Contains(Scene.Hovered) ) {
                        Scene.Hovered = null;
                    }              
                    control.CommandsExecute();
                }
            }
        }
    }
}
