using System;
using Limaki.Common;
using Limaki.View.UI;
using Limaki.Drawing.WPF;
using System.Windows.Threading;
using Limaki.View.Clipping;
using Limaki.View.Rendering;
using Limaki.View.WPF.UI;
using Xwt.Drawing;

namespace Limaki.View.WPF.Display {
    public class WPFRenderer<T> : IBackendRenderer {
        public virtual WPFDisplay<T> Device { get; set; }
        public virtual IDisplay<T> Display { get; set; }

        public void Render() {
            Render(new BoundsClipper(Device.ClientRectangle));
        }

        public void Render(IClipper clipper) {
            OnPaint (new WPFRenderEventArgs (Device.Surface, clipper));
        }

        public Func<Color> BackColor {get;set;}


        public void OnPaint(IRenderEventArgs e) {
            var display = this.Display;
            var data = display.Data;
            var device = this.Device;

            if (data != null) {
#if ! SILVERLIGHT
                //device.LayoutRoot.BeginInit();
#endif

                var args = e as WPFRenderEventArgs;
                if (args == null) {
                    new WPFRenderEventArgs (device.Surface, e.Clipper);
                }

                lock (display.Clipper) {
                    Action paint = () => display.EventControler.OnPaint(args);
#if ! SILVERLIGHT
                    device.Dispatcher.Invoke(DispatcherPriority.Render, paint);
#else                   
                    device.Dispatcher.BeginInvoke(paint));
#endif
                    display.Clipper.Clear();
                }

#if ! SILVERLIGHT
                //device.LayoutRoot.EndInit();

#endif
            }
        }

    }
}