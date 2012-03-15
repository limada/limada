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
 * 
 */

using System.Drawing;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.View.Display;
using Limaki.View.GDI.UI;
using Limaki.View.UI;
using Limaki.View.Winform.Display;
using Xwt;
using Point = Xwt.Point;
using Size = Xwt.Size;

namespace Limaki.View.Winform {
    public class WinformImageDisplay:WinformDisplay<Image> {
       
        public override DisplayFactory<Image> CreateDisplayFactory(WinformDisplay<Image> device) {
            var result = new DisplayFactory<Image>();
            
            var deviceInstrumenter = new ImageDeviceComposer();
            deviceInstrumenter.Device = device;
            result.DeviceComposer = deviceInstrumenter;
            
            result.DisplayComposer = new ImageDisplayComposer ();
            //device.AutoScroll = false;
            //device.ScrollBarsVisible = false;

            return result;
        }
    }
   
    public class ImageDeviceComposer : WinformDeviceComposer<Image> {
        public override void Factor(Display<Image> display) {
            base.Factor(display);
            
            this.DataLayer = new GDIImageLayer();
        }
    }

    public class ImageDisplayComposer: DisplayComposer<Image> {
        public override void Compose(Display<Image> display) {
            
            this.DataOrigin = () => { return Point.Zero; };
            this.DataSize = () => {
                var data = display.Data;
                if (data != null)
                    return GDIConverter.Convert(data.Size);
                else
                    return Size.Zero;
            };

            base.Compose(display);

            Compose(display, display.SelectionRenderer);
            Compose(display, display.MoveResizeRenderer);

            var selectAction = new SelectorAction();
            Compose(display, selectAction, true);
            selectAction.ShowGrips = false;
            selectAction.Enabled = false;

            display.SelectAction = selectAction;
            display.EventControler.Add(selectAction);

            display.SelectAction.ShowGrips = true;
            display.SelectAction.Enabled = false;
            display.MouseScrollAction.Enabled = true;
            
        }
    }
}