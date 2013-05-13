/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View;

namespace Limaki.Viewers {
    public interface ITextOkCancelBox : IVidgetBackend {
        DialogResult Result { get; set; }

        string Title { get; set; }
        string Text { get; set; }
        Action<string> OnOk { get; set; }
        event EventHandler<TextOkCancelBoxEventArgs> Finish;
    }
}