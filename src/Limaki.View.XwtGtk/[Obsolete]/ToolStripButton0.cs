/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Vidgets;
using System;


namespace Limaki.View.GtkBackend {

    [Obsolete]
    public class ToolStripButton0 : ToolStripButton, IToolStripCommandToggle0, IToolStripItem0 {

        public ToolStripButton0 (): base () { }
        
        public IToolStripCommandToggle0 ToggleOnClick { get; set; }

        protected ToolStripCommand0 _command = null;
        public ToolStripCommand0 Command {
            get { return _command; }
            set {
                var first = _command == null;
                VidgetUtils.SetCommand (this, ref _command, value);
                if (first)
                    Compose ();
            }
        }




        public string ToolTipText {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }

        public Xwt.Size Size {
            get {
                throw new NotImplementedException ();
            }
            set {
                throw new NotImplementedException ();
            }
        }

        public event EventHandler Click;
    }
}