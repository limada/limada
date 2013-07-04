/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;

namespace Limaki.View.Ui.DragDrop0 {

    public interface ITransferDataHandler<TContainer, TItem> {
        Type HandledType { get; }
        string[] Formats { get; }
        void SetData (TransferDataSource data, TContainer container, TItem value);
        TItem GetData (TransferDataSource data, TContainer container);
    }

    /// <summary>
    /// base class for handling IDataObject
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    public abstract class TransferDataHandler<TContainer, IN, OUT> : ITransferDataHandler<TContainer, IN> {
        public virtual Type HandledType {
            get { return typeof(OUT); }
        }
        public abstract string[] Formats { get; }
        public abstract IN GetData (TransferDataSource data, TContainer container);
        public abstract void SetData (TransferDataSource data, TContainer container, IN value);

    }

    public abstract class TransferDataInprocHandler<TContainer, TItem> : ITransferDataHandler<TContainer, TItem>
    where TItem : class {
        public virtual Type HandledType {
            get { return typeof(TItem); }
        }
        public abstract string[] Formats { get; }
        public virtual TItem GetData (TransferDataSource data, TContainer container) {
            object o = data.GetData(HandledType);
            if (o is TItem)
                return o as TItem;
            else
                return null;
        }
        public virtual void SetData (TransferDataSource data, TContainer container, TItem value) {
            data.SetData(HandledType, value);
        }

    }
}