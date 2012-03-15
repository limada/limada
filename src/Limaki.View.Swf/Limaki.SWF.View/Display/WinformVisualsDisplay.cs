/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 */

using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View.Display;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.Display;
using Limaki.View.Visuals.UI;
using Limaki.View.Winform.Visuals;
using Limaki.Visuals;


namespace Limaki.View.Winform.Display {
    public class WinformVisualsDisplay : WinformGraphSceneDisplay<IVisual, IVisualEdge> {

        public override DisplayFactory<IGraphScene<IVisual, IVisualEdge>> CreateDisplayFactory(WinformDisplay<IGraphScene<IVisual, IVisualEdge>> device) {
            var result = new VisualsDisplayFactory();
            var deviceInstrumenter = new WinformVisualsDeviceInstrumenter();
            deviceInstrumenter.Device = device;
            result.DeviceComposer = deviceInstrumenter;
            result.DisplayComposer = new WinformVisualsDisplayIntrumenter ();
            return result;
        }

        
    }

    public class WinformVisualsDisplayIntrumenter:VisualsDisplayComposer {

        public VisualsDragDrop DragDrop { get; set; }
        public override void Factor(Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            base.Factor(aDisplay);
            
        }

        public override void Compose(Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            var display = aDisplay;
            base.Compose(display);
            
            var DragDrop = new VisualsDragDrop(
               this.GraphScene,
               display.Device as IDragDopControl,
               this.Camera(),
               this.Layout());
            DragDrop.Enabled = true;
            display.EventControler.Add(DragDrop);

            var selector = display.EventControler.GetAction<GraphSceneFocusAction<IVisual,IVisualEdge>> ();
            if (selector != null) {
                var catcher = new DragDropCatcher<GraphSceneFocusAction<IVisual, IVisualEdge>>(selector, display.Device as IControl);
                display.EventControler.Add (catcher);
            }

            var addEdgeAction = display.EventControler.GetAction<AddEdgeAction>();
            if (addEdgeAction != null) {
                var catcher = new DragDropCatcher<AddEdgeAction>(addEdgeAction, display.Device as IControl);
                display.EventControler.Add(catcher);
            }
            var editor = new VisualsTextEditor (
                this.GraphScene,
                display.Device as ContainerControl,
                display,
                this.Camera (),
                this.Layout()
                );

            display.EventControler.Add(editor);
        }
    }


    public class WinformVisualsDeviceInstrumenter : WinformGraphSceneDeviceComposer<IVisual, IVisualEdge> {
        public override void Factor(Display<IGraphScene<IVisual, IVisualEdge>> display) {
            base.Factor(display);
        }

        public override void Compose(Display<IGraphScene<IVisual, IVisualEdge>> display) {
            base.Compose(display);
           
        }
    }
}