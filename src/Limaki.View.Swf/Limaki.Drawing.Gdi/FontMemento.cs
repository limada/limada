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
 * http://www.limada.org
 * 
 */

using System;
using System.Drawing;

namespace Limaki.Drawing.Gdi {

    public struct FontMemento : IComparable<FontMemento>, IComparable<System.Drawing.Font> {
        public float SizeInPoints;
        public System.Drawing.FontStyle Style;
        public string Name;
        public FontMemento (string name, System.Drawing.FontStyle style, float sizeInPoints) {
            this.SizeInPoints = sizeInPoints;
            this.Name = name;
            this.Style = style;
        }
        public FontMemento (System.Drawing.Font font) {
            this.Style = font.Style;
            this.Name = font.Name;
            this.SizeInPoints = font.SizeInPoints;
        }

        #region IComparable<FontMemento> Member

        public int CompareTo (FontMemento other) {
            int result = this.Name.CompareTo (other.Name);
            if (result == 0) {
                result = this.SizeInPoints.CompareTo (other.SizeInPoints);
                if (result == 0) {
                    result = this.Style.CompareTo (other.Style);
                }
            }
            return result;
        }

        #endregion

        #region IComparable<Font> Member

        public int CompareTo (System.Drawing.Font other) {
            int result = this.Name.CompareTo (other.Name);
            if (result == 0) {
                result = this.SizeInPoints.CompareTo (other.SizeInPoints);
                if (result == 0) {
                    result = this.Style.CompareTo (other.Style);
                }
            }
            return result;
        }

        #endregion
    }
}