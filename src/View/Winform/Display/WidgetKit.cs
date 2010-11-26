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

using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Limaki.Winform;
using Limaki.Winform.Widgets;

namespace Limaki.Winform.Displays {
    public class WidgetKit : DisplayKit<Scene> {
        
        protected PainterFactory painterFactory = new PainterFactory();

        protected IStyleSheet _styleSheet = null;
        public virtual IStyleSheet StyleSheet {
            get {
                if (_styleSheet == null) {
                    IStyle style = Limaki.Drawing.StyleSheet.SystemStyle;
                    style.FillColor = Color.WhiteSmoke;
                    style.PenColor = Color.Teal;//Color.Gainsboro;
                    style.Pen.Color = style.PenColor;
                    style.Font = new Font (style.Font.FontFamily, 10);
                    _styleSheet = new StyleSheet("Default",style);
                    _styleSheet[StyleNames.SelectedStyle].FillColor = Color.Teal;
                    _styleSheet[StyleNames.SelectedStyle].TextColor = Color.WhiteSmoke;

                }
                return _styleSheet;
            }
            set { _styleSheet = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get {
                if (_layout == null) {
                    _layout =
                        new GraphLayout<Scene, IWidget>(this.dataHandler, StyleSheet);
                    _layout.PainterFactory = this.painterFactory;
                }
                return _layout;
            }
            set { _layout = value; }
        }

        public override Layer<Scene> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
            WidgetLayer layer = new WidgetLayer(zoomTarget, scrollTarget);
            layer.Layout = this.Layout;
            return layer;
        }
        
        public override SelectionBase SelectAction(IWinControl control, ICamera camera) {
            WidgetMultiSelector result = new WidgetMultiSelector(dataHandler,control, camera);
            //result.RenderType = RenderType.DrawAndFill;
            result.ShapeDataType = typeof (Rectangle);
            result.PainterFactory = painterFactory;
            result.ShapeFactory = new ShapeFactory();
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            //return new ResizeableSelector(control, camera);
            //return new RectangleSelector(control, camera);
            return result;
        }

        public virtual WidgetChanger WidgetChanger(IWinControl control, ICamera camera) {
            WidgetChanger result = new WidgetChanger(control, camera);
            result.SceneHandler = this.dataHandler;
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle]; 
            return result;
        }

        public virtual WidgetSelector WidgetSelector(IWinControl control, ICamera camera) {
            WidgetSelector result = new WidgetSelector(dataHandler, control, camera);
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual EdgeWidgetChanger LinkWidgetChanger(IWinControl control, ICamera camera) {
            EdgeWidgetChanger result = new EdgeWidgetChanger(dataHandler, control, camera);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual AddEdgeAction AddLinkAction(IWinControl control, ICamera camera) {
            AddEdgeAction result = new AddEdgeAction(dataHandler, control, camera);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize; 
            result.HitSize = this.HitSize;
            return result;
        }

        
        public virtual ILayoutControler CommandsAction(IControl control, IScrollTarget scrollTarget,ICamera camera) {
            ILayoutControler<Scene, IWidget> layoutControler = 
                new SceneControler<Scene, IWidget>(this.dataHandler, control,scrollTarget, camera, Layout);
            return layoutControler;
        }

        public virtual AddWidgetAction AddWidgetAction(IWinControl control, ICamera camera) {
            AddWidgetAction result = new AddWidgetAction(control, camera);
            result.SceneHandler = this.dataHandler;
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            return result;
        }

        public virtual WidgetTextEditor WidgetEditor(ContainerControl control, ICamera camera) {
            WidgetTextEditor result = new WidgetTextEditor(dataHandler, control, camera);
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            return result;
        }

        public virtual WidgetDeleter WidgetDeleter(IControl control, ICamera camera) {
            WidgetDeleter result = new WidgetDeleter(dataHandler, control, camera);
            return result;
        }

        public virtual WidgetDragDrop WidgetDragDrop( IDragDopControl control, ICamera camera ) {
            WidgetDragDrop result = new WidgetDragDrop(dataHandler, control, camera);
            return result;
        }

        public virtual WidgetFolding WidgetFolding(IDragDopControl control) {
            WidgetFolding result = new WidgetFolding (dataHandler, control, Layout);
            return result;
        }
    }
}