/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

namespace Limaki.UseCases.Viewers.ToolStrips {
    public abstract class ToolController<TDisplay, TTool> {
        public virtual TTool Tool { get; set; }
        public virtual TDisplay CurrentDisplay { get; set; }
        public abstract void Attach ( object sender );
    }
}