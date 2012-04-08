using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.View.Rendering;
using Limaki.View.Swf;
using Xwt.Gdi.Backend;

namespace Limaki.View.Viewers.Swf {

    public class GraphScenePainterRenderer<T> : IBackendRenderer {
        public ILayer<T> Layer { get; set; }

        public void Render() {
            throw new NotImplementedException();
        }

        public void Render(IClipper clipper) {
            throw new NotImplementedException();
        }

        public Get<global::Xwt.Drawing.Color> BackColor { get; set; }

        public void OnPaint(IRenderEventArgs e) {
            this.OnPaint(Converter.Convert(e));
        }

        public virtual void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;

            var b = new SolidBrush(GdiConverter.ToGdi(BackColor()));
            g.FillRectangle(b, e.ClipRectangle);

            Layer.OnPaint(Converter.Convert(e));

        }
    }
}