/*
 * Limaki 
 * Version 0.07
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

using System;
using System.Windows.Forms;
using Limaki.Widgets;
using Limaki.Winform.DragDrop;

namespace Limaki.Winform.DragDrop {
    public abstract class WidgetDataObjectHandler<OUT> : DataObjectHandler<IWidget, OUT>, IWidgetDataObjectHandler {
        public override IWidget GetData(IDataObject data) {
            object odata = data.GetData(HandledType);
            IWidget result = null;
            if (odata is OUT) {
                result = new Widget<OUT>((OUT)odata );
            }
            return result;
        }
    }
}