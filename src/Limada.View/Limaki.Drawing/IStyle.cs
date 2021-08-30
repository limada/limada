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
    /// Font, Fillcolor etc.
    /// </summary>
    public interface IStyle : IDisposable, ICloneable {

        string Name { get; set; }

        /// <summary>
        /// color of interior of shapes.
        /// </summary>
        Color FillColor { get; set; }

        /// <summary>
        /// The text color is used to draw text strings
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// color of lines
        /// </summary>
        Color StrokeColor { get; set; }

        double LineWidth { get; set; }

        /// <summary>
        /// The font is used as the typeface for drawing text
        /// </summary>
        Font Font { get; set; }
        
        /// <summary>
        /// Styles can be linked to other Styles to reuse
        /// resource-intensiv GDI-Objects
        /// Implementation of IStyle should use the GDI-Objects of the
        /// ParentStyle if it has the same appearence
        /// </summary>
        IStyle ParentStyle { get; set; }

        /// <summary>
        /// maximum height and width
        /// of an automatic sizing process
        /// </summary>
        Size AutoSize { get; set; }

        /// <summary>
        /// if the data should be painted or only the shape
        /// </summary>
        bool PaintData { get; set; }

        Spacing Padding { get; set; }

        void CopyTo(IStyle other);
    }

}
