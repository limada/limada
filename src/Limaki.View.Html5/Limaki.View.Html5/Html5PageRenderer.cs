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
using Xwt.Html5.Backend;
using Limaki.View.Html5;
using Xwt;

namespace Limaki.View.Html5 {

    public class Html5PageRenderer {

        Action<Context> paint = null;
        public void Paint (Action<Context> draw) {
            paint += draw;
        }

        public void ClearPaint () { paint = null; }

        public Size PageSize { get; set; }

        public string Page () {
            if (paint == null)
                paint = c => { };
            if (PageSize == Size.Zero)
                PageSize = new Size(1000, 1000);
            return Page(paint, PageSize);
        }

        public string Page (Action<Context> draw) {
            Paint(draw);
            return Page();
        }

        public bool CanvasBorder { get; set; }

        public string Page (Action<Context> draw, Size canvasSize) {

            var htmlCanvas = new Html5Canvas { ElementId = "myCanvas" };

            draw(htmlCanvas.Context);

            var result = string.Format(@"
<html>
    <head>     
        <meta http-equiv=""X-UA-Compatible"" content=""IE=Edge""/>
        <style> 
            body {{ 
                    margin: 0px; 
                    padding: 0px;
                }}" 

+  (CanvasBorder ? @"
            #{1} {{border: 1px solid #9C9898;}}" : "") +
                                                 
@"      </style>
        <script> 
            window.onload = {0} 
        </script>
    </head>
    <body>
        <canvas id=""{1}"" width=""{2}"" height=""{3}""></canvas>
    </body>
</html>",
                                    htmlCanvas.Html(),
                                    htmlCanvas.ElementId,
                                    canvasSize.Width.ToHtml(),
                                    canvasSize.Height.ToHtml()

                );
            return result;
        }

    }
}