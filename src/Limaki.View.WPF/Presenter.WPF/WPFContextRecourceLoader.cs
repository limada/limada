using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Painters;
using Limaki.Drawing.WPF.Shapes;
using Limaki.View.UI;
using Limaki.Visuals;
using Limaki.Visuals.WPF;
using Xwt;


namespace Limaki.View.WPF {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader an application
    /// which uses StyleSheets, a VisualsDisplay and IGraphMapping
    /// </summary>
    public class WPFContextRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader().ApplyResources(context);
            var tk = Toolkit.CreateToolkit<Xwt.WPFBackend.WPFEngine>(false);
            tk.SetActive();

            context.Factory.Add<IExceptionHandler, WPFExeptionHandler>();
            context.Factory.Add<IDrawingUtils, WPFDrawingUtils>();
            context.Factory.Add<ISystemFonts, WPFSystemFonts>();
            context.Factory.Add<IPainterFactory, WPFPainterFactory>();

            context.Factory.Add<IUISystemInformation, WPFSystemInformation>();
            context.Factory.Add<IShapeFactory, WPFShapeFactory>();
            context.Factory.Add<IVisualFactory, WpfVisualFactory>();

            context.Factory.Add<ICursorHandler, CursorHandlerBackend>();

            new ViewContextRecourceLoader().ApplyResources(context);


        }




    }
}