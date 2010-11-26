using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Widgets;
using Limaki.Widgets;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Presenter.Widgets.UI;
using Limaki.Presenter.WPF;

namespace Limaki.Presenter.WPF.Display {
    public abstract class WPFGraphSceneDisplay<TItem, TEdge> : WPFDisplay<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {}

    public class WPFGraphSceneDeviceComposer<TItem, TEdge> : WPFDeviceComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Limaki.Presenter.Display.Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new WPFGraphSceneLayer<TItem, TEdge>();
        }
    }


    public class WPFWidgetDisplay : WPFGraphSceneDisplay<IWidget, IEdgeWidget> {

        public override DisplayFactory<IGraphScene<IWidget, IEdgeWidget>> CreateDisplayFactory(WPFDisplay<IGraphScene<IWidget, IEdgeWidget>> device) {
            var result = new WidgetDisplayFactory();
            var deviceInstrumenter = new WPFWidgetDeviceComposer();
            deviceInstrumenter.Device = device;
            result.DeviceComposer = deviceInstrumenter;
            result.DisplayComposer = new WPFWidgetDisplayComposer();
            return result;
        }


    }

    public class WPFWidgetDisplayComposer : WidgetDisplayComposer {

        //public WidgetDragDrop DragDrop { get; set; }
        public override void Factor(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            base.Factor(aDisplay);
            var display = aDisplay as WidgetDisplay;
            display.GraphItemRenderer = new WPFWidgetRenderer();
        }

        public override void Compose(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            var display = aDisplay;
            base.Compose(display);

            
            //var DragDrop = new WidgetDragDrop(
            //   this.GraphScene,
            //   display.Device as IDragDopControl,
            //   this.Camera(),
            //   this.Layout());
            //DragDrop.Enabled = true;
            //display.EventControler.Add(DragDrop);

            var selector = display.EventControler.GetAction<GraphSceneFocusAction<IWidget, IEdgeWidget>>();
            if (selector != null) {
                //var catcher = new DragDropCatcher<GraphSceneFocusAction<IWidget, IEdgeWidget>>(selector, display.Device as IControl);
                //display.EventControler.Add(catcher);
            }

            var addEdgeAction = display.EventControler.GetAction<AddEdgeAction>();
            if (addEdgeAction != null) {
                //var catcher = new DragDropCatcher<AddEdgeAction>(addEdgeAction, display.Device as IControl);
                //display.EventControler.Add(catcher);
            }
            //var editor = new WidgetTextEditor(
            //    this.GraphScene,
            //    display.Device as ContainerControl,
            //    display,
            //    this.Camera(),
            //    this.Layout()
            //    );

            //display.EventControler.Add(editor);

        }
    }


    public class WPFWidgetDeviceComposer : WPFGraphSceneDeviceComposer<IWidget, IEdgeWidget> {
        public override void Factor(Display<IGraphScene<IWidget, IEdgeWidget>> display) {
            base.Factor(display);
        }

        public override void Compose(Display<IGraphScene<IWidget, IEdgeWidget>> display) {
            base.Compose(display);

        }
    }
}