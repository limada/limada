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
using Xwt.WPFBackend;
using System.Windows.Media;

namespace Limaki.View.WpfBackend {

    public class ToolStripButton0 : ToolStripButton, IToolStripCommandToggle0, IToolStripItem0 {
        protected ToolStripCommand0 _command = null;
        public new ToolStripCommand0 Command {
            get { return _command; }
            set { VidgetUtils0.SetCommand (this, ref _command, value); }
        }

        #region IToolStripItem0-Implementation



        public IToolStripCommandToggle0 ToggleOnClick { get; set; }

        #endregion
    }
}
