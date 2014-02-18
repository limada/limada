using Xwt.Drawing;
using Xwt.Headless.Backend;
using Xwt;
using Xwt.Backends;

namespace Limaki.Drawing {

    public class HeadlessSystemFonts:ISystemFonts {
        public Font CaptionFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font DefaultFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font DialogFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font IconTitleFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font MenuFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font MessageBoxFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font SmallCaptionFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

        public Font StatusFont {
            get { return new Xwt.Headless.Backend.FontData().ToXwt(); }
        }

    }
}