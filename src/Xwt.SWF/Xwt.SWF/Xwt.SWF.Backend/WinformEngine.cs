using Xwt.Engine;
using SWF = System.Windows.Forms;
using Xwt.GDIBackend;

namespace Xwt.WinformBackend {

    public class SWFEngine : GdiEngine {
        public override void InitializeApplication() {

            SWF.Application.EnableVisualStyles();
            SWF.Application.SetCompatibleTextRenderingDefault(false);
            RegisterBackends();
            
        }

        public override void RegisterBackends() {
            base.RegisterBackends();
        }

        public override void RunApplication() {
            SWF.Application.Run();
        }

        public override void Invoke(System.Action action) {
           
            SWF.Form.ActiveForm.Invoke(action);
        }

        public override object TimeoutInvoke(System.Func<bool> action, System.TimeSpan timeSpan) {
            throw new System.NotImplementedException();
        }

        public override void CancelTimeoutInvoke(object id) {
            throw new System.NotImplementedException();
        }

        public override object GetNativeWidget(Widget w) {
            var backend = (IWinformWidgetBackend)WidgetRegistry.GetBackend(w);
            return backend.Control;
        }

        public override Backends.IWindowFrameBackend GetBackendForWindow(object nativeWindow) {
            throw new System.NotImplementedException();
        }
    }

    public interface IWinformWidgetBackend {
        SWF.Control Control { get; }
    }
}