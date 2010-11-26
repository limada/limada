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

namespace Limaki.Winform.DragDrop {
    public abstract class DataObjectHandler<IN,OUT> : IDataObjectHandler<IN> {
        public virtual Type HandledType {
            get { return typeof (OUT); }
        }
        public abstract string MimeType { get; }
        public abstract IN GetData(IDataObject data);
        public abstract void SetData(IDataObject data, IN value);

    }
}