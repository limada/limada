/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt.Drawing;
using Limaki.View.Html5;

namespace Limaki.View.Html5 {
    public class Html5PageWriter : Html5ContextWriter {
        
        public string Page () {
            return Page(paintStack);
        }

        public string Page (Action<Context> draw) {
            paintStack = null;
            PushPaint(draw);
            Flush();
            return CanvasPage;
        }

        

      
    }
}