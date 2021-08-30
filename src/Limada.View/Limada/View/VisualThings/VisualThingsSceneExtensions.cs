/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using Limada.Model;
using Limaki.Common;
using Limaki.Contents;
using System.IO;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.Visuals;

namespace Limada.View.VisualThings {

    /// <summary>
    /// extensions of VisualGraphScenes backed by ThingGraphs
    /// </summary>
    public static class VisualThingsSceneExtensions {

        /// <summary>
        /// true if scene.Graph is backed by a ThingGraph
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static bool HasThingGraph (this IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene == null)
                return false;

            return scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink> ()!=null;
        }
    }
}