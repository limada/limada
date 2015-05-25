using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt.Drawing;
using Xwt.Html5.Backend;
using Xwt.Backends;

namespace Limaki.View.Html5 {

    public class Html5SystemFontBackend : SystemFontBackend {

        public override Font CaptionFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt(); } }

        public override Font DefaultFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font DialogFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font IconTitleFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font MenuFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font MessageBoxFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font SmallCaptionFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font StatusFont { get { return new Xwt.Html5.Backend.FontData { Family = "serif", Size = 10 }.ToXwt (); } }

    }

}
