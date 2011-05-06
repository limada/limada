/*
 * Limaki 
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


using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using Id = System.Int64;

namespace Limaki.Presenter.Widgets {
    public class WidgetDisplay: GraphSceneDisplay<IWidget,IEdgeWidget> {
        
        public virtual new Scene Data {
            get { return base.Data as Scene; }
            set { base.Data = value; }
        }

       
       

        

        

    }

    public class WidgetRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IGraphModelFactory<IWidget, IEdgeWidget>, WidgetFactory>();
        }
    }
}