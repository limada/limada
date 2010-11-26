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

using System.ComponentModel;
using System.Drawing;
using Limaki.Actions;
using Limaki.Widgets;

namespace Limaki.Widgets.Layout {
    public class LayoutProperties {
        private Orientation _orientation = Orientation.TopBottom;
        private bool _centered = false;
        private Algo _algo = Algo.DepthFirst;
        private bool _showDebug = false;

        private ILayout _layout;
        private GraphLayout<Scene, IWidget> layout;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILayout Layout {
            get { return _layout; }
            set {
                layout = null;
                if (value is GraphLayout<Scene, IWidget>) {
                    layout = (GraphLayout<Scene, IWidget>)value;
                }
                _layout = value;

            }
        }

        public Orientation Orientation {
            get { return layout.Orientation; }
            set { layout.Orientation = value; }
        }

        public bool Centered {
            get { return layout.Centered; }
            set { layout.Centered = value; }
        }

        private Size _distance;
        public Size Distance {
            get { return layout.Distance; }
            set { layout.Distance = value; }
        }

        public Algo Algo {
            get { return layout.Algo; }
            set { layout.Algo = value; }
        }

        public bool ShowDebug {
            get { return layout.showDebug; }
            set { layout.showDebug = value; }
        }
    }
}