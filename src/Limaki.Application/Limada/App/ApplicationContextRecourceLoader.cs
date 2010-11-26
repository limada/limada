/*
 * Limada
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using Limada.Data;
using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;
using Limaki.Data;

namespace Limada.App {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader for limada-application
    /// which uses a DataBaseHandler, a Marker, and a GraphMapping for IWidget, IThing-Graphs
    /// </summary>
    public class ApplicationContextRecourceLoader : Limaki.Context.WinformContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            
            base.ApplyResources (context);

            var providers = context.Pool.TryGetCreate<DataProviders<IThingGraph>>();
            providers.Add(typeof(Db4oThingGraphProvider));
            //providers.Add(typeof(XMLThingGraphProvider));

        }
    }
}