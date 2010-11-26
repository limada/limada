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

namespace Limaki.Displays {
    public class WidgetKit : DisplayKit<Scene> {
        
        protected PainterFactory painterFactory = new PainterFactory();

        protected IStyleSheet _styleSheet = null;
        public virtual IStyleSheet StyleSheet {
            get {
                if (_styleSheet == null) {
                    IStyle style = Limaki.Drawing.StyleSheet.SystemStyle;
                    style.FillColor = Color.WhiteSmoke;
                    style.PenColor = Color.Gainsboro;
                    style.Pen.Color = style.PenColor;
                    style.Font = new Font (style.Font.FontFamily, 10);
                    _styleSheet = new StyleSheet("Default",style);

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
                        new SimpleWidgetLayout<Scene, IWidget>(this.dataHandler, StyleSheet);                    
                }
                return _layout;
            }
            set { _layout = value; }
        }

        public override Layer<Scene> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
            WidgetLayer layer = new WidgetLayer(zoomTarget, scrollTarget);
            layer.Layout = this.Layout;
            layer.PainterFactory = this.painterFactory;
            return layer;
        }
        
        public override SelectionBase SelectAction(IWinControl control, ITransformer transformer) {
            SelectionShape result = new SelectionShape(control, transformer);
            //result.RenderType = RenderType.DrawAndFill;
            result.ShapeDataType = typeof (Rectangle);
            result.PainterFactory = painterFactory;
            result.ShapeFactory = new ShapeFactory();
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            //return new ResizeableSelector(control, transformer);
            //return new RectangleSelector(control, transformer);
            return result;
        }

        public virtual WidgetChanger WidgetChanger(IWinControl control, ITransformer transformer) {
            WidgetChanger result = new WidgetChanger(control, transformer);
            result.SceneHandler = this.dataHandler;
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle]; 
            return result;
        }

        public virtual WidgetSelector WidgetSelector(IWinControl control, ITransformer transformer) {
            WidgetSelector result = new WidgetSelector(dataHandler, control, transformer);
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual LinkWidgetChanger LinkWidgetChanger(IWinControl control, ITransformer transformer) {
            LinkWidgetChanger result = new LinkWidgetChanger(dataHandler, control, transformer);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            return result;
        }

        public virtual AddLinkAction AddLinkAction(IWinControl control, ITransformer transformer) {
            AddLinkAction result = new AddLinkAction(dataHandler, control, transformer);
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            result.GripSize = this.GripSize; 
            result.HitSize = this.HitSize;
            return result;
        }

        
        public virtual ICommandAction CommandsAction(IControl control, IScrollTarget scrollTarget,ITransformer transformer) {
            ICommandAction<Scene, IWidget> commandAction = 
                new SceneCommandAction<Scene, IWidget>(this.dataHandler, control,scrollTarget, transformer, Layout);
            return commandAction;
        }

        public virtual AddWidgetAction AddWidgetAction(IWinControl control, ITransformer transformer) {
            AddWidgetAction result = new AddWidgetAction(control, transformer);
            result.SceneHandler = this.dataHandler;
            result.GripSize = this.GripSize;
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            return result;
        }

        public virtual WidgetTextEditor WidgetEditor(ContainerControl control, ITransformer transformer) {
            WidgetTextEditor result = new WidgetTextEditor(dataHandler, control, transformer);
            result.HitSize = this.HitSize;
            result.Style = StyleSheet[StyleNames.DefaultStyle];
            return result;
        }

        public virtual WidgetDeleter WidgetDeleter(IControl control, ITransformer transformer) {
            WidgetDeleter result = new WidgetDeleter(dataHandler, control, transformer);
            return result;
        }

        public virtual WidgetDragDrop WidgetDragDrop( IDragDopControl control, ITransformer transformer ) {
            WidgetDragDrop result = new WidgetDragDrop(dataHandler, control, transformer);
            return result;
        }
    }
}