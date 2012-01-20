using System;
using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Drawing;


namespace Limaki.Drawing.WPF {
    public class WPFUtils {
        public static System.Windows.Media.Color Convert(Limaki.Drawing.Color color) {
            return System.Windows.Media.Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
        }

        static TextBlock stringContext = new TextBlock();
        public static SizeI GetTextDimension(string text, IStyle style) {
            SetTextStyle (stringContext, style);
            stringContext.Text = text;
#if ! SILVERLIGHT
            stringContext.Measure (new System.Windows.Size (style.AutoSize.Width, style.AutoSize.Height));
            System.Windows.Size size = stringContext.DesiredSize;
            return new SizeI((int)size.Width, (int)size.Height);
#else
            // TODO: look at StringPainter; set maxheight, maxwith
            // take aktualSize, aktualWidth then
            var size =
                new SizeI (
                    Math.Min((int) stringContext.ActualWidth,style.AutoSize.Width),
                    Math.Min((int)stringContext.ActualHeight, style.AutoSize.Height));
            return size;
#endif
        }

        public const double PixelToPoint = 1.5;
        public static void SetTextStyle(TextBlock textBlock, IStyle style) {
            textBlock.FontFamily = new FontFamily(style.Font.FontFamily);
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
            if ((style.Font.Style & FontStyle.Bold) != 0) {
                textBlock.FontWeight = System.Windows.FontWeights.Bold;
            }
#if ! SILVERLIGHT
            textBlock.TextDecorations.Clear ();
#else
            textBlock.TextDecorations = null;
#endif
            if ((style.Font.Style & FontStyle.Underline) != 0) {
#if ! SILVERLIGHT
                textBlock.TextDecorations.Add(System.Windows.TextDecorations.Underline);
#else
                textBlock.TextDecorations = System.Windows.TextDecorations.Underline;
#endif
            } 
            //textBlock.FontStretch = style.Font.Stretch;
            //textBlock.FontWeight = style.Font.Weight;
            textBlock.Foreground = new SolidColorBrush(Convert(style.TextColor));
        }

        public static void AddPoints(PointCollection points, PointI[] toolkit) {
            foreach(var point in toolkit) {
                points.Add (new System.Windows.Point (point.X, point.Y));
            }
        }
    }
}