using System.Drawing;
using System.IO;
using Limaki.Iconerias;
using Xwt.GdiBackend;

namespace Limaki.View.Properties {

    public class GdiIconery {

        static GdiIconery () {
            
            OK = Iconery.OK.ToGdi();
            Cancel = Iconery.Cancel.ToGdi();
            Select = Iconery.Select.ToGdi();
            FullLayout = Iconery.FullLayout.ToGdi();
            Zoom = Iconery.Zoom.ToGdi();
            Panning = Iconery.Panning.ToGdi();


            LimadaIcon = IconFromByte(global::Limaki.View.Resources.Resource.LogoDrop32Icon);
            LimadaLogo = ImageFromByte(global::Limaki.View.Resources.Resource.LogoDrop32);
            LimadaSubWin = ImageFromByte(global::Limaki.View.Resources.Resource.LogoDrop32);
            LimadaSubWinIcon = IconFromByte(global::Limaki.View.Resources.Resource.LogoDrop32Icon);

        }

        static Image ImageFromByte(byte[] source) {
            using (var s = new MemoryStream(source))
            {
                return Image.FromStream(s);
            }
        }
        static Icon IconFromByte(byte[] source)
        {
            using (var s = new MemoryStream(source))
            {
                return new Icon(s);
            }
        }
        public static Image OK { get; set; }
        public static Image Cancel { get; set; }
        public static Image Select { get; set; }
        public static Image FullLayout { get; set; }
        public static Image Zoom { get; set; }
        public static Image Panning { get; set; }

        public static Image AddVisual { get; set; }

        public static Icon LimadaIcon { get; set; }
        public static Image LimadaLogo { get; set; }

        public static Image LimadaSubWin { get; set; }
        public static Icon LimadaSubWinIcon { get; set; }
    }
}