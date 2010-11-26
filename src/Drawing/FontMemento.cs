/*
 * Limaki 
 * Version 0.064
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
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Limaki.Drawing {
    public struct FontMemento:IComparable<FontMemento>, IComparable<Font> {
        public float SizeInPoints;
        public FontStyle Style;
        public string Name;
        public FontMemento(string name, FontStyle style, float sizeInPoints) {
            this.SizeInPoints = sizeInPoints;
            this.Name = name;
            this.Style = style;
        }
        public FontMemento(Font font) {
            this.Style = font.Style;
            this.Name = font.Name;
            this.SizeInPoints = font.SizeInPoints;
        }

        #region IComparable<FontMemento> Member

        public int CompareTo(FontMemento other) {
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

        public int CompareTo(Font other) {
            int result = this.Name.CompareTo(other.Name);
            if (result == 0) {
                result = this.SizeInPoints.CompareTo(other.SizeInPoints);
                if (result == 0) {
                    result = this.Style.CompareTo(other.Style);
                }
            }
            return result;
        }

        #endregion
    }
}
