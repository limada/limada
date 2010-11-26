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

using Limaki.Graphs;
using Limaki.Widgets;

namespace Limaki.Winform.DragDrop {
    public class SerializedWidgetDataObjectHandler : SerializedDataObjectHandler<IGraph<IWidget, IEdgeWidget>, IWidget>, IWidgetDataObjectHandler {
        public override string[] DataFormats {
            get { return new string[] { "Limaki_IWidget-Binary" }; }
        }
    }
}