using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Presenter.Widgets.Layout;
using Limaki.Presenter.Widgets.UI;
using Limaki.Widgets;

namespace Limaki.Presenter.Widgets {
    public class WidgetDisplayComposer : GraphSceneDisplayComposer<IWidget, IEdgeWidget> {
        public override void Factor(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            var display = aDisplay as WidgetDisplay;
            
            base.Factor(display);

            display.ModelFactory = Registry.Factory.Create < IGraphModelFactory<IWidget, IEdgeWidget>> ();
            display.GraphItemRenderer = new WidgetRenderer ();

            display.Layout = new ArrangerLayout<IWidget, IEdgeWidget>(this.GraphScene, display.StyleSheet);
        }

        protected IMouseAction AddGraphEdgeAction { get; set; }

        public override void Compose(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            var display = aDisplay as WidgetDisplay;
            
            base.Compose(display);

            GraphSceneFolding folding = new GraphSceneFolding ();
            folding.SceneHandler = this.GraphScene;
            folding.Layout = this.Layout;
            folding.DeviceRenderer = display.DeviceRenderer;
            folding.MoveResizeRenderer = display.MoveResizeRenderer;
            display.EventControler.Add (folding);

            var addGraphEdgeAction = new AddEdgeAction();
            addGraphEdgeAction.SceneHandler = this.GraphScene;
            Compose (display, addGraphEdgeAction, false);
            addGraphEdgeAction.LayoutHandler = this.Layout;
            addGraphEdgeAction.Enabled = false;
            display.EventControler.Add (addGraphEdgeAction);

            var focusCatcher = new FocusCatcher ();
            focusCatcher.OnSceneFocusChanged = display.OnSceneFocusChanged;
            display.EventControler.Add(focusCatcher);

        }
    }

    public class FocusCatcher : MouseActionBase, ICheckable {
        public FocusCatcher():base() {
            this.Priority = ActionPriorities.SelectionPriority;
        }

        public Action OnSceneFocusChanged { get; set; }
        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            OnSceneFocusChanged();
        }

        public override void OnMouseMove(MouseActionEventArgs e) {}

        public bool Check() {
            if (this.OnSceneFocusChanged == null) {
                throw new CheckFailedException(this.GetType(), typeof(Action));
            }
            return true;
        }


    }
}