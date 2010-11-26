using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Context;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Painters;
using Limaki.Drawing.WPF.Shapes;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using Limaki.Widgets.WPF;

namespace Limaki.Presenter.WPF {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader an application
    /// which uses StyleSheets, a WidgetDisplay and IGraphMapping
    /// </summary>
    public class WPFContextRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader().ApplyResources(context);


            context.Factory.Add<IExceptionHandler, WPFExeptionHandler>();
            context.Factory.Add<IDrawingUtils, WPFDrawingUtils>();
            context.Factory.Add<ISystemFonts, WPFSystemFonts>();
            context.Factory.Add<IPainterFactory, WPFPainterFactory>();

            context.Factory.Add<IUISystemInformation, WPFSystemInformation>();
            context.Factory.Add<IShapeFactory, WPFShapeFactory>();
            context.Factory.Add<IWidgetFactory, WPFWidgetFactory>();

            context.Factory.Add<IDeviceCursor, CursorHandler>();

            new ViewContextRecourceLoader().ApplyResources(context);


        }




    }
}