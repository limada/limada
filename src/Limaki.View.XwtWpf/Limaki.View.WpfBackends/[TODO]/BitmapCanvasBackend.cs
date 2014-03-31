using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SWM = System.Windows.Media; 
using System.Windows.Media.Imaging;
using Xwt.Backends;
using Xwt.WPFBackend;

namespace Limaki.View.WpfBackend {

    /// <summary>
    /// TODO: faster wpf.canvasbackend based on WriteableBitmap
    /// </summary>
    public class BitmapCanvasBackend : WidgetBackend, ICanvasBackend {
        public void QueueDraw () {
            throw new System.NotImplementedException();
        }

        public void QueueDraw (Xwt.Rectangle rect) {
            throw new System.NotImplementedException();
        }

        public void AddChild (IWidgetBackend widget, Xwt.Rectangle bounds) {
            throw new System.NotImplementedException();
        }

        public void SetChildBounds (IWidgetBackend widget, Xwt.Rectangle bounds) {
            throw new System.NotImplementedException();
        }

        public void RemoveChild (IWidgetBackend widget) {
            throw new System.NotImplementedException();
        }
    }

    #region expample

    public class BitmapCanvasExample {
        public partial class ImgCanvas : UserControl {

            private Image image;
            private RenderTargetBitmap bitmap;
            public ImgCanvas () {
                image = new Image();
                this.AddChild(image);
                this.SizeChanged += this.OnSizeChanged;
            }
            
            void OnSizeChanged (object sender, SizeChangedEventArgs args) {
                if (args.NewSize.IsEmpty)
                    return;
                var dpiX = 96;
                var dpiY = 96;
                bitmap = new RenderTargetBitmap((int) args.NewSize.Width,
                                             (int) args.NewSize.Height, dpiX,dpiY,PixelFormats.Default);
                image.Source = bitmap;

            }

            protected override void OnRender (SWM.DrawingContext drawingContext) {
                var drawingVisual = new DrawingVisual();
                using (var dvctx = drawingVisual.RenderOpen()) {
                    //
                    // ... draw on the drawingContext
                    //
                   
                    bitmap.Render(drawingVisual);

                }
                base.OnRender(drawingContext);
            }

        }
    }
    #endregion
}