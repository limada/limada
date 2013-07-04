/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.IO;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Text;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Ui.DragDrop1 {

    public class DragDropViz {

        public virtual void LinkItem (IGraphScene<IVisual, IVisualEdge> scene, IVisual item, Point pt, int hitSize, bool itemIsRoot) {
            if (item != null) {
                var target = scene.Hovered;
                if (target == null && scene.Focused != null && scene.Focused.Shape.IsHit(pt, hitSize)) {
                    target = scene.Focused;
                }
                if (item != target) {
                    if (itemIsRoot)
                        SceneExtensions.CreateEdge(scene, item, target);
                    else
                        SceneExtensions.CreateEdge(scene, target, item);
                }
            }
        }
    }
}