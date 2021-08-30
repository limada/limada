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
using Limaki.View.Common;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (ITextOkCancelBoxBackend))]
    public class TextOkCancelBox : Vidget {

        public string Title { set { Backend.Title = value; } }
        public string Text { get { return Backend.Text; } set { Backend.Text = value; } }

        ITextOkCancelBoxBackend _backend = null;
        public virtual new ITextOkCancelBoxBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as ITextOkCancelBoxBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        public override void Dispose () {
            if (Backend != null) {
                Backend.Dispose ();
                Backend = null;
            }
        }
    }

    public interface ITextOkCancelBoxBackend : IVidgetBackend {
        string Title { set; }
        string Text { set; get; }
        Action<DialogResult> Finish { get; set; }
    }
}