using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Display;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.Display;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Limaki.View.UI;
using Limaki.View.WPF;

namespace Limaki.View.WPF.Display {
    public abstract class WPFGraphSceneDisplay<TItem, TEdge> : WPFDisplay<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {}

    public class WPFGraphSceneDeviceComposer<TItem, TEdge> : WPFDeviceComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new WPFGraphSceneLayer<TItem, TEdge>();
        }
    }


    public class WPFVisualsDisplay : WPFGraphSceneDisplay<IVisual, IVisualEdge> {

        public override DisplayFactory<IGraphScene<IVisual, IVisualEdge>> CreateDisplayFactory(WPFDisplay<IGraphScene<IVisual, IVisualEdge>> device) {
            var result = new VisualsDisplayFactory();
            var deviceInstrumenter = new WPFVisualsDeviceComposer();
            deviceInstrumenter.Device = device;
            result.DeviceComposer = deviceInstrumenter;
            result.DisplayComposer = new WpfVisualsDisplayComposer();
            return result;
        }


    }

    public class WpfVisualsDisplayComposer : VisualsDisplayComposer {

        //public VisualsDragDrop DragDrop { get; set; }
        public override void Factor(Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            base.Factor(aDisplay);
            var display = aDisplay as GraphSceneDisplay<IVisual, IVisualEdge>;
            display.GraphItemRenderer = new WpfVisualsRenderer();
        }

        public override void Compose(Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            var display = aDisplay;
            base.Compose(display);

            
            //var DragDrop = new VisualsDragDrop(
            //   this.GraphScene,
            //   display.Device as IDragDopControl,
            //   this.Camera(),
            //   this.Layout());
            //DragDrop.Enabled = true;
            //display.EventControler.Add(DragDrop);

            var selector = display.EventControler.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>>();
            if (selector != null) {
                //var catcher = new DragDropCatcher<GraphSceneFocusAction<IVisual, IVisualEdge>>(selector, display.Device as IControl);
                //display.EventControler.Add(catcher);
            }

            var addEdgeAction = display.EventControler.GetAction<AddEdgeAction>();
            if (addEdgeAction != null) {
                //var catcher = new DragDropCatcher<AddEdgeAction>(addEdgeAction, display.Device as IControl);
                //display.EventControler.Add(catcher);
            }
            //var editor = new VisualsTextEditor(
            //    this.GraphScene,
            //    display.Device as ContainerControl,
            //    display,
            //    this.Camera(),
            //    this.Layout()
            //    );

            //display.EventControler.Add(editor);

        }
    }


    public class WPFVisualsDeviceComposer : WPFGraphSceneDeviceComposer<IVisual, IVisualEdge> {
        public override void Factor(Display<IGraphScene<IVisual, IVisualEdge>> display) {
            base.Factor(display);
        }

        public override void Compose(Display<IGraphScene<IVisual, IVisualEdge>> display) {
            base.Compose(display);

        }
    }
}