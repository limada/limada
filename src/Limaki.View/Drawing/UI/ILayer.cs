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
 */


namespace Limaki.Drawing.UI {
    public interface ILayer : IPaintAction, ICameraTarget {
        SizeI Size { get; set;}
        PointI Offset { get; set; }
        void DataChanged();
    }

    public interface ILayer<T> : ILayer {
        T Data { get; set;}
    }
}