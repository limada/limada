using Xwt.Drawing;
using Xwt.Blind.Backend;
using Xwt;
using Xwt.Backends;

namespace Limaki.Drawing {

    public class BlindSystemFonts:ISystemFonts {
        public Font CaptionFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font DefaultFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font DialogFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font IconTitleFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font MenuFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font MessageBoxFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font SmallCaptionFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

        public Font StatusFont {
            get { return new Xwt.Blind.Backend.FontData().ToXwt(); }
        }

    }
}