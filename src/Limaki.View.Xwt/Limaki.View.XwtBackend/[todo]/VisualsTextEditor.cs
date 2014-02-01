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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Limaki.View.UI;

namespace Limaki.View.XwtBackend.Visualizers {

    public class VisualsTextEditor : VisualsTextEditorBase {

        public VisualsTextEditor(
            Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler,
            IDisplay display, ICamera camera,
            IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base(sceneHandler, display, camera, layout) {

            this.displayBackend = display.Backend;
        }

        private IVidgetBackend displayBackend = null;

        protected override void AttachEditor () {
           
        }

        protected override void DetachEditor (bool writeData) {
            
        }

        protected override void ActivateMarkers () {
            
        }

        public override void OnKeyPressed (KeyActionEventArgs e) {
            
        }
    }
}
