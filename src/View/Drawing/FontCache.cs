using System.Drawing;

namespace Limaki.Drawing {
    /// <summary>
    /// A simple FontCache which remembers the last used font
    /// if the font changes, disposes the old font and makes a new one
    /// </summary>
    public class FontCache {
        Font cachedFont = null;
        FontMemento cachedMemento = default(FontMemento);

        public Font GetFont(FontMemento newFont) {
            bool doNewFont = cachedFont == null;

            if (!doNewFont) {
                doNewFont = cachedMemento.CompareTo(newFont) != 0;
            }
            if (doNewFont) {
                Font lastFont = cachedFont;
                cachedFont = new Font(newFont.Name, newFont.SizeInPoints, newFont.Style);
                cachedMemento = new FontMemento(cachedFont);

                if (lastFont != null && !object.ReferenceEquals(lastFont, cachedFont))
                    lastFont.Dispose();
                lastFont = null;

            }
            return cachedFont;
        }
    }
}