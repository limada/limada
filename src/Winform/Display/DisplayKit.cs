/*
 * Limaki 
 * Version 0.07
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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Winform;

namespace Limaki.Winform.Displays {
    public abstract class DisplayKit<TData> {

        public Handler<TData> dataHandler;

        private int _hitSize = 6;
        public int HitSize { get { return _hitSize; } set { _hitSize = value; } }

        private int _gripSize = 4;
        public int GripSize { get { return _gripSize; } set { _gripSize = value; } }


        # region Factory-Methods

        public abstract SelectionBase SelectAction(IWinControl control, ICamera camera);

        public abstract Layer<TData> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget);
        
        # endregion
    }
}