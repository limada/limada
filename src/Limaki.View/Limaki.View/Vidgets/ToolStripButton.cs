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

using System;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    public interface IToolStripSeparatorBackend : IToolStripItemBackend {}
    [BackendType (typeof (IToolStripSeparatorBackend))]
    public class ToolStripSeparator : ToolStripItem {
    }

    [BackendType (typeof (IToolStripButtonBackend))]
    public class ToolStripButton : ToolStripItem {

        public ToolStripButton () { }

        public ToolStripButton (IToolStripCommand command) : base (command) { }

        private IToolStripButtonBackend _backend = null;
        public new virtual IToolStripButtonBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripButtonBackend); }
            set { _backend = value; }
        }

        public IToolStripCommand ToggleOnClick { get; set; }

        protected override void SetBackendAction (Action<object> value) {
            base.SetBackendAction (o => {
                                       value (o);
                                       if (ToggleOnClick != null)
                                           VidgetUtils.ToggleCommand (this, ToggleOnClick);
                                   });
        }

        public override void Dispose () { }

        public bool? IsChecked { get { return Backend.IsChecked; } set { Backend.IsChecked = value; } }

        public bool IsEnabled { get { return Backend.IsEnabled; } set { Backend.IsEnabled = value; } }

        public bool IsCheckable { get; set; }
    }

    public interface IToolStripButtonBackend : IToolStripItemBackend {
        //bool IsCheckable { get; set; }
        bool? IsChecked { get; set; }
    }
}