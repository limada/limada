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

namespace Limaki.View.Viz.Rendering {
    /// <summary>
    /// calls the Renderer and updates the Viewport
    /// the subjects are Viewport and Rederer
    /// the command-subject is the Clipper 
    /// </summary>
    public interface IClipReceiver : IReceiver {

        IClipper Clipper { get; set; }

        Func<IViewport> Viewport { get; set; }
        Func<IBackendRenderer> Renderer { get; set; }
    }
}