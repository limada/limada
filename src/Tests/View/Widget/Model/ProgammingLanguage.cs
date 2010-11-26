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
    public class ProgammingLanguage : OldSceneTestData {
        public override string Name {
            get { return "Programming Language"; }
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
                        lastNode2 = Node5;
                        lastNode3 = Node8;
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
                            edgeWidget.Leaf = Node5;
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

            widget = new Widget<string>("Programming");
            scene.Add(widget);
            Node1 = widget;


            widget = new Widget<string>("Language");
            scene.Add(widget);
            Node2 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Node1, Node2);
            scene.Add(edgeWidget);
            Link1 = edgeWidget;


            widget = new Widget<string>("Java");
            scene.Add(widget);
            Node3 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link1, Node3);
            scene.Add(edgeWidget);
            Link2 = edgeWidget;

            widget = new Widget<string>(".NET");
            scene.Add(widget);
            Node4 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link1, Node4);
            scene.Add(edgeWidget);
            Link3 = edgeWidget;

            widget = new Widget<string>("Libraries");
            scene.Add(widget);
            Node5 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Node1, Node5);
            scene.Add(edgeWidget);
            Link4 = edgeWidget;

            widget = new Widget<string>("Collections");
            scene.Add(widget);
            Node6 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link4, Node6);
            scene.Add(edgeWidget);
            Link5 = edgeWidget;

            widget = new Widget<string>("List");
            scene.Add(widget);
            Node7 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link5, Node7);
            scene.Add(edgeWidget);
            Link6 = edgeWidget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link2, Node7);
            scene.Add(edgeWidget);

            widget = new Widget<string>("IList");
            scene.Add(widget);
            Node8 = widget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link5, Node8);
            scene.Add(edgeWidget);
            Link7 = edgeWidget;

            edgeWidget = new EdgeWidget<string>(string.Empty, Link3, Node8);
            scene.Add(edgeWidget);

        }


    }


}
