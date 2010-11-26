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

namespace Limaki.Drawing.UI {
    public interface IControl {
        void Invalidate();
        void Invalidate(RectangleI rect);
        
        RectangleI ClientRectangle { get;}
        SizeI Size {get;}
        void Update();
        void CommandsExecute();
    }

    public interface IDataDisplay<T> {
        T Data { get; set; }
        void CommandsInvoke();
        void CommandsExecute();
    }
}