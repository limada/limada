/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing.GDI {
    public class GDIDrawingUtils : IDrawingUtils {
        public virtual Font CreateFont(string familiy, double size) {
            var result = new GDIFont();
            result.FontFamily = familiy;
            result.Size = size;
            return result;
        }

        public Pen CreatePen(Color color) {
            return new GDIPen(color);
        }

        public Limaki.Drawing.Matrice NativeMatrice() {
            return new GDIMatrice ();
        }

        public object GetCustomLineCap(float arrowWidth, float arrowHeigth) {
            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException ("ArrowWidth must not be 0");
            GraphicsPath path = new GraphicsPath();
            System.Drawing.PointF p1 = new System.Drawing.PointF(0, 1);
            System.Drawing.PointF p2 = new System.Drawing.PointF(-arrowHeigth, -arrowWidth);
            System.Drawing.PointF p3 = new System.Drawing.PointF(arrowHeigth, -arrowWidth);
            path.AddPolygon(new System.Drawing.PointF[3] { p1, p2, p3 });
            //path.AddLine(p1, p2);
            //path.AddLine(p2, p3);
            //path.AddLine(p3, p1);

            CustomLineCap result = new CustomLineCap(path, null);
            result.BaseInset = 1;
            //result.StrokeJoin = LineJoin.Round;

            return result;

        }

        public virtual SizeS GetTextDimension(string text, Limaki.Drawing.IStyle style) {
            return GDIUtils.GetTextDimension(
                ((GDIFont)style.Font).Native,
                text,
                GDIConverter.Convert(style.AutoSize));
        }


        public uint GetSysColor(SysColorIndex index) {
            if (index == SysColorIndex.COLOR_SCROLLBAR) {
                return (uint)SystemColors.ScrollBar.ToArgb();
            }
            if (index == SysColorIndex.COLOR_BACKGROUND) {
                return (uint)SystemColors.WindowText.ToArgb();
            }
            if (index == SysColorIndex.COLOR_ACTIVECAPTION) {
                return (uint)SystemColors.ActiveCaption.ToArgb();
            }
            if (index == SysColorIndex.COLOR_INACTIVECAPTION) {
                return (uint)SystemColors.InactiveCaption.ToArgb();
            }
            if (index == SysColorIndex.COLOR_MENU) {
                return (uint)SystemColors.Menu.ToArgb();
            }
            if (index == SysColorIndex.COLOR_WINDOW) {
                return (uint)SystemColors.Window.ToArgb();
            }
            if (index == SysColorIndex.COLOR_WINDOWFRAME) {
                return (uint)SystemColors.WindowFrame.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_MENUTEXT) {
                return (uint)SystemColors.MenuText.ToArgb();
            }
            if (index == SysColorIndex.COLOR_WINDOWTEXT) {
                return (uint)SystemColors.WindowText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_CAPTIONTEXT) {
                return (uint)SystemColors.ActiveCaptionText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_ACTIVEBORDER) {
                return (uint)SystemColors.ActiveBorder.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_INACTIVEBORDER) {
                return (uint)SystemColors.InactiveBorder.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_APPWORKSPACE) {
                return (uint)SystemColors.Window.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_HIGHLIGHT) {
                return (uint)SystemColors.Highlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_HIGHLIGHTTEXT) {
                return (uint)SystemColors.HighlightText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_BTNFACE) {
                return (uint)SystemColors.ButtonFace.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_BTNSHADOW) {
                return (uint)SystemColors.ButtonShadow.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_GRAYTEXT) {
                return (uint)SystemColors.GrayText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_BTNTEXT) {
                return (uint)SystemColors.WindowText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_INACTIVECAPTIONTEXT) {
                return (uint)SystemColors.InactiveCaptionText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_BTNHIGHLIGHT) {
                return (uint)SystemColors.ButtonHighlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DDKSHADOW) {
                return (uint)SystemColors.ButtonShadow.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DLIGHT) {
                return (uint)SystemColors.ButtonHighlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_INFOTEXT) {
                return (uint)SystemColors.InfoText.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_INFOBK) {
                return (uint)SystemColors.Info.ToArgb();
            }
            ;

            if (index == SysColorIndex.COLOR_HOTLIGHT) {
                return (uint)SystemColors.HotTrack.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_GRADIENTACTIVECAPTION) {
                return (uint)SystemColors.GradientActiveCaption.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_GRADIENTINACTIVECAPTION) {
                return (uint)SystemColors.GradientInactiveCaption.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_MENUHIGHLIGHT) {
                return (uint)SystemColors.MenuHighlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_MENUBAR) {
                return (uint)SystemColors.MenuBar.ToArgb();
            }
            ;

            if (index == SysColorIndex.COLOR_DESKTOP) {
                return (uint)SystemColors.Desktop.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DFACE) {
                return (uint)SystemColors.ButtonFace.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DSHADOW) {
                return (uint)SystemColors.ButtonShadow.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DHIGHLIGHT) {
                return (uint)SystemColors.ButtonHighlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_3DHILIGHT) {
                return (uint)SystemColors.ButtonHighlight.ToArgb();
            }
            ;
            if (index == SysColorIndex.COLOR_BTNHILIGHT) {
                return (uint)SystemColors.ButtonHighlight.ToArgb();
            }
            ;


            return 0;
        }

    }
}