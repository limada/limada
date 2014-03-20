using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.View.Vidgets;

namespace Limaki.View.Viz.UI {

    public class DelegatingMouseAction : MouseActionBase, ICheckable {

        public DelegatingMouseAction (): base() {
            this.Priority = ActionPriorities.SelectionPriority;
        }

        public Action<MouseActionEventArgs> MouseDown { get; set; }

        public override void OnMouseDown (MouseActionEventArgs e) {
            base.OnMouseDown(e);
            if (MouseDown != null)
                MouseDown(e);
        }

        public override void OnMouseMove (MouseActionEventArgs e) { }

        public bool Check () {
            if (this.MouseDown == null) {
                throw new CheckFailedException(this.GetType(), typeof(Action));
            }
            return true;
        }


    }
}