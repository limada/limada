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
 */

using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Presenter.Widgets;
using Limaki.Presenter.Widgets.UI;
using Limaki.Presenter.Winform.Widgets;
using Limaki.Widgets;

namespace Limaki.Presenter.Winform.Display {
    public class WinformWidgetDisplay : WinformGraphSceneDisplay<IWidget, IEdgeWidget> {

        public override DisplayFactory<IGraphScene<IWidget, IEdgeWidget>> CreateDisplayFactory(WinformDisplay<IGraphScene<IWidget, IEdgeWidget>> device) {
            var result = new WidgetDisplayFactory();
            var deviceInstrumenter = new WinformWidgetDeviceInstrumenter();
            deviceInstrumenter.Device = device;
            result.DeviceComposer = deviceInstrumenter;
            result.DisplayComposer = new WinformWidgetDisplayIntrumenter ();
            return result;
        }

        
    }

    public class WinformWidgetDisplayIntrumenter:WidgetDisplayComposer {

        public WidgetDragDrop DragDrop { get; set; }
        public override void Factor(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            base.Factor(aDisplay);
            
        }

        public override void Compose(Display<IGraphScene<IWidget, IEdgeWidget>> aDisplay) {
            var display = aDisplay;
            base.Compose(display);
            
            var DragDrop = new WidgetDragDrop(
               this.GraphScene,
               display.Device as IDragDopControl,
               this.Camera(),
               this.Layout());
            DragDrop.Enabled = true;
            display.EventControler.Add(DragDrop);

            var selector = display.EventControler.GetAction<GraphSceneFocusAction<IWidget,IEdgeWidget>> ();
            if (selector != null) {
                var catcher = new DragDropCatcher<GraphSceneFocusAction<IWidget, IEdgeWidget>>(selector, display.Device as IControl);
                display.EventControler.Add (catcher);
            }

            var addEdgeAction = display.EventControler.GetAction<AddEdgeAction>();
            if (addEdgeAction != null) {
                var catcher = new DragDropCatcher<AddEdgeAction>(addEdgeAction, display.Device as IControl);
                display.EventControler.Add(catcher);
            }
            var editor = new WidgetTextEditor (
                this.GraphScene,
                display.Device as ContainerControl,
                display,
                this.Camera (),
                this.Layout()
                );

            display.EventControler.Add(editor);
        }
    }


    public class WinformWidgetDeviceInstrumenter : WinformGraphSceneDeviceComposer<IWidget, IEdgeWidget> {
        public override void Factor(Display<IGraphScene<IWidget, IEdgeWidget>> display) {
            base.Factor(display);
        }

        public override void Compose(Display<IGraphScene<IWidget, IEdgeWidget>> display) {
            base.Compose(display);
           
        }
    }
}