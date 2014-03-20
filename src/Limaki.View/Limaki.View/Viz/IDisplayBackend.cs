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
 */

namespace Limaki.View.Viz {

    public interface IDisplayBackend : IVidgetBackend {
        IDisplay Frontend { get; set; }
    }

    public interface IDisplayBackend<T> : IDisplayBackend  {
        new IDisplay<T> Frontend { get; set; }
    }
}