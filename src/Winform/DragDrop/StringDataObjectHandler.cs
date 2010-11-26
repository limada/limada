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
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Winform.DragDrop;

namespace Limaki.Winform.DragDrop {
    public class StringDataObjectHandler : WidgetDataObjectHandler<string> {
        public override string MimeType {
            get { return "Mime/Text"; }
        }

        public override void SetData(IDataObject data, IWidget value) {
            data.SetData(HandledType, value.Data.ToString());
        }
    }
}