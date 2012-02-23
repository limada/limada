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
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Common;
using Limaki.Drawing;
using Xwt;

namespace Limaki.Presenter {
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
        /// ViewPort Coordinates 
        /// </summary>
        Point DataOrigin { get; }

        /// <summary>
        /// size of the visible Data in 
        /// ViewPort Coordinates
        /// </summary>
        Size DataSize { get; }

        /// <summary>
        /// origin of the visible Clip in 
        /// ViewPort Coordinates
        /// </summary>
        Point ClipOrigin { get; set; }

        /// <summary>
        /// size of the visible Clip in 
        /// ViewPort Coordinates
        /// </summary>
        Size ClipSize { get; }

        /// <summary>
        /// Origin of Data in
        /// World Coordinates
        /// </summary>
        Get<Point> GetDataOrigin { get; set; }
        /// <summary>
        /// size of Data in 
        /// World Coordinates  
        /// </summary>
        Get<Size> GetDataSize { get; set; }
        

        void Update();
        void Update(IClipper clipper);
        void UpdateCamera();
        void UpdateZoom();
        void Reset();
        double ZoomFactor { get; set; }
        ZoomState ZoomState { get; set; }
        
    }
}