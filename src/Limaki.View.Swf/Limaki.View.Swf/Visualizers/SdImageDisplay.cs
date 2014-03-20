/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Viz;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;
using Xwt;
using Xwt.Backends;
using Xwt.Gdi.Backend;
using Image = System.Drawing.Image;

namespace Limaki.View.Swf.Visualizers {

    [BackendType(typeof(ISdImageDisplayBackend))]
    public class SdImageDisplay : Display<Image> { }
    
    /// <summary>
    /// a display based on System.Drawing.Image
    /// </summary>
    public interface ISdImageDisplayBackend : IDisplayBackend<Image> { }
    
    public class SdImageDisplayFactory:DisplayFactory<Image> {
        public override Display<Image> Create () {
            return new SdImageDisplay();
        }
    }

    public class SdImageDisplayComposer: DisplayComposer<Image> {

        public override void Compose(Display<Image> display) {
            
            this.DataOrigin = () => { return Point.Zero; };
            this.DataSize = () => {
                var data = display.Data;
                if (data != null)
                    return data.Size.ToXwt ();
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