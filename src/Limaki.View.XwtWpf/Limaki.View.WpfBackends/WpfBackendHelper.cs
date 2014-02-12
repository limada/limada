using System.Windows;

namespace Limaki.View.WpfBackends {

    public static class WpfBackendHelper {

        public static Xwt.Size VidgetBackendSize (this FrameworkElement backend) {
            return new Xwt.Size(backend.ActualWidth, backend.ActualHeight);
        }

        public static void VidgetBackendUpdate (this FrameworkElement widget) {
           
        }

        public static void VidgetBackendInvalidate (this FrameworkElement widget) {
            
        }

        public static void VidgetBackendInvalidate (this FrameworkElement widget, Xwt.Rectangle rect) {
           
        }
    }
}