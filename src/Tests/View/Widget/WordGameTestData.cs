/*
 * Limaki 
 * Version 0.064
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
    public class WordGameTestData : SceneTestData {
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
                        ILinkWidget linkWidget = new LinkWidget<string>(string.Empty);
                        linkWidget.Root = lastNode1;
                        linkWidget.Leaf = Node1;
                        result.Add(linkWidget);
                        if (seperateLattice) {
                            linkWidget = new LinkWidget<string>(string.Empty);
                            linkWidget.Root = lastNode2;
                            linkWidget.Leaf = Node3;
                            result.Add(linkWidget);
                        }
                        if (addDensity) {
                            linkWidget = new LinkWidget<string>(string.Empty);
                            linkWidget.Root = Node2;
                            linkWidget.Leaf = lastNode3;
                            result.Add(linkWidget);
                        }
                    }
                }

                MakeLinkStrings(result);

                return result;
            }
        }


        public override void Populate(Scene scene) {
            ILinkWidget linkWidget = null;
            IWidget widget = null;


            widget = new Widget<string>("Tags");
            scene.Add(widget);
            Node1 = widget;


            widget = new Widget<string>("Word");
            scene.Add(widget);
            Node2 = widget;

            linkWidget = new LinkWidget<string>(string.Empty, Node1, Node2);
            scene.Add(linkWidget);
            Link1 = linkWidget;


            widget = new Widget<string>("Game");
            scene.Add(widget);
            Node3 = widget;

            linkWidget = new LinkWidget<string>(string.Empty, Node2, Node3);
            scene.Add(linkWidget);
            Link2 = linkWidget;

            widget = new Widget<string>("Something");
            scene.Add(widget);
            Node4 = widget;

            linkWidget = new LinkWidget<string>(string.Empty, Link2, Node4);
            scene.Add(linkWidget);
            Link3 = linkWidget;

            linkWidget = new LinkWidget<string>(string.Empty, Node1, Link2);
            scene.Add(linkWidget);
            Link3 = linkWidget;

        }


    }


}
