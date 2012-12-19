using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Gdi.Painters {

    public abstract class StringPainterBase : GdiPainter<string>,IDataPainter<string>, IDataPainter {

        protected System.Drawing.Pen _pen = null;
        protected virtual System.Drawing.Pen GetPen (System.Drawing.Color color) {
            if (_pen == null) {
                _pen = new System.Drawing.Pen (color);
                _pen.Width = 0.1f;
            }
            if (_pen.Color != color) {
                _pen.Color = color;
            }
            return _pen;
        }

        #region IPainter Member

        
        protected IShape _shape;
        public virtual IShape Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        public override RenderType RenderType {
            get { return RenderType.Fill; }
            set { }
        }

        protected string _text;
        public virtual string Text {
            get { return _text; }
            set { _text = value; }
        }


        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils> ();
                }
                return _drawingUtils;
            }
        }

        public override Point[] Measure (Matrix matrix, int delta, bool extend) {
            var shape = this.Shape;
            if (this.Text != null && shape != null) {
                var style = this.Style;
                var font = style.Font;
                if (AlignText && shape is IVectorShape) {
                    var vector = ((IVectorShape) shape).Data;
                    var vLen = Vector.Length (vector);
                    var fontSize = font.Size + 2;
                    var size = new Size (vLen, fontSize);
                    size = drawingUtils.GetTextDimension (this.Text, style);
                    if (size.Width == 0) {
                        size.Width = vLen;
                        size.Height = fontSize;
                    }
                    var result = vector.Hull (-(vLen - size.Width) / 2, size.Height / 2);
                    if (matrix != null)
                        matrix.Transform (result);
                    return result;

                } else {
                    if (matrix != null)
                        return shape.Hull (matrix, delta, extend);
                    else
                        return shape.Hull (delta, extend);
                }
            } else
                return null;
        }
        #endregion

        private bool _alignText = true;
        /// <summary>
        /// Text is aligned along a vector-shape if Shape is IVectorShape
        /// </summary>
        public bool AlignText {
            get { return _alignText; }
            set { _alignText = value; }
        }

        public string Data {
            get { return Text; }
            set { Text = value; }
        }


        object IDataPainter.Data {
            get { return this.Data; }
            set {
                if (value is string)
                    this.Data = (string) value;
                else
                    this.Data = value.ToString ();
            }
        }

    }
}