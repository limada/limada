using System;
using Xwt;
using SystemColors = System.Windows.SystemColors;
using Xwt.Drawing;

namespace Limaki.Drawing.WPF {

    public class WPFDrawingUtils : IDrawingUtils {

        public virtual Size GetTextDimension(string text, IStyle style) {
            return WPFUtils.GetTextDimension(text,style);
        }

        uint ToArgb(System.Windows.Media.Color color) {
            return (uint) ( color.A << 24 | color.R << 16 | color.G << 8 | color.B );
        }

        public uint GetSysColor(SysColorIndex index) {
#if ! SILVERLIGHT
            if (index == SysColorIndex.COLOR_SCROLLBAR) {
                return ToArgb(SystemColors.ScrollBarColor);
            }
            if (index == SysColorIndex.COLOR_BACKGROUND) {
                return ToArgb(SystemColors.WindowTextColor);
            }
            if (index == SysColorIndex.COLOR_ACTIVECAPTION) {
                return ToArgb(SystemColors.ActiveCaptionColor);
            }
            if (index == SysColorIndex.COLOR_INACTIVECAPTION) {
                return ToArgb(SystemColors.InactiveCaptionColor);
            }
            if (index == SysColorIndex.COLOR_MENU) {
                return ToArgb(SystemColors.MenuColor);
            }
            if (index == SysColorIndex.COLOR_WINDOW) {
                return ToArgb(SystemColors.WindowColor);
            }
            if (index == SysColorIndex.COLOR_WINDOWFRAME) {
                return ToArgb(SystemColors.WindowFrameColor);
            }
            ;
            if (index == SysColorIndex.COLOR_MENUTEXT) {
                return ToArgb(SystemColors.MenuTextColor);
            }
            if (index == SysColorIndex.COLOR_WINDOWTEXT) {
                return ToArgb(SystemColors.WindowTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_CAPTIONTEXT) {
                return ToArgb(SystemColors.ActiveCaptionTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_ACTIVEBORDER) {
                return ToArgb(SystemColors.ActiveBorderColor);
            }
            ;
            if (index == SysColorIndex.COLOR_INACTIVEBORDER) {
                return ToArgb(SystemColors.InactiveBorderColor);
            }
            ;
            if (index == SysColorIndex.COLOR_APPWORKSPACE) {
                return ToArgb(SystemColors.WindowColor);
            }
            ;
            if (index == SysColorIndex.COLOR_HIGHLIGHT) {
                return ToArgb(SystemColors.HighlightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_HIGHLIGHTTEXT) {
                return ToArgb(SystemColors.HighlightTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_BTNFACE) {
                return ToArgb(SystemColors.ControlColor);
            }
            ;
            if (index == SysColorIndex.COLOR_BTNSHADOW) {
                return ToArgb(SystemColors.ControlDarkColor);
            }
            ;
            if (index == SysColorIndex.COLOR_GRAYTEXT) {
                return ToArgb(SystemColors.GrayTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_BTNTEXT) {
                return ToArgb(SystemColors.ControlTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_INACTIVECAPTIONTEXT) {
                return ToArgb(SystemColors.InactiveCaptionTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_BTNHIGHLIGHT) {
                return ToArgb(SystemColors.ControlLightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DDKSHADOW) {
                return ToArgb(SystemColors.ControlDarkColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DLIGHT) {
                return ToArgb(SystemColors.ControlLightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_INFOTEXT) {
                return ToArgb(SystemColors.InfoTextColor);
            }
            ;
            if (index == SysColorIndex.COLOR_INFOBK) {
                return ToArgb(SystemColors.InfoColor);
            }
            ;

            if (index == SysColorIndex.COLOR_HOTLIGHT) {
                return ToArgb(SystemColors.HotTrackColor);
            }
            ;
            if (index == SysColorIndex.COLOR_GRADIENTACTIVECAPTION) {
                return ToArgb(SystemColors.GradientActiveCaptionColor);
            }
            ;
            if (index == SysColorIndex.COLOR_GRADIENTINACTIVECAPTION) {
                return ToArgb(SystemColors.GradientInactiveCaptionColor);
            }
            ;
            if (index == SysColorIndex.COLOR_MENUHIGHLIGHT) {
                return ToArgb(SystemColors.MenuHighlightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_MENUBAR) {
                return ToArgb(SystemColors.MenuBarColor);
            }
            ;

            if (index == SysColorIndex.COLOR_DESKTOP) {
                return ToArgb(SystemColors.DesktopColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DFACE) {
                return ToArgb(SystemColors.ControlColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DSHADOW) {
                return ToArgb(SystemColors.ControlDarkColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DHIGHLIGHT) {
                return ToArgb(SystemColors.ControlLightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_3DHILIGHT) {
                return ToArgb(SystemColors.ControlLightColor);
            }
            ;
            if (index == SysColorIndex.COLOR_BTNHILIGHT) {
                return ToArgb(SystemColors.ControlLightColor);
            }
            ;
#else
            return ToArgb(System.Windows.Media.Colors.Black);
#endif
            return 0;
        }

        public virtual Font CreateFont(string familiy, double size) {
            var result = Font.FromName(familiy,size);
            return result;
        }

        public Pen CreatePen(Color color) {
            return new Pen(color);
        }

        public Matrice NativeMatrice() {
            return new Limaki.Drawing.Matrice();
        }

        public object GetCustomLineCap(double arrowWidth, double arrowHeigth) {

            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException("ArrowWidth must not be 0");

            var path = new System.Windows.Media.PolyLineSegment();

            var p1 = new System.Windows.Point(0, 1);
            var p2 = new System.Windows.Point(-arrowHeigth, -arrowWidth);
            var p3 = new System.Windows.Point(arrowHeigth, -arrowWidth);
            
            path.Points = new System.Windows.Media.PointCollection ();
            path.Points.Add (p1);
            path.Points.Add(p2);
            path.Points.Add(p3);
#if ! SILVERLIGHT
            path.IsSmoothJoin = true;
#endif
            return path;

        }
    }
}