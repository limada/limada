/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public static class Geom {

        #region Rectangle

        public static Rectangle Inflate (this Rectangle bounds, Spacing padding) {
            return
                new Rectangle (
                    bounds.X - padding.Left,
                    bounds.Y - padding.Top,
                    bounds.Width + padding.Right + padding.Left,
                    bounds.Height + padding.Bottom + padding.Top);

        }

        public static Rectangle Padding (this Rectangle bounds, Spacing padding) {
            return
                new Rectangle (
                    bounds.X + padding.Left,
                    bounds.Y + padding.Top,
                    bounds.Width - padding.Right -  padding.Left,
                    bounds.Height - padding.Bottom - padding.Top);

        }

        #endregion

    }
}
