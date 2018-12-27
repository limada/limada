
using Xwt.Drawing;
namespace Limaki.View.Headless.VidgetBackends {
    public static class ImageMissingStuffExtension {
        public static ImageFormat ImageFormatDUMMY(this BitmapImage bitmap) {
            return Xwt.Drawing.ImageFormat.ARGB32;
        }
    }
}