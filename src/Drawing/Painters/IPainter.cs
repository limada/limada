/*
 * Limaki 
 * Version 0.07
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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Limaki.Drawing;

namespace Limaki.Drawing.Painters {
    /// <summary>
    /// Painters are responsible for drawing a shape 
    /// on a System.Drawing.Graphics
    /// </summary>
    public interface IPainter:IDisposable {
        /// <summary>
        /// how to draw the shape: borders, inside, both or none
        /// </summary>
        RenderType RenderType { get; set; }

        /// <summary>
        /// the style information
        /// </summary>
		IStyle Style { get;set; }

        /// <summary>
        /// the shape to draw
        /// </summary>
		IShape Shape { get;set; }

        /// <summary>
        /// paint the shape with style on a Graphics
        /// </summary>
        /// <param name="g"></param>
        void Render(Graphics g);

        /// <summary>
        /// the Hull of the shape with style
        /// </summary>
        /// <returns></returns>
        Point[] Measure(Matrice matrix, int delta, bool extend);
    }

    public interface IPainter<T>:IPainter {
		new IShape<T> Shape { get;set; }
    }

    [Flags]
    public enum RenderType {
        /// <summary>
        /// dont draw or fill
        /// </summary>
        None = 0,
        /// <summary>
        /// draw the borders öf the shape
        /// </summary>
        Draw = 1,
        /// <summary>
        /// fill the area of the shape
        /// </summary>
        Fill = 2,
        /// <summary>
        /// draw and fill the shape
        /// </summary>
        DrawAndFill = Draw | Fill
    }

    public interface IDataPainter:IPainter {
        object Data { get; set; }
    }
}
