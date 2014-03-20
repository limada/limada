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
 * http://www.limada.org
 * 
 */


using System;
using Limaki.Drawing;

namespace Limaki.View.Viz.Rendering {

    public abstract class ContentRenderer<T> : IContentRenderer<T> {

        public virtual Func<ICamera> Camera {get;set;}

        public abstract void Render(T data, IRenderEventArgs e);


    }
}