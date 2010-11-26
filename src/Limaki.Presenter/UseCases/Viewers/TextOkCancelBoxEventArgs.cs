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
using Limaki.Common;

namespace Limaki.UseCases.Viewers {
    public class TextOkCancelBoxEventArgs:EventArgs<DialogResult> {
        public TextOkCancelBoxEventArgs(DialogResult arg, Action<string> onOK ): base(arg) {
            this.OnOK = onOK;
        }

        public Action<string> OnOK { get; protected set; }
    }
}