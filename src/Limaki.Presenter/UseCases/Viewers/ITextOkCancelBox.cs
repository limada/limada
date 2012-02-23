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
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limaki.Presenter;

namespace Limaki.UseCases.Viewers {
    public interface ITextOkCancelBox : IControl {
        DialogResult Result { get; set; }

        string Title { get; set; }
        string Text { get; set; }
        Action<string> OnOk { get; set; }
        event EventHandler<TextOkCancelBoxEventArgs> Finish;
    }
}