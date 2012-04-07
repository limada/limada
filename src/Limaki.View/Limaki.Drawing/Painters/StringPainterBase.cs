using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.Drawing.Painters {
    /// <summary>
    /// A painter that draws text
    /// </summary>
    public abstract class StringPainterBase : IDataPainter<string>,IDataPainter {

        #region IPainter Member

        public RenderType RenderType {
            get { return RenderType.Fill; }
            set { }
        }

        protected IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        protected IShape _shape;
        public virtual IShape Shape {
            get { return _shape; }
            set { _shape = value; }
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
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
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


        public virtual void Dispose(bool disposing) { }
        public virtual void Dispose() {
            Dispose(true);
        }
        
        public string Data {
            get { return Text; }
            set {Text = value; }
        }

        public virtual Point[] Measure(int delta, bool extend) {
            return Measure (null, delta, extend);
        }

        public virtual Point[] Measure(Matrice matrix, int delta, bool extend) {
            var shape = this.Shape;
            if (this.Text != null && shape != null) {
                var style = this.Style;
                var font = style.Font;
                if (AlignText && shape is IVectorShape) {
                    var vector = ((IVectorShape)shape).Data;
                    var vLen = Vector.Length(vector);
                    var fontSize = font.Size + 2;
                    var size = new Size(vLen, fontSize);
                    size = drawingUtils.GetTextDimension(this.Text, style);
                    if (size.Width == 0) {
                        size.Width = vLen;
                        size.Height = fontSize;
                    }
                    var result = vector.Hull(-(vLen - size.Width) / 2, size.Height / 2);
                    if (matrix != null)
                        matrix.TransformPoints(result);
                    return result;

                } else {
                    if (matrix != null)
                        return shape.Hull(matrix, delta, extend);
                    else
                        return shape.Hull(delta, extend);
                }
            } else
                return null;
        }

        public abstract void Render(ISurface surface);

       

        IShape<string> IPainter<string>.Shape {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

       
        object IDataPainter.Data {
            get { return this.Data; }
            set {
                if (value is string)
                    this.Data = (string)value;
                else
                    this.Data = value.ToString();
            }
        }
       
    }
}