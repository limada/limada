using Xwt.Backends;

namespace Xwt.GtkBackend {
    internal partial class GtkAlertDialog  {
        public void LmkGtkAlertDialog(ApplicationContext actx, MessageDescription message) {
            Title = message.Title ?? null;

        }
    }
}