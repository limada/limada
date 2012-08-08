using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Limaki.Visuals;
using Xwt;
using Xwt.Html5.Backend;
using Limaki.Common.IOC;
using Limaki.Drawing.Shapes;
using Limaki.View.UI;
using Limaki.IOC;
using Limaki.Viewers;

using Limaki.Drawing.Painters;

using Xwt.Engine;

namespace Limaki.View.Html5 {

    public class Html5ContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {
            
            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            var engine = new Html5Engine ();
            engine.InitializeRegistry(new WidgetRegistry());
            WidgetRegistry.MainRegistry = Html5Engine.Registry;
            Html5Engine.Registry.RegisterBackend(
                typeof (Xwt.Drawing.SystemColors), typeof (SystemColorsBackend)
                );

            context.Factory.Add<IExceptionHandler, Html5ExeptionHandlerBackend> ();
            context.Factory.Add<IDrawingUtils, Html5DrawingUtils> ();
            context.Factory.Add<ISystemFonts, Html5SystemFonts> ();
            
            context.Factory.Add<IPainterFactory, DefaultPainterFactory> ();

            context.Factory.Add<IUISystemInformation, Html5SystemInformation> ();
            context.Factory.Add<IShapeFactory, ShapeFactory> ();
            context.Factory.Add<IVisualFactory, VisualFactory> ();

            //context.Factory.Add<ICursorHandler, CursorHandlerBackend> ();
            //context.Factory.Add<IDisplay<IGraphScene<IVisual, IVisualEdge>>> (() => new Html5VisualsDisplayBackend ().Display);
            //context.Factory.Add<IMessageBoxShow, MessageBoxShow> ();



            new ViewContextRecourceLoader ().ApplyResources (context);


        }




    }
}