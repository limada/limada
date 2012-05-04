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


using System;

using Limaki.Drawing;
using Limaki.UnitTest;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.Drawing {

    public class PolygonHulltest:DomainTest {
        public enum Algo {
            AtanSinCos,
            Transform,
            SqareRoot

        }

        public override void Setup () {
            base.Setup();
            Tickers.Add("",new Ticker());
        }

        public Point[] PolygonHull_AtanSinCos ( Vector _data, int width ) {
            var result = new Point[4];
            var x1 = _data.Start.X;
            var x2 = _data.End.X;
            var y1 = _data.Start.Y;
            var y2 = _data.End.Y;
            if ( ( x2 - x1 ) != 0.0d ) {
                var theta = Math.Atan(( y2 - y1 ) / ( x2 - x1 ));
                var dx = (int) ( Math.Sin(theta) * width );
                var dy = (int) ( Math.Cos(theta) * width );
                result[0] = new Point(x1 - dx, y1 + dy);
                result[1] = new Point(x1 + dx, y1 - dy);
                result[2] = new Point(x2 + dx, y2 - dy);
                result[3] = new Point(x2 - dx, y2 + dy);

            } else {
                // special case, vertical line
                result[0] = new Point(x1 - width, y1);
                result[1] = new Point(x1 + width, y1);
                result[2] = new Point(x2 + width, y2);
                result[3] = new Point(x2 - width, y2);

            }
            return result;
        }

        public virtual Point[] PolygonHull_Transform ( Vector _data, double delta ) {
            Point[] line = null;

            //if (_data.Start.X <= _data.End.X && _data.Start.Y <= _data.End.Y) {
            if ( _data.Start.X <= _data.End.X ) {
                line = new Point[] { _data.Start, _data.End };
            } else {
                line = new Point[] { _data.End, _data.Start };
            }


            var lineMatrice = new Matrice();
            var angle = Vector.Angle(_data);
            lineMatrice.Rotate(-angle);
            lineMatrice.TransformPoints(line);
            var poly = new Point[] {
                new Point (line[0].X - delta, line[0].Y - delta),
                new Point(line[1].X + delta, line[1].Y - delta),
                new Point (line[1].X + delta, line[1].Y + delta),
                new Point (line[0].X - delta, line[0].Y + delta)
            };
            lineMatrice.Reset();
            lineMatrice.Rotate(angle);
            lineMatrice.TransformPoints(poly);
            return poly;

        }

        public virtual Point[] PolygonHullSqareRoot (Vector _data, int delta ) {
            // get it near:
            var startX = _data.Start.X;
            var startY = _data.Start.Y;
            var endX = _data.End.X;
            var endY = _data.End.Y;

            var deltaSinusAlpha = 0d;
            var deltaSinusBeta = 0d;

            var a = endX - startX;
            var b = endY - startY;
            if (a==0) {
                if (b > 0) {
                    deltaSinusBeta = delta; 
                } else {
                    deltaSinusBeta = -delta; 
                }
            } else if (b == 0) {
                if (a > 0) {
                    deltaSinusAlpha = delta;
                } else {
                    deltaSinusAlpha = -delta;
                }
            } else {

                // calculation of the hypotenuse:
                var c = Math.Sqrt (( a*a + b*b ));

                // calculation of Sinus Alpha and Beta, factorized with delta:
                deltaSinusAlpha = (int) ( delta*( a/c ) );
                deltaSinusBeta = (int) ( delta*( b/c ) );
            }

            // extending the original line to make it longer:
            startX = startX - deltaSinusAlpha;
            startY = startY - deltaSinusBeta;
            endX = endX + deltaSinusAlpha;
            endY = endY + deltaSinusBeta;


            return new Point[] {
                new Point (startX - deltaSinusBeta, startY + deltaSinusAlpha),
                new Point (startX + deltaSinusBeta, startY - deltaSinusAlpha),
                new Point(endX + deltaSinusBeta, endY - deltaSinusAlpha),
                new Point (endX - deltaSinusBeta, endY + deltaSinusAlpha)
            };
        }

        void PaintWidenPolygon (Algo algo, Point start, Size size, int i ) {
            if ( algo == Algo.AtanSinCos )
                PolygonHull_AtanSinCos(new Vector(start, start + size), 10);
            else if ( algo == Algo.SqareRoot )
                PolygonHullSqareRoot(new Vector(start, start + size), 10);
            else if ( algo == Algo.Transform )
                PolygonHull_Transform(new Vector(start, start + size), 10);
        }

        public void TestCases ( Algo algo ) {
                        Ticker ticker = new Ticker();
            ticker.Start();
            for ( int i = 0; i < count; i++ ) {
                Point start = new Point(200, 100);
                Size size = new Size(0, -100);
                Size distance = new Size(30, 30);
                PaintWidenPolygon(algo, start, size, 1);

                start += distance;
                size = new Size(0, 100);
                PaintWidenPolygon(algo, start, size, 2);

                start += distance;
                size = new Size(100, 0);
                PaintWidenPolygon(algo, start, size, 3);

                start += distance;
                size = new Size(-100, 0);
                PaintWidenPolygon(algo, start, size, 4);

                // second

                start += distance;
                size = new Size(10, -100);
                PaintWidenPolygon(algo, start, size, 5);

                start += distance;
                size = new Size(10, 100);
                PaintWidenPolygon(algo, start, size, 6);

                start += distance;
                size = new Size(100, 10);
                PaintWidenPolygon(algo, start, size, 7);

                start += distance;
                size = new Size(-100, 10);
                PaintWidenPolygon(algo, start, size, 8);

                // third

                start += distance;
                size = new Size(-10, -100);
                PaintWidenPolygon(algo, start, size, 9);

                start += distance;
                size = new Size(-10, 100);
                PaintWidenPolygon(algo, start, size, 10);

                start += distance;
                size = new Size(100, -80);
                PaintWidenPolygon(algo, start, size, 11);

                start += distance;
                size = new Size(-100, -10);
                PaintWidenPolygon(algo, start, size, 12);
            }
            ticker.Stop();
            this.ReportDetail(algo.ToString() + "\t" + ticker.ElapsedInSec());

        }

        private int count = 100000;
        [Test]
        public void Test() {
            this.ReportDetail("PolygonHullTest");
            TestCases(Algo.Transform);
            TestCases(Algo.SqareRoot);
            TestCases(Algo.AtanSinCos);
        }
    }
}
