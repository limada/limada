using Limaki.Painting;
using Xwt.Drawing;
using Xwt.Gdi;
using Xwt.Gdi.Backend;
using Xwt.Engine;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Limaki.GDI.Painting {

    public class PaintContextBackendHandler : ContextBackendHandler, IPaintContextBackendHandler {

        public PaintContextBackendHandler () {
            AntiAlias = true;
        }

        public override object CreateContext (Xwt.Widget w) {
            var c = (GdiContext) base.CreateContext (w);
            InitBackend (c);
            return c;
        }


        public void InitBackend (object backend) {
            var c = (GdiContext) backend;
            var g = c.Graphics;
            if (AntiAlias) {
                // this is hiqh quality mode:
                g.SmoothingMode = SmoothingMode.HighQuality; //.AntiAlias;//.HighQuality;//.HighSpeed;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//.SystemDefault;//.AntiAliasGridFit;//.ClearTypeGridFit:this is slowest on mono;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // 
            } else {
                // this is speed - mode, best compromise, not highspeed:
                g.SmoothingMode = SmoothingMode.HighSpeed; // .none is fastest
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.InterpolationMode = InterpolationMode.Low;
            }
            //e.Graphics.TextContrast = 12; // 0..12

            g.CompositingMode = CompositingMode.SourceOver;
        }



        public bool AntiAlias { get; set; }

        public void DrawTextLayout (object backend, TextLayout layout, double x, double y, double height) {
            
            var context = (GdiContext) backend;
            var tl = (TextLayoutBackend) WidgetRegistry.GetBackend (layout);
            var font = tl.Font.ToGdi ();
            var rect = new System.Drawing.RectangleF ((float) x, (float) y, (float) layout.Width, (float) height);
            context.Transform();
            context.Graphics.DrawString (tl.Text, font, context.Brush, rect, tl.Format);
        
        }

        public void TextLayout (object backend, TextLayout layout, double x, double y, double height) {

            var context = (GdiContext) backend;
            var tl = (TextLayoutBackend) WidgetRegistry.GetBackend (layout);
            var font = tl.Font.ToGdi ();
            var rect = new System.Drawing.RectangleF ((float) x, (float) y, (float) layout.Width, (float) height);
            
            context.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            context.Path.AddString (tl.Text, font.FontFamily, (int) font.Style, (int)(font.Size*1.4), rect, tl.Format);

        }

        public void TranslatePath (object backend, double x, double y) {
            var context = (GdiContext) backend;
            context.TranslatePath ((float)x, (float)y);
        }

       
      
    }
}