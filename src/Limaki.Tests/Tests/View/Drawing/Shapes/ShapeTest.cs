using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.UnitTest;
using NUnit.Framework;

namespace Limaki.Tests.View.Drawing.Shapes {
    public class ShapeTest:DomainTest {

        public void TestClone(IShape shape) {
            IShape clone = (IShape)shape.Clone();
            TestShape (clone, shape.Location, shape.Size);
        }

        public void TestShape(IShape shape, PointI location, SizeI size) {
            Assert.AreEqual (shape.Location, location, "wrong location");
            Assert.AreEqual (shape.Size, size, "wrong size");
        }
        
        [Test]
        public void CloneTest() {
            CloneTest<RectangleShape> ();
            CloneTest<RoundedRectangleShape> ();
            CloneTest<VectorShape> ();
        }

        public void CloneTest<T>() where T : IShape, new() {
            ReportSummary ("CloneTest<"+typeof(T).Name+">");
            PointI locaction = new PointI (5, 10);
            SizeI size = new SizeI (20, 40);

            IShape shape = new T ();
            shape.Location = locaction;
            shape.Size = size;

            TestShape (shape, locaction, size);
            TestClone (shape);
                    
        }
    }
}
