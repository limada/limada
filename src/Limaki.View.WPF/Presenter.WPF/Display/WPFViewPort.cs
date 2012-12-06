using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Xwt;

namespace Limaki.View.WPF.Display {
    [TODO]
    public class WpfViewport<T>:Viewport {
        
        public WpfViewport(WPFDisplay<T> device) {
            this.Device = device;
        }
        public virtual WPFDisplay<T> Device { get; set; }

        private Point _scrollPosition = Point.Zero;
        public override Point ClipOrigin {
            get {
               
                return _scrollPosition;
            }
            set {
                if (_scrollPosition != value) {
                    _scrollPosition = value;
                    Device.ScrollPosition =
                        _scrollPosition;
                }

            }
        }
        public override Size ClipSize {
            get { return Device.Size; }
        }

        private bool _scrollMinSizeChanging = false;
        public override Size DataSize {
            get {
                    return Device.ScrollMinSize;

            }
            set {
                _scrollMinSizeChanging = true;
                Device.ScrollMinSize = value;
                _scrollMinSize = value;
                _scrollMinSizeChanging = false;
            }
        }

        public override Matrice GetMatrix() {
            var zoomFactor = this.ZoomFactor;
            var scrollPosition = this.ClipOrigin;
            var offset = this.DataOrigin;
            var matrice = CreateMatrix();
            matrice.Scale(zoomFactor, zoomFactor);

            //matrice.Translate(
            //    (-offset.X - scrollPosition.X) / zoomFactor,
            //    (-offset.Y - scrollPosition.Y) / zoomFactor);
            matrice.Translate(
                (-offset.X ) / zoomFactor,
                (-offset.Y ) / zoomFactor);
            //transformSandBox(matrice);

            return matrice;
        }
    }
}