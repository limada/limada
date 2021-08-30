using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Xwt.Drawing;
using Xwt;
using NUnit.Framework;
using Limaki.Tests;
using System.Diagnostics;

namespace Limaki.Playground.View {
    public class DrawingTests : DomainTest {
        [Test]
        public void PrototypeSinCos () {
            var stw = new Stopwatch();
            var matrix = new Matrix();
            var times = 100000;
            double cos = 0;
            double sin = 0;
            double angle = 90;
            double pi180 = Math.PI / 180;
            Action prove = () => {
                stw.Restart();

                var theta = angle * pi180;
                cos = Math.Cos(theta);
                sin = Math.Sin(theta);
                matrix.SetIdentity();
                matrix.Rotate(angle);
                
                stw.Stop();
                ReportDetail("{0}\t{1}\t{2}\t{3}", angle, cos, sin, stw.ElapsedMilliseconds);
                ReportDetail("{{M11={0} M12={1} M21={2} M22={3} OffsetX={4} OffsetY={5}}}",
                              matrix.M11.ToString(),
                              matrix.M12.ToString(),
                              matrix.M21.ToString(),
                              matrix.M22.ToString(),
                              matrix.OffsetX.ToString(),
                              matrix.OffsetY.ToString());
            };
            prove();
            angle = -90;
            prove();
            angle = 180;
            prove();
            angle = -180;
            prove();
            angle = 270;
            prove();
            angle = -270;
            prove();
            angle = 45;
            prove();
            angle = 0;
            prove();
            angle = 360;
            prove();
            angle = -360;
            prove();
        }

        [Test]
        public void TestQuadrantRotation () {
            var matrix = new Matrix();

            Action<double, double, double, double, double> prove = (angle, m11, m12, m21, m22) => {
                matrix.SetIdentity();
                matrix.Rotate(angle);
                Assert.AreEqual(matrix.M11, m11);
                Assert.AreEqual(matrix.M12, m12);
                Assert.AreEqual(matrix.M21, m21);
                Assert.AreEqual(matrix.M22, m22);
            };
            prove(90, 0, 1, -1, 0);
            prove(-90, 0, -1, 1, 0);
            prove(180, -1, 0, 0, -1);
            prove(-180, -1, 0, 0, -1);
            prove(270, 0, -1, 1, 0);
            prove(-270, 0, 1, -1, 0);
            var mi = Matrix.Identity;
            prove(360, mi.M11,mi.M12,mi.M21,mi.M22);
            prove(-360, mi.M11, mi.M12, mi.M21, mi.M22);
            prove(0, mi.M11, mi.M12, mi.M21, mi.M22);

        }
    }
}