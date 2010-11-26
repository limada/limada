/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;

namespace Limaki.Drawing {
    public class Font : IDisposable, ICloneable {
        public Font() {
            this.Style = FontStyle.Normal;
            //this.Weight = FontWeight.Normal;
            this.Size = 10;
        }

        public virtual string FontFamily { get; set; }
        public virtual double Size { get; set; }
        public virtual FontStyle Style { get; set; }
        //public virtual FontWeight Weight { get; set; }
        
        public virtual void Dispose(bool disposing) {}

        ~Font() {
            Dispose(false);
        }

        public virtual void Dispose() {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        public Font(string fontFamily, double size) {
            this.FontFamily = fontFamily;
            this.Size = size;
        }

        public Font(Font font)  {
            this.FontFamily = font.FontFamily;
            this.Size  = font.Size;
            this.Style = font.Style;
            //this.Weight = font.Weight;
            
        }

        public virtual object Clone() {
            return new Font (this);
        }

        public override bool Equals(object obj) {
            if (!(obj is Font))
                return false;
            var other = (Font)obj;
            return 
                   this.FontFamily == other.FontFamily &&
                   this.Size == other.Size &&
                   this.Style == other.Style;
                   // && this.Weight == other.Weight;
        }

        public override int GetHashCode() {
            return
                this.FontFamily.GetHashCode () ^
                this.Size.GetHashCode () ^
                this.Style.GetHashCode ();
                //^this.Weight.GetHashCode();
        }
    }

    /* 
     * this class is ported from
     * Moonlight - moon\class\moon\class\System.Windows\System.Windows\FontWeigth.cs
     * 
     */

    //public enum FontWeight {
    //    Thin = 100,
    //    ExtraLight = 200,
    //    Light = 300,
    //    Normal = 400,
    //    Medium = 500,
    //    SemiBold = 600,
    //    Bold = 700,
    //    ExtraBold = 800,
    //    Black = 900,
    //    ExtraBlack = 950,
    //}

    /* 
     * this class is ported from
     * mono 2.4 - mcs\class\System.Drawing\System.Drawing\FontStyle.cs
     * 
     */

    [Flags]
    public enum FontStyle {
        Normal=0,
        Bold=1,
        Italic=2,
        Underline=4,
        Strikeout = 8,
        AllStyles = 0xF
    }
}