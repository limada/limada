using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Actions;

namespace Limaki.Winform.Widgets {
    /// <summary>
    /// Deletes a widget
    /// </summary>
    public class WidgetDeleter:KeyActionBase {
        ITransformer transformer = null;
        IControl control = null;
        public WidgetDeleter():base() {}

        public WidgetDeleter(Handler<Scene> sceneHandler, IControl control, ITransformer transformer)
            : this() {
            this.control = control;
            this.transformer = transformer;
            this.SceneHandler = sceneHandler;
            
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        public override void OnKeyDown(System.Windows.Forms.KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.KeyCode == System.Windows.Forms.Keys.Delete) {
                if (Scene.Selected != null) {
                    IWidget deleteRoot = Scene.Selected;
                    List<IWidget> deleteList = new List<IWidget> ();
                    foreach(IWidget delete in Scene.AffectedByChange(deleteRoot)) {
                        Scene.Commands.Add(new Command<IWidget>(delete));
                    }
                    Scene.Commands.Add(new Command<IWidget>(deleteRoot));
                    Scene.Remove(deleteRoot);
                    Scene.Selected = null;
                    if (Scene.Hovered == deleteRoot) {
                        Scene.Hovered = null;
                    }
                    control.CommandsExecute();
                }
            }
        }
    }
}
