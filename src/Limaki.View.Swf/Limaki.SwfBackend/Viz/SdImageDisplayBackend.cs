/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Drawing;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;

namespace Limaki.SwfBackend.Viz {

    public class SdImageDisplayBackend : DisplayBackend<Image>, ISdImageDisplayBackend {

        public override DisplayFactory<Image> CreateDisplayFactory (DisplayBackend<Image> backend) {

            return new SdImageDisplayFactory {
                BackendComposer = new ImageDisplayBackendComposer { Backend = backend },
                DisplayComposer = new SdImageDisplayComposer(),
            };
        }
    }

    public class ImageDisplayBackendComposer : DisplayBackendComposer<Image> {

        public override void Factor (Display<Image> display) {
            base.Factor(display);
            this.DataLayer = new GdiImageLayer();
        }
    }
}