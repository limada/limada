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

using Limaki.Graphs;
using Limaki.Common.IOC;
using Limaki.Visuals;
using Limaki.Drawing;
using Limaki.View.Visuals.Layout;
using Limaki.View.Mesh;
using Limaki.View.Visuals.UI;
using Limaki.Common;
using System;
using Limada.Model;

namespace Limaki.View.Visuals.Visualizers {

    public class VisualsResourceLoader : ContextResourceLoader {

        public override void ApplyResources (IApplicationContext context) {

            if (!context.Factory.Contains<IVisualFactory> ())
                context.Factory.Add<IVisualFactory, VisualFactory> ();

            context.Factory.Add<IGraphModelFactory<IVisual, IVisualEdge>, VisualFactory> ();
            context.Factory.Add<IGraphSceneLayout<IVisual, IVisualEdge>> (
                args => new VisualsSceneLayout<IVisual, IVisualEdge> (args[0] as Func<IGraphScene<IVisual, IVisualEdge>>, args[1] as IStyleSheet)
                );

            context.Factory.Add<IGraphSceneMesh<IVisual, IVisualEdge>, VisualGraphSceneMesh> ();

            var dependencies = context.Pool.TryGetCreate<GraphDepencencies<IVisual, IVisualEdge>> ();
            dependencies.Visitor += (c, a, t) => GraphDepencencyExtension
                .DependencyVisitor<IVisual, IVisualEdge, IThing, ILink> (c, a, t);

        }
    }
}