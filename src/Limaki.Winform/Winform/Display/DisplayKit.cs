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

using System;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Displays {
    public abstract class DisplayKit<TData> {

        public Func<TData> DataHandler;

        private int _hitSize = 6;
        public int HitSize { get { return _hitSize; } set { _hitSize = value; } }

        private int _gripSize = 4;
        public int GripSize { get { return _gripSize; } set { _gripSize = value; } }


        protected IStyleSheet _styleSheet = null;

        /// <summary>
        /// remark: StyleSheet is set in the DisplayContextProcessor
        /// </summary>
        public virtual IStyleSheet StyleSheet {
            get {
                if (_styleSheet == null) {
                    return new StyleSheet ("");
                }
                return _styleSheet;
            }
            set { _styleSheet = value; }
        }

        # region Factory-Methods

        public abstract SelectionBase SelectAction(IWinControl control, ICamera camera);

        //public abstract Layer<TData> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget);
        public abstract Layer<TData> Layer(ICamera camera);

        # endregion
    }
}