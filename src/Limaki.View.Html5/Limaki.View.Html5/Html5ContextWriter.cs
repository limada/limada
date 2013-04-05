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


using System.IO;
using Limaki.View.Visualizers;
using Xwt;
using Xwt.Html5.Backend;

namespace Limaki.View.Html5 {

    public class Html5ContextWriter : ContextWriter {

        public bool CanvasBorder { get; set; }

        protected string CanvasPage {get;set;}

        public override void Write (Stream stream) {
            using (var writer = new StreamWriter(stream)) {
                Flush();
                writer.Write(CanvasPage);
            }
        }

        public override void Flush () {
            var htmlCanvas = new Html5Canvas { ElementId = "myCanvas" };

            if (paintStack == null)
                paintStack = c => { };
            if (CanvasSize == Size.Zero)
                CanvasSize = new Size(1000, 1000);

            paintStack(htmlCanvas.Context);

            CanvasPage = string.Format(@"
<html>
    <head>     
        <meta http-equiv=""X-UA-Compatible"" content=""IE=Edge""/>
        <style> 
            body {{ 
                    margin: 0px; 
                    padding: 0px;
                }}"

                                  + (CanvasBorder ? @"
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
                                  CanvasSize.Width.ToHtml(),
                                  CanvasSize.Height.ToHtml()

                );
        }

        public override void ClearPaint () {
            base.ClearPaint();
            CanvasPage = null;
        }

        
    }
}