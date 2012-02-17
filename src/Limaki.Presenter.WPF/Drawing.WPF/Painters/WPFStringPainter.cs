using System.Windows.Controls;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Shapes;
using Limaki.Drawing.Shapes;
using System.Windows.Media;

namespace Limaki.Drawing.WPF.Painters {
    public class WPFStringPainter : Limaki.Drawing.Painters.StringPainterBase, IPainter<string> {
        
        public System.Windows.UIElement DataElement { get; set; }
        
        public override void Render(ISurface surface) {
            var shape = this.Shape;
            var bounds =
                new RectangleI(shape.Location, shape.Size).NormalizedRectangle();
            
            var border = new System.Windows.Thickness (4, 4, 4, 1);

            var isVector = shape.ShapeDataType == typeof (Vector);
            double angle = 0;
            PointI middle = default( PointI );

            if (this.DataElement is TextBlock) {
                var textBlock = (TextBlock) DataElement;
                WPFUtils.SetTextStyle(textBlock, this.Style);

                if (isVector) {
                    Vector vector = ((Limaki.Drawing.Shapes.VectorShape)shape).Data;
                    angle = Vector.Angle (vector);
                    var vLen = (int)(Vector.Length(vector));
                    bounds.Height = (int)(
                        (this.Style.Font.Size*WPFUtils.PixelToPoint)+
                        border.Top+border.Bottom);
                    bounds.Width = vLen;

                }

                textBlock.MaxHeight = bounds.Height;
                textBlock.MinHeight = bounds.Height;

                textBlock.MaxWidth = bounds.Width;
                textBlock.MinWidth = bounds.Width;

                textBlock.Height = bounds.Height;
                textBlock.Width = bounds.Width;

                // TODO Silverlight:
                // if actualHeigth > Shape.Height
                // take a look if textBlock has inlines and delete then

                // TODO: move this to WPFUtils.GetTextDimension
                textBlock.Padding = border;

                if (isVector) {
                    middle = shape[Anchor.Center];
                    bounds.X = middle.X - (int)((textBlock.Width / 2)+border.Left);
                    bounds.Y = middle.Y - (int)((textBlock.Height / 2)+border.Top+border.Bottom);
                }
               
            }

            if (this.DataElement != null) {
                
                //if (isVector && angle !=0) {
                //    var rotate = new RotateTransform ();
                //    rotate.Angle = angle;
                //    rotate.CenterX = middle.X;
                //    rotate.CenterY = middle.Y;
                //    // DataElement already has a transform!
                //    var group = new TransformGroup ();
                //    group.Children.Add (this.DataElement.RenderTransform);
                //    group.Children.Add (rotate);
                //    this.DataElement.RenderTransform = group;

                //}

                Canvas.SetLeft(this.DataElement, bounds.X);
                Canvas.SetTop(this.DataElement, bounds.Y);

                Canvas.SetZIndex(this.DataElement, 10);
            }
        }

        public override PointI[] Measure(Matrice matrix, int delta, bool extend) {
            IShape shape = this.Shape;
            if (this.Text != null && shape != null) {
                IStyle style = this.Style;
                var font = style.Font;
                if (AlignText && shape.ShapeDataType == typeof(Vector)) {
                    Vector vector = ((Limaki.Drawing.Shapes.VectorShape)shape).Data;
                    float vLen = (float)Vector.Length(vector);
                    float fontSize = (float)font.Size + 2;
                    var size = new SizeS(vLen, fontSize);
                    size = WPFUtils.GetTextDimension(this.Text, style);
                    if (size.Width == 0) {
                        size.Width = vLen;
                        size.Height = fontSize;
                    }
                    var result = vector.Hull(-(vLen - size.Width) / 2, size.Height / 2);
                    matrix.TransformPoints(result);
                    return result;

                } else {
                    return shape.Hull(matrix, delta, extend);
                }
            } else
                return null; 
        }
    }
}