using Xwt.Drawing;
using Xwt.Engine;
using Xwt.Blind.Backend;

namespace Limaki.Drawing {

    public class BlindSystemFonts:ISystemFonts {

        public Font CaptionFont {
            get { return BlindEngine.Registry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData()); }
        }

        public Font DefaultFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font DialogFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font IconTitleFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font MenuFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font MessageBoxFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font SmallCaptionFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

        public Font StatusFont {
            get { return BlindEngine.Registry.CreateFrontend<Font>(new Xwt.Blind.Backend.FontData()); }
        }

    }
}