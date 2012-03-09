using Limaki.Painting;
using Limaki.GDI.Painting;
using Xwt.Drawing;
using Xwt.Gdi.Backend;
namespace Limaki.Tests.Sandbox {
   
    public class PaintContextTestControl:System.Windows.Forms.UserControl {

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint(e);
            var pen = new System.Drawing.Pen(System.Drawing.Color.WhiteSmoke);
            pen.Width = 2;

            var graphics = new GdiContext {Graphics = e.Graphics};
            var painter = new PaintContext(graphics);
            
            painter.MoveTo(1, 1);
            painter.LineTo(30, 30);
            painter.Arc(20, 20,40, 90, 70);

            painter.SetColor(Colors.Wheat);
            painter.FillPreserve();

            painter.SetColor(Colors.Red);
            painter.SetLineWidth(2);
            painter.ClosePath();
            painter.Stroke();

            painter.SetColor(Colors.Blue);
            painter.MoveTo(50, 50);
            painter.Arc(70, 70, 40, 360, 180);
            painter.ClosePath();
            painter.Stroke();

            graphics.Dispose();

        }
        
        
    }

    
}