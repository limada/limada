using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt.Drawing;
using Xwt.Html5.Backend;

namespace Limaki.View.Html5 {

    public class Html5SystemFonts : ISystemFonts {

        public Font CaptionFont { get { return new Xwt.Html5.Backend.FontData { Family = "Serif", Size = 10 }.ToXwt(); } }

        public Font DefaultFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font DialogFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font IconTitleFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font MenuFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font MessageBoxFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font SmallCaptionFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

        public Font StatusFont { get { return new Xwt.Html5.Backend.FontData {Family = "Serif", Size = 10}.ToXwt(); } }

    }

}
