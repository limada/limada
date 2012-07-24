using Xwt.Drawing;
using Xwt.Engine;

namespace Limaki.Drawing {

    public class BlindSystemFonts:ISystemFonts {

        public Font CaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData()); }
        }

        public Font DefaultFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData()); }
        }

        public Font DialogFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

        public Font IconTitleFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

        public Font MenuFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

        public Font MessageBoxFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

        public Font SmallCaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

        public Font StatusFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Blind.Backend.FontData ()); }
        }

    }
}