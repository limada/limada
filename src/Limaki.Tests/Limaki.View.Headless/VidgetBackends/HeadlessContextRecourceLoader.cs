using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.IOC;
using Limaki.View.DragDrop;
using Limaki.View.Headless.VidgetBackends;
using Limaki.View.HeadlessBackends;
using Limaki.View.UI;
using Limaki.View.XwtContext;
using Limaki.Viewers;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;
using Xwt.Headless.Backend;

namespace Limaki.View.Headless {

    public class HeadlessContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyHeadlessResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            if (Toolkit.CurrentEngine == null) {
                tk = Toolkit.CreateToolkit<HeadlessEngine> (false);
                tk.SetActive();
            }

            
            if (tk.Backend is HeadlessEngine) {
                tk.RegisterBackend<SystemColorsBackend, HeadlessSystemColorsBackend>();

                context.Factory.Add<IExceptionHandler, HeadlessExeptionHandlerBackend>();
                context.Factory.Add<IDrawingUtils, HeadlessDrawingUtils>();
                context.Factory.Add<ISystemFonts, HeadlessSystemFonts>();

                context.Factory.Add<IPainterFactory, DefaultPainterFactory>();

                context.Factory.Add<IUISystemInformation, HeadlessSystemInformation>();
                context.Factory.Add<IShapeFactory, ShapeFactory>();
                context.Factory.Add<IVisualFactory, VisualFactory>();

                context.Factory.Add<ICursorHandler, HeadlessCursorHandlerBackend>();
                context.Factory.Add<IMessageBoxShow, HeadlessMessageBoxShow>();
                context.Factory.Add<IProgressHandler, HeadlessProgressHandler>();

            }

        }

        public virtual void ApplyResources (IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            ApplyHeadlessResources (context);

            new ViewContextRecourceLoader ().ApplyResources (context);

            RegisterBackends (context);
            RegisterDragDropFormats (context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
            if (VidgetToolkit.CurrentEngine == null) {
                var engine = new HeadlessVidgetToolkitEngineBackend();
                engine.Initialize();
                VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
            }
        }

        [TODO]
        public virtual void RegisterDragDropFormats (IApplicationContext context) {

            Registry.Factory.Add<IDragDropBackendHandler> (args => new DragDropBackendHandler (args[0] as IVidgetBackend));

            var man = new TransferDataManager ();
            man.RegisterSome ();
            man.TransferContentTypes.Add ("html format", ContentTypes.HTML);
            man.TransferContentTypes.Add ("rich text format", ContentTypes.RTF);

            // TODO: register the others
            //man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.UnicodeText, 0);

        }
    }
}