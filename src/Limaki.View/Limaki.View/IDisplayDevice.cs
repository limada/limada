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
 */


namespace Limaki.View {
    public interface IDisplayDevice : IControl {
        IDisplay Display { get; set; }
    }
    public interface IDisplayDevice<T> : IDisplayDevice  {
//        new IDisplay<T> Display { get; set; }
    }
}