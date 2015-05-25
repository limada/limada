using Xwt.Drawing;
using Xwt.Headless.Backend;
using Xwt;
using Xwt.Backends;

namespace Limaki.Drawing {

    public class HeadlessSystemFontsBackend : SystemFontBackend {

        public override Font CaptionFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font DefaultFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font DialogFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font IconTitleFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font MenuFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font MessageBoxFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font SmallCaptionFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }

        public override Font StatusFont { get { return new FontData { Family = "serif", Size = 10 }.ToXwt (); } }


    }
}