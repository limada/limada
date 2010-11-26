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

using System.ComponentModel;
using Limaki.Drawing;

namespace Limaki.Widgets.Layout {
    public class LayoutProperties {
        private ILayout _layout;
        private GraphLayout<Scene, IWidget> layout;

        [Browsable(false)]
#if !SILVERLIGHT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
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

        
        public SizeI Distance {
            get { return layout.Distance; }
            set { layout.Distance = value; }
        }

    }
}