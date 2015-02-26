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
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    /// <summary>
    /// holds the information about
    /// Font, Pen, Fillcolor etc.
    /// IStyle-Implementations are the place for storing and reusing GDI-Objects
    /// </summary>
    public interface IStyle : IDisposable, ICloneable {
        string Name { get; set;}
        
        /// <summary>
        /// The fill color is used to fill the interior of shapes.
        /// </summary>
        Color FillColor { get; set; }

        /// <summary>
        /// The text color is used to draw text strings
        /// </summary>
        Color TextColor { get; set; }

        TextDecoration TextDecoration { get; set; }

        /// <summary>
        /// color of lines
        /// </summary>
        Color StrokeColor { get; set; }

        double LineWidth { get; set; }

        /// <summary>
        /// The font is used as the typeface for drawing text
        /// </summary>
        Font Font { get; set;}


        /// <summary>
        /// Styles can be linked to other Styles to reuse
        /// resource-intensiv GDI-Objects
        /// Implementation of IStyle should use the GDI-Objects of the
        /// ParentStyle if it has the same appearence
        /// </summary>
        IStyle ParentStyle { get;set;}
        
        /// <summary>
        /// The autoSize is used to define the maximum height and width
        /// of a automatic sizing process
        /// </summary>
        Size AutoSize { get;set;}

        /// <summary>
        /// The text color is used to draw text strings
        /// </summary>
        bool PaintData { get; set; }

        Spacing Padding { get; set; }

        void CopyTo(IStyle target);
    }

    [Flags]
    public enum TextDecoration {
        BaseLine = 0,
        Underline=1
    }
}
