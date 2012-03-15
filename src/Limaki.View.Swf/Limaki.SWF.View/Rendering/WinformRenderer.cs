using System.Drawing;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing.GDI;
using Limaki.View.Rendering;
using Xwt.Gdi;

namespace Limaki.View.Winform.Display {
    public class WinformRenderer<T>:IDeviceRenderer {
        
        public virtual WinformDisplay<T> Device { get; set; }
        
        public virtual IDisplay<T> Display { get; set; }
        
        public WinformRenderer() { }

        public void Render() {
            Device.Invalidate ();
        }

        public void Render(IClipper clipper) {
            if (clipper.RenderAll) {
                Device.Invalidate();    
            } else {
                Device.Invalidate (GDIConverter.Convert (clipper.Bounds));
            }
        }

        public bool Opaque { get; set; }
        protected SolidBrush _backBrush = new SolidBrush(SystemColors.ButtonFace);
        protected SolidBrush backBrush {
            get {
                _backBrush.Color = GdiConverter.ToGdi(BackColor());
                return _backBrush;
            }
        }

        
        public Get<global::Xwt.Drawing.Color> BackColor {get;set;}

        protected object lockRender = new object();
        
        public virtual void OnPaint(IRenderEventArgs e) {
            this.OnPaint (Converter.Convert (e));
        }

        public virtual void OnPaint(PaintEventArgs e) {
            var display = this.Display;
            var data = display.Data;

            if (data != null) {
                Graphics g = e.Graphics;
                Region saveRegion = g.Clip;
                Rectangle clipRect = e.ClipRectangle;

                lock (display.Clipper) {
                    // draw background
                    if (Opaque) {
                        g.FillRectangle(backBrush, clipRect);
                    }
#if TraceInvalidate
                    System.Console.Out.WriteLine("Paint  cliprect {0}", clipRect);
#endif
                    display.EventControler.OnPaint(Converter.Convert(e));


                    display.Clipper.Clear();
                }
                g.Clip = saveRegion;
                g.Transform.Reset();
            } else {
                if (Opaque) { // draw background
                    e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
                }
            }
        }


    }
}