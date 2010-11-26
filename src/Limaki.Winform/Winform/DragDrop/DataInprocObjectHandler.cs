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


using System;
using System.Windows.Forms;

namespace Limaki.Winform.DragDrop {
    public abstract class DataInprocObjectHandler<TContainer, TItem> : IDataObjectHandler<TContainer, TItem>
        where TItem : class {
        public virtual Type HandledType {
            get { return typeof(TItem); }
        }
        public abstract string[] DataFormats { get; }
        public virtual TItem GetData(IDataObject data, TContainer container) {
            object o = data.GetData(HandledType);
            if (o is TItem)
                return o as TItem;
            else
                return null;
        }
        public virtual void SetData(IDataObject data, TContainer container, TItem value) {
            data.SetData(HandledType, value);
        }

    }
}