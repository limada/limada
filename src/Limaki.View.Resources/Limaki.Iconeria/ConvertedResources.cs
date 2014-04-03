using System.IO;
using SDI = System.Drawing.Imaging;
using XD = Xwt.Drawing;

namespace Limaki.Iconerias {

    /// <summary>
    /// <remarks>
    /// hide this class; System.Drawing should not be visible outside
    /// </remarks>
    /// </summary>
    internal class ConvertedResources {

        internal static XD.Image LimadaLogo = AsImage (global::Limaki.View.Properties.Resources.LogoDrop32);
        internal static XD.Image SubWinIcon = AsImage (global::Limaki.View.Properties.Resources.SubWin);

        internal static XD.Image AsImage (System.Drawing.Image source) {
            Xwt.Drawing.Image result = null;
            using (var stream = new MemoryStream ()) {
                source.Save (stream, SDI.ImageFormat.Png);
                stream.Position = 0;
                result = Xwt.Drawing.Image.FromStream (stream);
            }
            return result;
        }
    }
}