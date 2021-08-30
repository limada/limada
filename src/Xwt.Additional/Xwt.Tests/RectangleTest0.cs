using System.Diagnostics;
using NUnit.Framework;
using Xwt;
using System;

namespace Xwt.Tests {
    // see also: 
    // https://github.com/mono/mono/blob/master/mcs/class/WindowsBase/Test/System.Windows/RectTest.cs
    // https://github.com/mono/mono/blob/master/mcs/class/System.Drawing/Test/System.Drawing/TestRectangleF.cs
    // https://github.com/mono/mono/blob/master/mcs/class/WindowsBase/System.Windows/Rect.cs

    [TestFixture]
    public class RectangleTest0  {

        public static Rectangle IntersectMonoWindowBase(Rectangle r1, Rectangle r2) {
            var _x = Math.Max(r1.X, r2.X);
            var _y = Math.Max(r1.Y, r2.Y);
            var _width = Math.Min(r1.Right, r2.Right) - _x;
            var _height = Math.Min(r1.Bottom, r2.Bottom) - _y;

            if (_width < 0 || _height < 0) {
                return Rectangle.Zero;
            }
            return new Rectangle(_x, _y, _width, _height);
        }

        public static Rectangle IntersectMonoSystemDrawing(Rectangle r1, Rectangle r2) {
            Func<bool> intersectsWithInclusive = () => !((r1.X > r2.Right) || (r1.Right < r2.Left) ||
                                             (r1.Y > r2.Bottom) || (r2.Bottom < r2.Top));

            if (!intersectsWithInclusive())
                return Rectangle.Zero;
            return Rectangle.FromLTRB(Math.Max(r1.Left, r2.Left),
                     Math.Max(r1.Top, r2.Top),
                     Math.Min(r1.Right, r2.Right),
                     Math.Min(r1.Bottom, r2.Bottom));
        }

        void TraceRect(Rectangle r) {
            Trace.WriteLine(string.Format("left:{0} top:{1} width:{2} heigth:{3} right:{4} bottom:{5}",
                r.Left,r.Top,r.Width,r.Height,r.Right,r.Bottom));
        }

        void TestLTRB(Rectangle rectangle, double l, double t, double r, double b) {
            TraceRect(rectangle);

            Assert.AreEqual(l, rectangle.Left);
            Assert.AreEqual(t, rectangle.Top);

            Assert.AreEqual(l, rectangle.X);
            Assert.AreEqual(t, rectangle.Y);

            Assert.AreEqual(r, rectangle.Right);
            Assert.AreEqual(b, rectangle.Bottom);

            Assert.AreEqual(r - l, rectangle.Width);
            Assert.AreEqual(b - t, rectangle.Height);

            Assert.AreEqual(new Size(r - l, b - t), rectangle.Size);
        }

        [Test]
        public void TestBottomRight() {
            
            TestLTRB( new Rectangle(), 0d, 0d, 0d, 0d);

        }

        [Test]
        public void TestFromLTRB() {
            var l = 0.1d;
            var t = 0.1d;
            var r = 0.5d;
            var b = 0.5d;

            var rectangle = Rectangle.FromLTRB(l, t, r, b);

            TestLTRB(rectangle, l, t, r, b);


        }
    }
}