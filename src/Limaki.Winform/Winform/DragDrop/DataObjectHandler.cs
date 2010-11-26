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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Limaki.Winform.DragDrop {
    /// <summary>
    /// base class for handling IDataObject
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    public abstract class DataObjectHandler<TContainer, IN, OUT> : IDataObjectHandler<TContainer,IN> {
        public virtual Type HandledType {
            get { return typeof (OUT); }
        }
        public abstract string[] DataFormats { get; }
        public abstract IN GetData(IDataObject data, TContainer container);
        public abstract void SetData(IDataObject data, TContainer container, IN value);

    }
}