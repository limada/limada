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

using Limaki.Drawing;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Limaki.Widgets.Paint;
using Limaki.Widgets.UI;
using Limaki.Winform.Widgets;

namespace Limaki.Winform.Displays {
    public class WidgetKit : DisplayKit<Scene> {
        private IPainterFactory _painterFactory = null;
        private IShapeFactory _shapeFactory = null;

        public IPainterFactory PainterFactory {
            get { return _painterFactory; }
            set { _painterFactory = value; }
        }

        public IShapeFactory ShapeFactory {
            get { return _shapeFactory; }
            set { _shapeFactory = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get {
                if (_layout == null) {
                    _layout = new GraphLayout<Scene, IWidget>(this.DataHandler, StyleSheet);
                    _layout.PainterFactory = this.PainterFactory;
                    _layout.ShapeFactory = this.ShapeFactory;
                }
                return _layout;
            }
            set { _layout = value; }
        }

        public override IStyleSheet StyleSheet {
            get {
                return base.StyleSheet;
            }
            set {
                base.StyleSheet = value;
                if (_layout != null) {
                    _layout.StyleSheet = value;
                }
            }
        }

        //public override Layer<Scene> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
        //    GDIWidgetLayer layer = new GDIWidgetLayer(zoomTarget, scrollTarget);
        //    layer.Layout = this.Layout;
        //    return layer;
        //}
        public override Layer<Scene> Layer(ICamera camera) {
            GDIWidgetLayer layer = new GDIWidgetLayer(camera);
            layer.Layout = this.Layout;
            return layer;
        }

        public override SelectionBase SelectAction(IWinControl control, ICamera camera) {
            WidgetMultiSelector result = new WidgetMultiSelector(DataHandler,control, camera);
            //result.RenderType = RenderType.DrawAndFill;
            result.ShapeDataType = typeof (RectangleI);
            result.Style = StyleSheet[StyleNames.ResizerToolStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual WidgetChanger WidgetChanger(IWinControl control, ICamera camera) {
            WidgetChanger result = new WidgetChanger(this.DataHandler,control, camera);
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.ResizerToolStyle]; 
            return result;
        }

        public virtual WidgetSelector WidgetSelector(IWinControl control, ICamera camera) {
            WidgetSelector result = new WidgetSelector(DataHandler, control, camera);
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual EdgeWidgetChanger LinkWidgetChanger(IWinControl control, ICamera camera) {
            EdgeWidgetChanger result = new EdgeWidgetChanger(DataHandler, control, camera);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual AddEdgeAction AddLinkAction(IWinControl control, ICamera camera) {
            AddEdgeAction result = new AddEdgeAction(DataHandler, control, camera,this.Layout);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize; 
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual ILayoutControler LayoutControler(IGDIControl control, IScrollTarget scrollTarget,ICamera camera) {
            ILayoutControler<Scene, IWidget> layoutControler = 
                new GDISceneControler<Scene, IWidget>(this.DataHandler, control,scrollTarget, camera, Layout);
            return layoutControler;
        }

        public virtual AddWidgetAction AddWidgetAction(IWinControl control, ICamera camera) {
            AddWidgetAction result = new AddWidgetAction(this.DataHandler, control, camera,Layout );
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            return result;
        }

        public virtual WidgetTextEditor WidgetEditor(DisplayBase<Scene> control, ICamera camera) {
            WidgetTextEditor result = new WidgetTextEditor(DataHandler, control, control, camera, Layout);
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual WidgetDeleter WidgetDeleter(IControl control, ICamera camera) {
            WidgetDeleter result = new WidgetDeleter(DataHandler, control, camera);
            return result;
        }

        public virtual WidgetDragDrop WidgetDragDrop( IDragDopControl control, ICamera camera ) {
            WidgetDragDrop result = new WidgetDragDrop(DataHandler, control, camera, Layout);
            return result;
        }

        public virtual WidgetFolding WidgetFolding(IDragDopControl control) {
            WidgetFolding result = new WidgetFolding (DataHandler, control, Layout);
            return result;
        }
    }
}
