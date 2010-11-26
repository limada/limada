/*
 * Limaki 
 * Version 0.08
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

namespace Limaki.Widgets {
    public interface IWidgetFactory {
        IWidget CreateWidget ( object data );
        IEdgeWidget CreateEdgeWidget ( object data );
        IEdgeWidget CreateEdgeWidget ( object data, IWidget root, IWidget leaf );
    }
}