using System;
using Xwt;
using SystemColors = System.Windows.SystemColors;
using Xwt.Drawing;
using System.Drawing.Drawing2D;
using Xwt.WPFBackend;
using System.Globalization;

namespace Limaki.Drawing.WPF {

    public class WPFDrawingUtils : IDrawingUtils {

        public virtual Size GetTextDimension(string text, IStyle style) {
            return WPFUtils.GetTextDimension(text,style);
        }

        uint ToArgb(System.Windows.Media.Color color) {
            return (uint) ( color.A << 24 | color.R << 16 | color.G << 8 | color.B );
        }

       

        public virtual Font CreateFont(string familiy, double size) {
            var result = Font.FromName(familiy+" "+size.ToString(CultureInfo.InvariantCulture));
            return result;
        }

        public Pen CreatePen(Color color) {
            return new Pen(color);
        }

        public object GetCustomLineCap(double arrowWidth, double arrowHeigth) {

            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException("ArrowWidth must not be 0");

            var path = new System.Windows.Media.PolyLineSegment();

            var p1 = new System.Windows.Point(0, 1);
            var p2 = new System.Windows.Point(-arrowHeigth, -arrowWidth);
            var p3 = new System.Windows.Point(arrowHeigth, -arrowWidth);
            
            path.Points = new System.Windows.Media.PointCollection ();
            path.Points.Add (p1);
            path.Points.Add(p2);
            path.Points.Add(p3);
#if ! SILVERLIGHT
            path.IsSmoothJoin = true;
#endif
            return path;

        }
#if ! SILVERLIGHT
        public void SetGraphicsMode (System.Drawing.Graphics graphics)
		{
			if (graphics == null)
				throw new ArgumentNullException ("graphics");
			
			var displayTier = (System.Windows.Media.RenderCapability.Tier >> 16);
			displayTier = 0; //debug code; remove that!
			if (displayTier == 0) {//no hardware acceleration
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.PixelOffsetMode = PixelOffsetMode.Half;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
			} else if (displayTier == 1) {//partial hardware acceleration
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.PixelOffsetMode = PixelOffsetMode.Half;
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				graphics.CompositingQuality = CompositingQuality.AssumeLinear;
			} else {//supports hardware acceleration
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
			}

		}

        public Size ScreenResolution() {
            throw new NotImplementedException();

            var MainWindow = System.Windows.Application.Current.MainWindow;
            var MainWindowPresentationSource = System.Windows.PresentationSource.FromVisual(MainWindow);
            var m = MainWindowPresentationSource.CompositionTarget.TransformToDevice;
            var thisDpiWidthFactor = m.M11;
            var thisDpiHeightFactor = m.M22;

            return new Size(thisDpiWidthFactor, thisDpiHeightFactor);
        }

        public Size Resolution(Context context) {
            return new Size(96,96);
        }
        #endif
    }
}