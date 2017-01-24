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
using Xwt;

namespace Limaki.View.XwtBackend {

    public class ToolbarPanelBackendDummy : VidgetBackend<Xwt.HBox>, IToolbarPanelBackend {

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
        }


        public void InsertItem (int index, IToolbarBackend backend) { }

        public void RemoveItem (IToolbarBackend backend) { }

        public void SetVisibility (Visibility value) { }
    }
}