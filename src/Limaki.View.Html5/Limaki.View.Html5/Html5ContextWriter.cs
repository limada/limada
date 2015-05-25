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
using System.Collections.Generic;
using System.IO;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Viz.Visualizers;
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
            var saver = this.Switch ();
            try {
                var htmlCanvas = new Html5Canvas { ElementId = "myCanvas" };

                if (paintStack == null)
                    paintStack = c => { };
                if (CanvasSize == Size.Zero)
                    CanvasSize = new Size (1000, 1000);

                paintStack (htmlCanvas.Context);

                CanvasPage = string.Format (@"
<html>
    <head>     
        <meta http-equiv=""X-UA-Compatible"" content=""IE=Edge""/>
        <meta charset=""UTF-8"" />
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
                    htmlCanvas.Html (),
                    htmlCanvas.ElementId,
                    CanvasSize.Width.ToHtml (),
                    CanvasSize.Height.ToHtml ()

                    );
            } finally {
                Restore (saver);
            }
        }

        public override void ClearPaint () {
            base.ClearPaint();
            CanvasPage = null;
        }

        public class EngineSave {
            public Toolkit Toolkit { get; set; }
            public Action Restore { get; set; }
        }

        public override object Switch () {
            var saver = new EngineSave { Toolkit = Toolkit.CurrentEngine };
            if (saver.Toolkit is Html5Engine)
                return saver;

            Toolkit.Engine<Html5Engine>().SetActive();
            var context = Registry.ConcreteContext;
            var ie =  context.Factory.Clazz<IExceptionHandler> ();
            var du = context.Factory.Clazz<IDrawingUtils> ();
            var si = context.Factory.Clazz<IUISystemInformation> ();
            
            context.Factory.Add<IExceptionHandler, Html5ExeptionHandlerBackend> ();
            context.Factory.Add<IDrawingUtils, Html5DrawingUtils> ();
            context.Factory.Add<IUISystemInformation, Html5SystemInformation> ();

            saver.Restore = () => {
                context.Factory.Add (typeof (IExceptionHandler), ie);
                context.Factory.Add (typeof (IDrawingUtils), du);
                context.Factory.Add (typeof (IUISystemInformation), si);
            };
            return saver;
        }

        public override void Restore (object saved) {
            var saver = saved as EngineSave;
            if (!(saver.Toolkit is Html5Engine)) {
                saver.Toolkit.SetActive ();

                if (saver.Restore != null)
                    saver.Restore ();
            }
        }
    }
}