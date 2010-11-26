/*
 * Limaki 
 * Version 0.081
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


using Limaki.Common;

namespace Limaki.Presenter {
    /// <summary>
    /// calls the Renderer and updates the ViewPort
    /// the subjects are ViewPort and Rederer
    /// the command-subject is the Clipper 
    /// </summary>
    public interface IClipReceiver : IReceiver {

        IClipper Clipper { get; set; }

        Get<IViewport> Viewport { get; set; }
        Get<IDeviceRenderer> Renderer { get; set; }
    }
}