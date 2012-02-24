using System;

using Limaki.Drawing;
using Xwt.Drawing;
using Xwt;

using SW = System.Windows;
using SWM = System.Windows.Media;
using SWC = System.Windows.Controls;

namespace Limaki.Drawing.WPF {
    public class WPFUtils {
        public static System.Windows.Media.Color Convert(Color color) {
            return Xwt.WPFBackend.DataConverter.ToWpfColor(color);
        }

        static SWC.TextBlock stringContext = new SWC.TextBlock();
        public static Size GetTextDimension(string text, IStyle style) {
            SetTextStyle (stringContext, style);
            stringContext.Text = text;
#if ! SILVERLIGHT
            stringContext.Measure (new System.Windows.Size (style.AutoSize.Width, style.AutoSize.Height));
            System.Windows.Size size = stringContext.DesiredSize;
            return new Size((int)size.Width, (int)size.Height);
#else
            // TODO: look at StringPainter; set maxheight, maxwith
            // take aktualSize, aktualWidth then
            var size =
                new SizeD (
                    Math.Min((int) stringContext.ActualWidth,style.AutoSize.Width),
                    Math.Min((int)stringContext.ActualHeight, style.AutoSize.Height));
            return size;
#endif
        }

        public const double PixelToPoint = 1.5;
        public static void SetTextStyle(SWC.TextBlock textBlock, IStyle style) {
            textBlock.FontFamily = new SWM.FontFamily(style.Font.Family);
            textBlock.FontSize = style.Font.Size * PixelToPoint;
#if ! SILVERLIGHT
            textBlock.TextTrimming = System.Windows.TextTrimming.CharacterEllipsis;
#endif
            textBlock.TextAlignment = System.Windows.TextAlignment.Center;
            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textBlock.TextWrapping = System.Windows.TextWrapping.Wrap;
            if ((style.Font.Style & FontStyle.Italic) !=0) {
                textBlock.FontStyle = System.Windows.FontStyles.Italic;    
            }
            if ((style.Font.Weight & FontWeight.Bold) != 0) {
                textBlock.FontWeight = System.Windows.FontWeights.Bold;
            }
#if ! SILVERLIGHT
            textBlock.TextDecorations.Clear ();
#else
            textBlock.TextDecorations = null;
#endif
            if ((style.TextDecoration & TextDecoration.Underline) != 0) {
#if ! SILVERLIGHT
                textBlock.TextDecorations.Add(System.Windows.TextDecorations.Underline);
#else
                textBlock.TextDecorations = System.Windows.TextDecorations.Underline;
#endif
            } 
            //textBlock.FontStretch = style.Font.Stretch;
            //textBlock.FontWeight = style.Font.Weight;
            textBlock.Foreground = new SWM.SolidColorBrush(Convert(style.TextColor));
        }

        public static void AddPoints(SWM.PointCollection points, Point[] toolkit) {
            foreach(var point in toolkit) {
                points.Add (new System.Windows.Point (point.X, point.Y));
            }
        }
    }
}