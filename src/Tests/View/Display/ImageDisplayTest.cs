using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Displays;
using System.Drawing;

namespace Limaki.Tests.Display {
    public class ImageDisplayTest:DisplayTest<ImageDisplay,Image> {
        public ImageDisplayTest():base() {}
        public ImageDisplayTest(ImageDisplay display) : base(display) {}
    }
}
