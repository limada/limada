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


using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public abstract class SceneTestWithTreeData : SceneTestData {
        protected int start = 1; 
        public override void Populate(Scene scene) {
            ILinkWidget linkWidget = null;
            IWidget widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node1 = widget;


            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node2 = widget;


            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node3 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node4 = widget;

            #region second lattice nodes

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node5 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node6 = widget;


            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node7 = widget;

            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node8 = widget;


            widget = new Widget<string> (( start++ ).ToString ());
            scene.Add (widget);
            Node9 = widget;

            #endregion
        }
    }
}
