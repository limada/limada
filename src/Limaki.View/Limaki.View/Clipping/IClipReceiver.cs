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
using Limaki.View.Rendering;

namespace Limaki.View.Clipping {
    /// <summary>
    /// calls the Renderer and updates the Viewport
    /// the subjects are Viewport and Rederer
    /// the command-subject is the Clipper 
    /// </summary>
    public interface IClipReceiver : IReceiver {

        IClipper Clipper { get; set; }

        Get<IViewport> Viewport { get; set; }
        Get<IBackendRenderer> Renderer { get; set; }
    }
}