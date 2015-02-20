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
using Limaki.Drawing;

namespace Limaki.View.Viz.Rendering {

    public interface ISelectionRenderer : IRenderAction {
        IShape Shape { get; set; }
        
        bool ShowGrips { get; set; }
        int GripSize { get; set; }
        IStyle Style { get; set; }
        
        void UpdateSelection();
        void InvalidateShapeOutline(IShape oldShape, IShape newShape);
        void Clear();

        IVidgetBackend Backend { get; set; }
        Func<IClipper> Clipper { get; set; }
        Func<ICamera> Camera { get; set; }

        Func<ISurface, object> SaveMatrix { get; set; }
        Action<ISurface> SetMatrix { get; set; }
        Action<ISurface, object> RestoreMatrix { get; set; }
    }
}