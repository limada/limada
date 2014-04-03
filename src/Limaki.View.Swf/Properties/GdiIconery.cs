using System.Drawing;
using Limaki.Iconerias;
using Xwt.Gdi.Backend;

namespace Limaki.View.Properties {

    public class GdiIconery {

        static GdiIconery () {
            
            OK = Iconery.OK.ToGdi();
            Cancel = Iconery.Cancel.ToGdi();
            Select = Iconery.Select.ToGdi();
            FullLayout = Iconery.FullLayout.ToGdi();
            Zoom = Iconery.Zoom.ToGdi();
            Panning = Iconery.Panning.ToGdi();


            LimadaIcon = global::Limaki.View.Properties.Resources.LogoDropIcon;
            LimadaLogo = global::Limaki.View.Properties.Resources.LogoDrop32;
            LimadaSubWin = global::Limaki.View.Properties.Resources.SubWin;
            LimadaSubWinIcon = global::Limaki.View.Properties.Resources.SubWinIcon;

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

        public static Bitmap LimadaSubWin { get; set; }
        public static Icon LimadaSubWinIcon { get; set; }
    }
}