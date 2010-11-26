/*
 * Limaki 
 * Version 0.063
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
    public class SceneTestBinaryTreeData : SceneTestWithTreeData {
        public override string Name {
            get { return "Binary Tree"; }
        }
        public override void Populate(Scene scene) {
            base.Populate (scene);

            ILinkWidget linkWidget = null;
            IWidget widget = null;
            
                #region Binarytree first

                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node1;
                linkWidget.Leaf = Node2;
                scene.Add (linkWidget);

                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node1;
                linkWidget.Leaf = Node4;
                scene.Add (linkWidget);


                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node2;
                linkWidget.Leaf = Node3;
                scene.Add (linkWidget);
                Link1 = linkWidget;

                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node4;
                linkWidget.Leaf = Node5;
                scene.Add (linkWidget);
                Link2 = linkWidget;


                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node4;
                linkWidget.Leaf = Node8;
                scene.Add (linkWidget);
                Link3 = linkWidget;

                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node5;
                linkWidget.Leaf = Node6;
                scene.Add (linkWidget);
                Link4 = linkWidget;

                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node5;
                linkWidget.Leaf = Node7;
                scene.Add (linkWidget);
                Link5 = linkWidget;


                linkWidget = new LinkWidget<string> (string.Empty);
                linkWidget.Root = Node8;
                linkWidget.Leaf = Node9;
                scene.Add (linkWidget);

                #endregion

        }


       
    }

   
}
