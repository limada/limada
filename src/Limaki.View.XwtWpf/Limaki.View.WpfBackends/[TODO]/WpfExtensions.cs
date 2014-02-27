using System;
using System.Windows;
using System.Windows.Threading;

namespace Limaki.View.WpfBackends {
    public static class WpfExtensions {

        public static void DoEvents() {
            Application.Current.Dispatcher.Invoke (DispatcherPriority.Background,
                new Action (delegate { }));
        }
    }
}