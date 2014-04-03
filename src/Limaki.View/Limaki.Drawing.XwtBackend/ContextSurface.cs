/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.Drawing.XwtBackend {

    /// <summary>
    /// a Surface providing a Xwt.Drawing.Context
    /// </summary>
    public class ContextSurface : ISurface {

        public virtual Context Context { get; set; }

        protected Matrix _matrix = null;
        public virtual Matrix Matrix {
            get { return _matrix ?? (_matrix = new Matrix()); }
            set { _matrix = value; }
        }
    }
}