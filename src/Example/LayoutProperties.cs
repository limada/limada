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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Limaki;
using Limaki.Actions;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Orientation=Limaki.Widgets.Layout.Orientation;

namespace Limaki.Examples {
    public class LayoutProperties {
        private Limaki.Widgets.Layout.Orientation _orientation = Limaki.Widgets.Layout.Orientation.TopBottom;
        private bool _centered = false;
        private Algo _algo = Algo.DepthFirst;
        private bool _showDebug = false;

        private ILayout _layout;
        private SimpleWidgetLayout<Scene, IWidget> layout;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILayout SceneLayout {
            get { return _layout; }
            set {
                layout = null;
                if (value is SimpleWidgetLayout<Scene, IWidget>) {
                    layout = (SimpleWidgetLayout<Scene, IWidget>)value;
                }
                _layout = value;

            }
        }

        public Limaki.Widgets.Layout.Orientation Orientation {
            get { return layout.Orientation; }
            set { layout.Orientation = value; }
        }

        public bool Centered {
            get { return layout.centered; }
            set { layout.centered = value; }
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
