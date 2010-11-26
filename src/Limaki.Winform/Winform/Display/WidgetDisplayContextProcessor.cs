using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;

namespace Limaki.Winform.Displays {
    public class WidgetDisplayContextProcessor : DisplayContextProcessor<Scene> {
        public override void ApplyProperties(IApplicationContext context, DisplayBase<Scene> target) {
            base.ApplyProperties (context, target);
            
            WidgetDisplay display = target as WidgetDisplay;
            
            if (display != null) {
                ( (WidgetKit) display.DisplayKit ).ShapeFactory = context.Factory.One<IShapeFactory> ();
                ( (WidgetKit) display.DisplayKit ).ShapeFactory.Add<RectangleI, RoundedRectangleShape> ();
                
                ( (WidgetKit) display.DisplayKit ).PainterFactory = context.Factory.One<IPainterFactory> ();
            }
        }
    }
}