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
using Limaki.Graphs;
using Limaki.Common.IOC;
using System;
using Limada.Model;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mesh;

namespace Limaki.View.Viz.Visuals {

    public class VisualsResourceLoader : ContextResourceLoader {

        public override void ApplyResources (IApplicationContext context) {

            if (!context.Factory.Contains<IVisualFactory> ())
                context.Factory.Add<IVisualFactory, VisualFactory> ();

            context.Factory.Add<IGraphModelFactory<IVisual, IVisualEdge>, VisualFactory> ();
            context.Factory.Add<IGraphSceneLayout<IVisual, IVisualEdge>> (
                args => new VisualsSceneLayout<IVisual, IVisualEdge> (args[0] as Func<IGraphScene<IVisual, IVisualEdge>>, args[1] as IStyleSheet)
                );

            context.Factory.Add<IGraphSceneMesh<IVisual, IVisualEdge>, VisualGraphSceneMesh> ();

            var dependencies = context.Pooled<GraphDepencencies<IVisual, IVisualEdge>> ();
            dependencies.Visitor += (c, a, t) => GraphDepencencyExtension
                .DependencyVisitor<IVisual, IVisualEdge, IThing, ILink> (c, a, t);

        }
    }
}