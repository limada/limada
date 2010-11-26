/*
 * Limaki 
 * Version 0.071
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
    public interface IDataObjectHandler<T> {
        Type HandledType { get; }
        string MimeType { get; }
        void SetData ( IDataObject data, T value );
    }
}