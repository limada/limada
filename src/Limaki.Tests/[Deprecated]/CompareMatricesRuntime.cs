/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.UnitTest;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;
using Matrice = Limaki.Drawing.Matrice0;
using System.Diagnostics;

namespace Limaki.Tests.Drawing.Toolkit {

    public class CompareMatricesRuntime : DomainTest {
        const string TickerName = "MatrixTestTime";
        public override void Setup () {
            base.Setup();
            this.Tickers.Add(TickerName, new Ticker());
        }
        public class TestData {
            public int pointCount = 5000;
            public Point[] Points;
            public Point[] PointsF;
            public List<MatrixOrder> matrixOrder = new List<MatrixOrder>();
            public float angle = 23.45f;
            public Point rotatePoint = new Point();
            public float scaleX = 0.003f;
            public float scaleY = 23f;
            public float shearX = 0.345F;
            public float shearY = 0.234F;
            public float offsetX = 13.3f;
            public float offsetY = 10.2f;

            public TestData () {
                Points = new Point[pointCount];
                for (int i = 0; i < pointCount; i++) {
                    Points[i] = new Point(i, pointCount - i);
                }
                PointsF = new Point[pointCount];
                for (int i = 0; i < pointCount; i++) {
                    PointsF[i] = new Point(i, pointCount - i);
                }
                matrixOrder.Add(MatrixOrder.Append);
                matrixOrder.Add(MatrixOrder.Prepend);
            }
        }

        private TestData data = new TestData();

        public void MatriceTransform (Matrice matrix) {
            matrix.Transform(data.Points);
            matrix.TransformVectors(data.Points);
            
        }

        public void XwtMatrixTransform (Xwt.Drawing.Matrix matrix) {
            matrix.Transform(data.Points);
            matrix.TransformVector(data.Points);

       
        }

        public void MatriceTransform () {
            using (var matrix = new Matrice()) {
                MatriceTransform(matrix);
            }
        }

        public void MatriceCalculate () {
            using (var matrix = new Matrice()) {

                matrix.Invert();

                using (var matrix2 = new Matrice(2, 2, 3, 3, 1, 1)) {
                    matrix.Multiply(matrix2);
                    matrix.Multiply(matrix2, MatrixOrder.Append);

                    matrix.Multiply(matrix2);
                    matrix.Multiply(matrix2, MatrixOrder.Prepend);
                }

                matrix.Rotate(data.angle);
                matrix.Rotate(data.angle, MatrixOrder.Append);
                matrix.RotateAt(data.angle, data.rotatePoint);
                matrix.RotateAt(data.angle, data.rotatePoint, MatrixOrder.Append);
                matrix.Scale(data.scaleX, data.scaleY);
                matrix.Scale(data.scaleX, data.scaleY, MatrixOrder.Prepend);
                matrix.Shear(data.shearX, data.shearY);
                matrix.Shear(data.shearX, data.shearY, MatrixOrder.Append);

                MatriceTransform(matrix);


                matrix.Translate(data.offsetX, data.offsetY);
                matrix.Translate(data.offsetX, data.offsetY, MatrixOrder.Prepend);
            }
        }

        public void XwtMatrixTransform () {
            XwtMatrixTransform(new Xwt.Drawing.Matrix());
        }
       

        [Test]
        public void XwtMatrixCalculate () {
            var matrix = new Xwt.Drawing.Matrix();

            matrix.Invert();

            var matrix2 = new Xwt.Drawing.Matrix(2, 2, 3, 3, 1, 1);
            Xwt.Drawing.Matrix.Multiply(matrix, matrix2);
            Xwt.Drawing.Matrix.Multiply(matrix, matrix2);

            Xwt.Drawing.Matrix.Multiply(matrix, matrix2);
            Xwt.Drawing.Matrix.Multiply(matrix, matrix2);

            matrix.Rotate(data.angle);
            matrix.RotatePrepend(data.angle);

            matrix.RotateAt(data.angle, data.rotatePoint.X,data.rotatePoint.Y);
            matrix.RotateAtPrepend(data.angle, data.rotatePoint.X,data.rotatePoint.Y);

            matrix.Scale(data.scaleX, data.scaleY);
            matrix.ScalePrepend(data.scaleX, data.scaleY);

            matrix.Skew(data.shearX, data.shearY);
            matrix.SkewPrepend(data.shearX, data.shearY);
            
            XwtMatrixTransform(matrix);

            matrix.Translate(data.offsetX, data.offsetY);
            matrix.TranslatePrepend(data.offsetX, data.offsetY);

        }

        private int testCount = 1000;
        [Test]
        public void CompareCalculate () {
            var sw = new Stopwatch();

            data = new TestData();
            sw.Start();
            for (int i = 0; i < testCount; i++)
                MatriceCalculate();
            sw.Stop();
            ReportDetail("MatriceCalculate:\t" + sw.ElapsedTicks.ToString("#,###"));

            data = new TestData();
            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < testCount; i++)
                XwtMatrixCalculate();
            sw.Stop();
            ReportDetail("XwtMatrixCalculate:\t" + sw.ElapsedTicks.ToString("#,###"));
        }

        [Test]
        public void CompareTransform () {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < testCount; i++)
                MatriceTransform();
            sw.Stop();
            ReportDetail("MatriceCalculate:\t" + sw.ElapsedTicks.ToString("#,###"));

            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < testCount; i++)
                XwtMatrixTransform();
            sw.Stop();
            ReportDetail("XwtMatrixCalculate:\t" + sw.ElapsedTicks.ToString("#,###"));
        }
    }
}
