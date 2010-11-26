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
using System.IO;
using System.Windows.Forms;

namespace Limaki.Presenter.Winform.DragDrop {
    public abstract class StreamDataObjectHandler<TContainer, TItem> : IDataObjectHandler<TContainer, TItem>
        where TItem : class {

        public virtual Type HandledType { get { return typeof(TItem); } }

        public abstract string[] DataFormats { get; }

        public abstract TItem GetData(Stream stream, TContainer container);
        public abstract Stream SetData(TContainer container, TItem value);

        public virtual TItem Clone(TContainer container, TItem data) {
            Stream stream = SetData(container, data);
            return GetData(stream, container);
        }


        public virtual TItem GetData(IDataObject data, TContainer container) {
            return GetData(data.GetData(DataFormats[0]) as Stream, container);
        }

        public virtual bool Supports(TContainer container, TItem value) {
            return false;
        }

        public virtual void SetData(IDataObject data, TContainer container, TItem value) {
            if(Supports(container,value)) {
                Stream stream = SetData (container, value);
                data.SetData (DataFormats[0], stream);
            }
        }
    }
}