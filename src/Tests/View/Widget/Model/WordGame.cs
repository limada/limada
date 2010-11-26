/*
 * Limaki 
 * Version 0.07
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
using System.Drawing;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Tests.Widget {
    public class WordGame : OldSceneTestData {
        public override string Name {
            get { return "Word Game"; }
        }
        public override Scene Scene {
            get {
                Scene result = new Scene();
                IWidget lastNode1 = null;
                IWidget lastNode2 = null;
                IWidget lastNode3 = null;
                for (int i = 0; i < Count; i++) {
                    if (i > 0) {
                        lastNode1 = Node1;
                        lastNode2 = Node3;
                        lastNode3 = Link3;
                    }
                    Populate(result);
                    if (i > 0) {
                        IEdgeWidget edgeWidget = new EdgeWidget<string>(string.Empty);
                        edgeWidget.Root = lastNode1;
                        edgeWidget.Leaf = Node1;
                        result.Add(edgeWidget);
                        if (seperateLattice) {
                            edgeWidget = new EdgeWidget<string>(string.Empty);
                            edgeWidget.Root = lastNode2;
                            edgeWidget.Leaf = Node3;
                            result.Add(edgeWidget);
                        }
                        if (addDensity) {
                            edgeWidget = new EdgeWidget<string>(string.Empty);
                            edgeWidget.Root = Node2;
                            edgeWidget.Leaf = lastNode3;
                            result.Add(edgeWidget);
                        }
                    }
                }

                MakeLinkStrings(result);

                return result;
            }
        }


        public override void Populate(Scene scene) {
            IEdgeWidget edgeWidget = null;
            IWidget widget = null;


            widget = new Widget<string>("Tags");
            scene.Add(widget);
            Node1 = widget;


            widget = new Widget<string>("Word");
            scene.Add(widget);
            Node2 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Node1, Node2);
            scene.Add(edgeWidget);
            Link1 = edgeWidget;


            widget = new Widget<string>("Game");
            scene.Add(widget);
            Node3 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Node2, Node3);
            scene.Add(edgeWidget);
            Link2 = edgeWidget;

            widget = new Widget<string>("Something");
            scene.Add(widget);
            Node4 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link2, Node4);
            scene.Add(edgeWidget);
            Link3 = edgeWidget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Node1, Link2);
            scene.Add(edgeWidget);
            Link3 = edgeWidget;

        }


    }


}
