/*
 * Limaki 
 * Version 0.071
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
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Query;
using Limaki.Common;
using Limaki.Data.db4o;
using Limaki.Widgets;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Data.db4o {
    public class WidgetGraph:Graph<IWidget,IEdgeWidget> {
        public WidgetGraph(Gateway gateway):base(gateway) {}

        protected override void DeclareTypesToConfigure() {
            base.DeclareTypesToConfigure();
            TypesToConfigure.Add(typeof(IShape));
            TypesToConfigure.Add(typeof(RectangleShape));
            TypesToConfigure.Add(typeof(Widget<string>));
            TypesToConfigure.Add(typeof(EdgeWidget<string>));
        }

        protected override void ConfigureType(Type type) {
            //Configuration.ObjectClass(typeof(Widget<string>)).GenerateUUIDs(true);
            //Configuration.ObjectClass(typeof(EdgeWidget<string>)).GenerateUUIDs(true);
            base.ConfigureType(type);

        }
    }
}
