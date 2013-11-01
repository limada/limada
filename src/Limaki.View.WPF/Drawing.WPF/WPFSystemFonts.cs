

using Xwt.Drawing;
using SystemFonts = System.Windows.SystemFonts;
using Xwt.WPFBackend;
using System.Globalization;

namespace Limaki.Drawing.WPF {
    public class WPFSystemFonts : ISystemFonts {
#if ! SILVERLIGHT
        public const double PixelToPoint = 1.5;
        Font CreateFont(System.Windows.Media.FontFamily family, double size) {
            return Font.FromName(family.Source+" "+(size/PixelToPoint).ToString(CultureInfo.InvariantCulture));
        }
        public Font CaptionFont {
            get { return CreateFont(SystemFonts.CaptionFontFamily,SystemFonts.CaptionFontSize); }
        }

        public Font DefaultFont {
            get { return CreateFont(SystemFonts.CaptionFontFamily,SystemFonts.CaptionFontSize); }
        }

        public Font DialogFont {
            get { return CreateFont(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize); }
        }

        public Font IconTitleFont {
            get { return CreateFont(SystemFonts.IconFontFamily, SystemFonts.IconFontSize); }
        }

        public Font MenuFont {
            get { return CreateFont(SystemFonts.MenuFontFamily, SystemFonts.MenuFontSize); }
        }

        public Font MessageBoxFont {
            get { return CreateFont(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize); }
        }

        public Font SmallCaptionFont {
            get { return CreateFont(SystemFonts.SmallCaptionFontFamily, SystemFonts.SmallCaptionFontSize); }
        }

        public Font StatusFont {
            get { return CreateFont(SystemFonts.StatusFontFamily, SystemFonts.StatusFontSize); }
        }

#else
        public Font CaptionFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font DefaultFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font DialogFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font IconTitleFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font MenuFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font MessageBoxFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font SmallCaptionFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font StatusFont {
            get { return new Font("Tahoma", 12); }
        }
#endif

    }
}