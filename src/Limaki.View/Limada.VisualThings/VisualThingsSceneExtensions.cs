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

using System.IO;
using Limaki.Drawing;
using Limaki.Model.Content;
using Limaki.Visuals;
using Limaki.View.Visuals.UI;
using Limaki.Graphs.Extensions;
using Limada.Model;

namespace Limada.VisualThings {

    public static class VisualThingsSceneExtensions {
        /// <summary>
        /// gives back the conntent of the scene's focused if it is backed by a StreamThing
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static Content<Stream> ContentOfFocused (this IGraphScene<IVisual, IVisualEdge> scene) {
            var graph = scene.Graph;
            if (graph != null && scene.Focused != null) {
                return new VisualThingContentFacade().ContentOf(graph, scene.Focused);
            }
            return null;
        }

        /// <summary>
        /// adds a visual, backed by a StreamThing out of content
        /// to the scene
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="content"></param>
        /// <param name="layout"></param>
        public static void AddContent (this IGraphScene<IVisual,IVisualEdge> scene, Content<Stream> content,  IGraphSceneLayout<IVisual, IVisualEdge> layout) {
            var thing = new VisualThingContentFacade().VisualOfContent(scene.Graph, content);
            if (scene.Focused != null) {
                SceneExtensions.PlaceVisual(scene, scene.Focused, thing, layout);
            } else {
                SceneExtensions.AddItem(scene, thing, layout, scene.NoHit);
            }
        }

        /// <summary>
        /// true if scene.Graph is backed by a ThingGraph
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static bool HasThingGraph (this IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene != null) {
                var graph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();
                return graph != null;
            }
            return false;
        }
    }
}