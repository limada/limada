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

using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt;
using SWF = System.Windows.Forms;

namespace Limaki.View.SwfBackend.VidgetBackends {
    
    public class ToolbarPanelBackendDummy : VidgetBackend<SWF.Panel>, IToolbarPanelBackend {

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
        }

        public void InsertItem (int index, IToolbarBackend backend) { }

        public void RemoveItem (IToolbarBackend backend) { }

        public void SetVisibility (Visibility value) { }

        public void AddToWindow (IVindow vindow) {

        }

    }
}