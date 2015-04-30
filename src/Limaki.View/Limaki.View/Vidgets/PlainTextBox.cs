/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt.Backends;

namespace Limaki.View.Vidgets {

    public interface IPlainTextBoxBackend : IVidgetBackend {
        string Text { get; set; }
        bool ShowFrame { get; set; }
    }

    public interface IPlainTextBoxVidgetBackend : IVidgetBackend, IPlainTextBoxBackend { }

    [BackendType (typeof (IPlainTextBoxVidgetBackend))]
    public class PlainTextBox : Vidget {
        protected IPlainTextBoxVidgetBackend _backend = null;
        public virtual new IPlainTextBoxVidgetBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as IPlainTextBoxVidgetBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        
        public string Text {
            get { return Backend.Text; }
            set { Backend.Text = value; }
        }

        public bool ShowFrame {
            get { return Backend.ShowFrame; }
            set { Backend.ShowFrame = value; }
        }

        public override void Dispose () {
            Backend.Dispose ();
        }
    }
}