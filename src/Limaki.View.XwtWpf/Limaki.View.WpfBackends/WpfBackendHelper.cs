using System;
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

        // example code for listening global clipboard changes
        public static void ListenClipboard () {
            //System.Windows.Input.ApplicationCommands.Paste.CanExecuteChanged +=
            //   Paste_CanExecuteChanged;
            // not working, fires only inapp and too often
            System.Windows.Input.ApplicationCommands.Copy.CanExecuteChanged +=
                Paste_CanExecuteChanged;
        }

        public static IDataObject clipo = null;
        public static void Paste_CanExecuteChanged (object sender, EventArgs e) {
            if (clipo==null || !Clipboard.IsCurrent (clipo)) {
                clipo = Clipboard.GetDataObject();
            }
        }
    }
}