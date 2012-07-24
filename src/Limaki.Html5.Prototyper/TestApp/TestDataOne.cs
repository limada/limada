using System;
using Xwt.Html5.Backend;
using Xwt.Drawing;

namespace Xwt.Html5.TestApp {
    public class TestDataOne {
        public static string MakeNavString(string html) {
            return Uri.EscapeUriString("data:text/html, " + html);
        }
        public static string SimpleLine {
            get {
                return @"
<html>
  <head>
    <style>
      body {
        margin: 0px;
        padding: 0px;
      }
      #myCanvas {
        border: 1px solid #9C9898;
      }
    </style>
    <script>
      window.onload = function() {
        var canvas = document.getElementById(""myCanvas"");
        var context = canvas.getContext(""2d"");

        context.beginPath();
        context.moveTo(100, 150);
        context.lineTo(450, 50);
        context.stroke();
      };

    </script>
  </head>
  <body>
    <canvas id=""myCanvas"" width=""578"" height=""200""></canvas>
  </body>
</html>
";
            }
        }

     
        
        
    }
}