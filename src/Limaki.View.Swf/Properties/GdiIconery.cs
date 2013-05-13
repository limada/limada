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
                   
            StraightConnector = global::Limaki.View.Properties.Resources.StraightConnector;
            AddVisual = global::Limaki.View.Properties.Resources.CreateWidget24;

            LimadaLogo = global::Limaki.View.Properties.Resources.LimadaLogo;
            LimadaLogoL32 = global::Limaki.View.Properties.Resources.LimadaLogoL32;

        }

        public static Image OK { get; set; }
        public static Image Cancel { get; set; }
        public static Image Select { get; set; }
        public static Image FullLayout { get; set; }
        public static Image Zoom { get; set; }
        public static Image Panning { get; set; }

        public static Image AddVisual { get; set; }
        public static Image StraightConnector { get; set; }

        public static Icon LimadaLogo { get; set; }
        public static Image LimadaLogoL32 { get; set; }
      
    }
}