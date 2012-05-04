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


namespace Limaki.Drawing.Gdi {
    /// <summary>
    /// A simple FontCache which remembers the last used font
    /// if the font changes, disposes the old font and makes a new one
    /// </summary>
    public class GdiFontCache {

        System.Drawing.Font cachedFont = null;
        FontMemento cachedMemento = default(FontMemento);

        public System.Drawing.Font GetFont(FontMemento newFont) {
            bool doNewFont = cachedFont == null;

            if (!doNewFont) {
                doNewFont = cachedMemento.CompareTo(newFont) != 0;
            }
            if (doNewFont) {
                System.Drawing.Font lastFont = cachedFont;
                cachedFont = new System.Drawing.Font(newFont.Name, newFont.SizeInPoints, newFont.Style);
                cachedMemento = new FontMemento(cachedFont);

                if (lastFont != null && !object.ReferenceEquals(lastFont, cachedFont))
                    lastFont.Dispose();
                lastFont = null;

            }
            return cachedFont;
        }
    }
}