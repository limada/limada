/*
 * Limaki 
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;

using Limaki.Common;
using Limaki.Drawing;
using Limaki.UnitTest;
using NUnit.Framework;

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

        public PointI[] PolygonHull_AtanSinCos ( Vector _data, int width ) {
            PointI[] result = new PointI[4];
            int x1 = _data.Start.X;
            int x2 = _data.End.X;
            int y1 = _data.Start.Y;
            int y2 = _data.End.Y;
            if ( ( x2 - x1 ) != 0.0 ) {
                double theta = Math.Atan(( y2 - y1 ) / ( x2 - x1 ));
                int dx = (int) ( Math.Sin(theta) * width );
                int dy = (int) ( Math.Cos(theta) * width );
                result[0] = new PointI(x1 - dx, y1 + dy);
                result[1] = new PointI(x1 + dx, y1 - dy);
                result[2] = new PointI(x2 + dx, y2 - dy);
                result[3] = new PointI(x2 - dx, y2 + dy);

            } else {
                // special case, vertical line
                result[0] = new PointI(x1 - width, y1);
                result[1] = new PointI(x1 + width, y1);
                result[2] = new PointI(x2 + width, y2);
                result[3] = new PointI(x2 - width, y2);

            }
            return result;
        }

        public virtual PointI[] PolygonHull_Transform ( Vector _data, int delta ) {
            PointI[] line = null;

            //if (_data.Start.X <= _data.End.X && _data.Start.Y <= _data.End.Y) {
            if ( _data.Start.X <= _data.End.X ) {
                line = new PointI[] { _data.Start, _data.End };
            } else {
                line = new PointI[] { _data.End, _data.Start };
            }


            Matrice lineMatrice = new Matrice();
            float angle = (float) Vector.Angle(_data);
            lineMatrice.Rotate(-angle);
            lineMatrice.TransformPoints(line);
            PointI[] poly = new PointI[] {
                                                 new PointI (line[0].X - delta, line[0].Y - delta),
                                                 new PointI(line[1].X + delta, line[1].Y - delta),
                                                 new PointI (line[1].X + delta, line[1].Y + delta),
                                                 new PointI (line[0].X - delta, line[0].Y + delta)
                                             };
            lineMatrice.Reset();
            lineMatrice.Rotate(angle);
            lineMatrice.TransformPoints(poly);
            return poly;

        }

        public virtual PointI[] PolygonHullSqareRoot (Vector _data, int delta ) {
            // get it near:
            int startX = _data.Start.X;
            int startY = _data.Start.Y;
            int endX = _data.End.X;
            int endY = _data.End.Y;

            int deltaSinusAlpha = 0;
            int deltaSinusBeta = 0;
            
            int a = endX - startX;
            int b = endY - startY;
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
                double c = Math.Sqrt (( a*a + b*b ));

                // calculation of Sinus Alpha and Beta, factorized with delta:
                deltaSinusAlpha = (int) ( delta*( a/c ) );
                deltaSinusBeta = (int) ( delta*( b/c ) );
            }

            // extending the original line to make it longer:
            startX = startX - deltaSinusAlpha;
            startY = startY - deltaSinusBeta;
            endX = endX + deltaSinusAlpha;
            endY = endY + deltaSinusBeta;


            return new PointI[] {
                new PointI (startX - deltaSinusBeta, startY + deltaSinusAlpha),
                new PointI (startX + deltaSinusBeta, startY - deltaSinusAlpha),
                new PointI(endX + deltaSinusBeta, endY - deltaSinusAlpha),
                new PointI (endX - deltaSinusBeta, endY + deltaSinusAlpha)
                                             };
        }

        void PaintWidenPolygon (Algo algo, PointI start, SizeI size, int i ) {
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
                PointI start = new PointI(200, 100);
                SizeI size = new SizeI(0, -100);
                SizeI distance = new SizeI(30, 30);
                PaintWidenPolygon(algo, start, size, 1);

                start += distance;
                size = new SizeI(0, 100);
                PaintWidenPolygon(algo, start, size, 2);

                start += distance;
                size = new SizeI(100, 0);
                PaintWidenPolygon(algo, start, size, 3);

                start += distance;
                size = new SizeI(-100, 0);
                PaintWidenPolygon(algo, start, size, 4);

                // second

                start += distance;
                size = new SizeI(10, -100);
                PaintWidenPolygon(algo, start, size, 5);

                start += distance;
                size = new SizeI(10, 100);
                PaintWidenPolygon(algo, start, size, 6);

                start += distance;
                size = new SizeI(100, 10);
                PaintWidenPolygon(algo, start, size, 7);

                start += distance;
                size = new SizeI(-100, 10);
                PaintWidenPolygon(algo, start, size, 8);

                // third

                start += distance;
                size = new SizeI(-10, -100);
                PaintWidenPolygon(algo, start, size, 9);

                start += distance;
                size = new SizeI(-10, 100);
                PaintWidenPolygon(algo, start, size, 10);

                start += distance;
                size = new SizeI(100, -80);
                PaintWidenPolygon(algo, start, size, 11);

                start += distance;
                size = new SizeI(-100, -10);
                PaintWidenPolygon(algo, start, size, 12);
            }
            ticker.Stop();
            this.ReportDetail(algo.ToString() + "\t" + ticker.ElapsedInSec());

        }

        private int count = 1000000;
        [Test]
        public void Test() {
            this.ReportDetail("PolygonHullTest");
            TestCases(Algo.Transform);
            TestCases(Algo.SqareRoot);
            TestCases(Algo.AtanSinCos);

            

        }
    }
}
