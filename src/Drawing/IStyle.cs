/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Drawing {
    /// <summary>
    /// Zusammenfassung für Style.
    /// </summary>
    public interface IStyle : IDisposable {
        string Name { get; set;}
        
        /// <summary>
        /// The fill color is used to fill the interior of shapes.
        /// </summary>
        Color FillColor { get; set; }

        /// <summary>
        /// The text color is used to draw text strings
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// The pen color is used to draw lines and the outlines of shapes.
        /// </summary>

        Color PenColor { get; set;}

        /// <summary>
        /// The pen used to draw lines and shape outlines.
        /// </summary>
        Pen Pen { get; set;}

        /// <summary>
        /// The font is used as the typeface for drawing text
        /// </summary>
        Font Font { get; set;}

        IStyle ParentStyle { get;set;}
        
        Size AutoSize { get;set;}
    }
}
