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
        public class DeleteCommand:Command<IWidget,Scene> {
            public DeleteCommand(IWidget target, Scene parameter):base(target,parameter) {}
            public override void Execute() {
                this.Parameter.Remove (Target);
            }
        }
        public void Delete ( IWidget deleteRoot, Set<IWidget> done ) {
            if ( !done.Contains(deleteRoot) ) {
                foreach (IWidget delete in Scene.AffectedByChange(deleteRoot)) {
                    if (!done.Contains(delete)) {
                        Scene.Commands.Add(new DeleteCommand(delete, Scene));
                        done.Add(delete);
                    }
                }
                //Scene.Commands.Add(new Command<IWidget>(deleteRoot));
                Scene.Commands.Add(new DeleteCommand(deleteRoot, Scene));
                //Scene.Remove(deleteRoot);
                Scene.Focused = null;
                if (Scene.Hovered == deleteRoot) {
                    Scene.Hovered = null;
                }
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
                    
                    control.CommandsExecute();
                }
            }
        }
    }
}
