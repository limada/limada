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

using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.View.Visuals.Visualizers {
    public class VisualsDisplayFactory:GraphSceneDisplayFactory<IVisual,IVisualEdge> {
        public override Display<IGraphScene<IVisual, IVisualEdge>> Create() {
            var result = new GraphSceneDisplay<IVisual, IVisualEdge>();
            return result;           
        }

        public override IComposer<Display<IGraphScene<IVisual, IVisualEdge>>> DisplayComposer {
            get {
                if (_displayComposer == null) {
                    _displayComposer = new VisualsDisplayComposer();
                }
                return base.DisplayComposer;
            }
            set {
                base.DisplayComposer = value;
            }
        }       
    }
}