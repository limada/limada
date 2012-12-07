﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt.Engine;
using Xwt.Drawing;
using Xwt.Html5.Backend;

namespace Limaki.View.Html5 {

    public class Html5SystemFonts : ISystemFonts {

        public Font CaptionFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font DefaultFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font DialogFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font IconTitleFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font MenuFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font MessageBoxFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font SmallCaptionFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

        public Font StatusFont {
            get { return Html5Engine.Registry.CreateFrontend<Font> (new Xwt.Html5.Backend.FontData ()); }
        }

     
    }

}