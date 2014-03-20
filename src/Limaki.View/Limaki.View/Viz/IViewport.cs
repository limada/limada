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


using Limaki.Drawing;
using Xwt;
using System;

namespace Limaki.View.Viz {
    /// <summary>
    /// home of the camera
    /// </summary>
    public interface IViewport {
        /// <summary>
        /// this camera is the source of all cameras
        /// </summary>
        ICamera Camera { get; }

        /// <summary>
        /// origin of the Data in 
        /// Viewport Coordinates 
        /// </summary>
        Point DataOrigin { get; }

        /// <summary>
        /// size of the visible Data in 
        /// Viewport Coordinates
        /// </summary>
        Size DataSize { get; }

        /// <summary>
        /// origin of the visible Clip in 
        /// Viewport Coordinates
        /// </summary>
        Point ClipOrigin { get; set; }

        /// <summary>
        /// size of the visible Clip in 
        /// Viewport Coordinates
        /// </summary>
        Size ClipSize { get; }

        /// <summary>
        /// Origin of Data in
        /// World Coordinates
        /// </summary>
        Func<Point> GetDataOrigin { get; set; }
        /// <summary>
        /// size of Data in 
        /// World Coordinates  
        /// </summary>
        Func<Size> GetDataSize { get; set; }
        

        void Update();
        void Update(IClipper clipper);
        void UpdateCamera();
        void UpdateZoom();
        void Reset();
        double ZoomFactor { get; set; }
        ZoomState ZoomState { get; set; }
        
    }
}