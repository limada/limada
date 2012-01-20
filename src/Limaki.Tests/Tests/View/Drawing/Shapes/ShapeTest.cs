/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using Limaki.Common;

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

        [Test]
        public void ShapeFactoryTest() {
            var shapeFactory = new Limaki.Drawing.Shapes.ShapeFactory();
            var shapeR = shapeFactory.Shape<Limaki.Drawing.RectangleI>(
                new Limaki.Drawing.PointI(10, 10),
                new Limaki.Drawing.SizeI(20, 100)
                );
            Assert.IsNotNull(shapeR);
            var shape = shapeFactory.Shape(typeof (RectangleI), new Limaki.Drawing.PointI(10, 10),
                                           new Limaki.Drawing.SizeI(20, 100));
            Assert.IsNotNull(shape);
        }

        [Test]
        public void PainterFactoryTest() {
            var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
            var shape = new RectangleShape();
            IPainter _painter = factory.CreatePainter(shape.GetType());
            Assert.IsNotNull(_painter);
            _painter = factory.CreatePainter(typeof(string));
            Assert.IsNotNull(_painter);
        }
    }
}
