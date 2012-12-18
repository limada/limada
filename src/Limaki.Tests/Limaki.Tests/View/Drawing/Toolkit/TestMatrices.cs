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
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.UnitTest;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.Drawing.Toolkit {
    public class TestMatrices:DomainTest {
        const string TickerName = "MatrixTestTime";
        public override void Setup() {
            base.Setup();
            this.Tickers.Add (TickerName, new Ticker ());
        }
        public class TestData {
            public int pointCount = 5000;
            public Point[] Points;
            public Point[] PointsF;
            public List<MatrixOrder> matrixOrder = new List<MatrixOrder> ();
            public float angle = 23.45f;
            public Point rotatePoint = new Point ();
            public float scaleX = 0.003f;
            public float scaleY = 23f;
            public float shearX = 0.345F;
            public float shearY = 0.234F;
            public float offsetX = 13.3f;
            public float offsetY = 10.2f;
            
            public TestData() {
                Points = new Point[pointCount];
                for (int i= 0; i<pointCount;i++) {
                    Points[i] = new Point (i, pointCount - i);
                }
                PointsF = new Point[pointCount];
                for (int i = 0; i < pointCount; i++) {
                    PointsF[i] = new Point(i, pointCount - i);
                }
                matrixOrder.Add (MatrixOrder.Append);
                matrixOrder.Add (MatrixOrder.Prepend);
            }
        }

        private TestData data = new TestData ();
        
        public void MatriceCalculate() {
            using (Matrice matrix = new Matrice()) {

                matrix.Invert();

                using (Matrice matrix2 = new Matrice (2, 2, 3, 3, 1, 1)) {
                    matrix.Multiply (matrix2);
                    matrix.Multiply (matrix2, MatrixOrder.Append);

                    matrix.Multiply (matrix2);
                    matrix.Multiply (matrix2, MatrixOrder.Prepend);
                }

                matrix.Rotate (data.angle);
                matrix.Rotate (data.angle, MatrixOrder.Append);
                matrix.RotateAt (data.angle, data.rotatePoint);
                matrix.RotateAt (data.angle, data.rotatePoint, MatrixOrder.Append);
                matrix.Scale (data.scaleX, data.scaleY);
                matrix.Scale (data.scaleX, data.scaleY, MatrixOrder.Prepend);
                matrix.Shear (data.shearX, data.shearY);
                matrix.Shear (data.shearX, data.shearY, MatrixOrder.Append);
                matrix.Transform (data.Points);
                matrix.Transform (data.PointsF);
                matrix.TransformVectors (data.Points);
                matrix.TransformVectors (data.PointsF);
                matrix.Translate (data.offsetX, data.offsetY);
                matrix.Translate (data.offsetX, data.offsetY, MatrixOrder.Prepend);
            }
        }
        //[Test]
        //public void MSMatrixCalculate() {
        //    using (Matrix matrix = new Matrix()) {

        //        matrix.Invert();

        //        using (Matrix matrix2 =
        //            new Matrix(2, 2, 3, 3, 1, 1)) {
        //            matrix.Multiply (matrix2);
        //            matrix.Multiply (matrix2, MatrixOrder.Append);

        //            matrix.Multiply (matrix2);
        //            matrix.Multiply (matrix2, MatrixOrder.Prepend);
        //        }

        //        matrix.Rotate (data.angle);
        //        matrix.Rotate (data.angle, MatrixOrder.Append);
        //        matrix.RotateAt (data.angle, data.rotatePoint);
        //        matrix.RotateAt (data.angle, data.rotatePoint, MatrixOrder.Append);
        //        matrix.Scale (data.scaleX, data.scaleY);
        //        matrix.Scale (data.scaleX, data.scaleY, MatrixOrder.Prepend);
        //        matrix.Shear (data.shearX, data.shearY);
        //        matrix.Shear (data.shearX, data.shearY, MatrixOrder.Append);
        //        matrix.TransformPoints (data.Points);
        //        matrix.TransformPoints (data.PointsF);
        //        matrix.TransformVectors (data.Points);
        //        matrix.TransformVectors (data.PointsF);
        //        matrix.Translate (data.offsetX, data.offsetY);
        //        matrix.Translate (data.offsetX, data.offsetY, MatrixOrder.Prepend);
        //    }
        //}
        private int testCount = 100;
        [Test]
        public void RunTest() {
            this.Tickers.Start ();
            for (int i = 0; i < testCount;  i++)
                MatriceCalculate();
            this.Tickers.Stop();
            Ticker ticker = Tickers[TickerName];
            ReportDetail ("GNUMatrixCalculate:\t" + ticker.Elapsed.ToString("#,###"));

            this.Tickers.Start();
            //for (int i = 0; i < testCount; i++)
                //MSMatrixCalculate();
            this.Tickers.Stop();
            //ReportDetail("MSMatrixCalculate:\t" + ticker.Elapsed.ToString("#,###"));
        }

    }
}
