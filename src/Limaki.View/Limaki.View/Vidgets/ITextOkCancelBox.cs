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

namespace Limaki.View.Vidgets {

    public interface ITextOkCancelBox  {
        DialogResult Result { get; set; }

        string Title { get; set; }
        string Text { get; set; }
        Action<string> OnOk { get; set; }
        event EventHandler<TextOkCancelBoxEventArgs> Finish;
    }

    public class TextOkCancelBox {
        public DialogResult Result { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public Action<string> OnOk { get; set; }
        public event EventHandler<TextOkCancelBoxEventArgs> Finish;
    }
}