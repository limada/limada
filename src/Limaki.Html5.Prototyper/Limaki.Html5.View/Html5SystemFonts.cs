using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt.Engine;
using Xwt.Drawing;

namespace Limaki.View.Html5 {

    public class Html5SystemFonts : ISystemFonts {

        public Font CaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font DefaultFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font DialogFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font IconTitleFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font MenuFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font MessageBoxFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font SmallCaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font StatusFont {
            get { return WidgetRegistry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

     
    }

}
