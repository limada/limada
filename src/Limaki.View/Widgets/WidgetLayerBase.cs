/*
 * Limaki 
 * Version 0.08
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

using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Widgets {
    public abstract class WidgetLayerBase : Layer<Scene> {
        public WidgetLayerBase(IZoomTarget zoomTarget, IScrollTarget scrollTarget)
            : base(zoomTarget, scrollTarget) {
            Priority = ActionPriorities.LayerPriority;
        }

        public WidgetLayerBase(ICamera camera) : base(camera) { }

        public override Scene Data {
            get { return _data; }
            set {
                bool refresh = value != _data;
                if (refresh) {
                    DisposeData();
                    if (value != null) {
                        isDataOwner = _data != value;
                        _data = value;
                    }

                    DataChanged();
                }
            }
        }

        public override SizeI Size {
            get {
                if (Data != null) {
                    return Data.Shape.Size;
                } else {
                    return SizeI.Empty;
                }
            }
            set {
                base.Size = value;
            }
        }

        public override void DataChanged() { }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

    }
}