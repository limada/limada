using System;
using Xwt.Drawing;
using Xwt.Html5.Backend;

namespace Xwt.Html5.TestApp {
    public class Html5Renderer {
        public string RenderPage (Action<Context> draw) {
            return RenderPage (draw, new Size (640, 480));
        }

        public string RenderPage (Action<Context> draw, Size canvasSize) {
            var htmlCanvas = new HtmlCanvas { ElementId = "myCanvas" };
            //htmlCanvas.Context = new ContextBackendHandler ();
            draw (htmlCanvas.Context);
            var result = string.Format (@"
                            <html>
                                <head>     
                                    <meta http-equiv=""X-UA-Compatible"" content=""IE=Edge""/>
                                    <style> 
                                      body {{ 
                                                margin: 0px; 
                                                padding: 0px;
                                           }}
                                      #{0} {{ 
                                                border: 1px solid #9C9898; 
                                           }}
                                    </style>
                                    <script> 
                                        window.onload = {0} 
                                    </script>
                                </head>
                                <body>
                                    <canvas id=""{1}"" width=""{2}"" height=""{3}""></canvas>
                                </body>
                            </html>",
                                    htmlCanvas.Render (),    
                                    htmlCanvas.ElementId,
                                    canvasSize.Width.ToHtml (),
                                    canvasSize.Height.ToHtml()
                                        
                );
            return result;
        }

        internal string RenderPage (Func<Context, Size> VisualScene) {
            throw new NotImplementedException ();
        }
    }
}