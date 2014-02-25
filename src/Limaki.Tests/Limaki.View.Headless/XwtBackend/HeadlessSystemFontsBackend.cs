using Xwt.Drawing;
using Xwt.Headless.Backend;
using Xwt;
using Xwt.Backends;

namespace Limaki.Drawing {

    public class HeadlessSystemFontsBackend : SystemFontBackend {
        public override Font CaptionFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font DefaultFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font DialogFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font IconTitleFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font MenuFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font MessageBoxFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font SmallCaptionFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }
                
        public override Font StatusFont { get { return new Xwt.Headless.Backend.FontData().ToXwt(); } }

    }
}