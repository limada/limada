using System;
using System.Collections.Generic;
using Limaki.Drawing;
using System.Linq;

namespace Limaki.Drawing {
    /// <summary>
    /// This class adopted from: AForge.Math.Geometry
    /// 
    /// Graham scan algorithm for finding convex hull.
    /// </summary>
    /// 
    /// <remarks><para>The class implements
    /// <a href="http://en.wikipedia.org/wiki/Graham_scan">Graham scan</a> algorithm for finding convex hull
    /// of a given set of points.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // generate some random points
    /// Random rand = new Random( );
    /// List&lt;PointI&gt; points = new List&lt;PointI&gt;( );
    /// 
    /// for ( int i = 0; i &lt; 10; i++ )
    /// {
    ///     points.Add( new PointI(
    ///            rand.Next( 200 ) - 100,
    ///            rand.Next( 200 ) - 100 ) );
    /// }
    /// 
    /// // find the convex hull
    /// IConvexHullAlgorithm hullFinder = new GrahamConvexHull( );
    /// List&lt;PointI&gt; hull = hullFinder.FindHull( points );
    /// </code>
    /// </remarks>
    /// 
    public class GrahamConvexHull {
        /// <summary>
        /// Find convex hull for the given set of points.
        /// </summary>
        /// 
        /// <param name="points">Set of points to search convex hull for.</param>
        /// 
        /// <returns>Returns set of points, which form a convex hull for the given <paramref name="points"/>.
        /// The first point in the list is the point with lowest X coordinate (and with lowest Y if there are
        /// several points with the same X value). Points are provided in counter clockwise order
        /// (<a href="http://en.wikipedia.org/wiki/Cartesian_coordinate_system">Cartesian
        /// coordinate system</a>).</returns>
        /// 
        public IEnumerable<PointI> FindHull(IEnumerable<PointI> points) {
            List<PointToProcess> pointsToProcess = new List<PointToProcess>();
            int firstCornerIndex = 0;
            PointToProcess firstCorner = null;
            int fi = 0;
            var firstPoint = new PointI();
            bool newPoly = true;
            int polygonCount = 0;
            foreach (var point in points) {
                // convert input points to points
                var pp = new PointToProcess (point);
                pointsToProcess.Add(pp);
                
                if (newPoly) {
                    firstPoint = point;
                    newPoly = false;
                } else {
                    if (point == firstPoint) {
                        polygonCount++;
                        newPoly = true;
                    }
                }
                // find a point, with lowest X and lowest Y    
                if ((firstCorner == null) ||
                    (pp.X < firstCorner.X) ||
                    ((pp.X == firstCorner.X) && (pp.Y < firstCorner.Y))) {
                    firstCorner = pp;
                    firstCornerIndex = fi;
                }
                fi++;
            }
            
            if (pointsToProcess.Count == 0)
                return points;

            // thats not true! a convexhull can be different from the polyon!
            //if (polygonCount <= 0)
            //    return points;

            // remove the just found point
            pointsToProcess.RemoveAt(firstCornerIndex);

            // find K (tangent of line's angle) and distance to the first corner
            foreach(var point in pointsToProcess){
                int dx = point.X - firstCorner.X;
                int dy = point.Y - firstCorner.Y;

                // don't need square root, since it is not important in our case
                point.Distance = dx * dx + dy * dy;
                // tangent of lines angle
                point.K = (dx == 0) ? double.PositiveInfinity : (double)dy / dx;
            }

            // sort point by angle and distance
            pointsToProcess.Sort();

            List<PointToProcess> convexHullTemp = new List<PointToProcess>();

            // add first corner, which is always on the hull
            convexHullTemp.Add(firstCorner);
            // add another point, which forms a line with lowest slope
            convexHullTemp.Add(pointsToProcess[0]);
            pointsToProcess.RemoveAt(0); //Lytico: error here: was: points.removeat(0);

            PointToProcess lastPoint = convexHullTemp[1];
            PointToProcess prevPoint = convexHullTemp[0];

            while (pointsToProcess.Count != 0) {
                PointToProcess newPoint = pointsToProcess[0];

                // skip any point, which has the same slope as the last one
                if (newPoint.K == lastPoint.K) {
                    pointsToProcess.RemoveAt(0);
                    continue;
                }

                // check if current point is on the left side from two last points
                if ((newPoint.X - prevPoint.X) * (lastPoint.Y - newPoint.Y) - (lastPoint.X - newPoint.X) * (newPoint.Y - prevPoint.Y) < 0) {
                    // add the point to the hull
                    convexHullTemp.Add(newPoint);
                    // and remove it from the list of points to process
                    pointsToProcess.RemoveAt(0);

                    prevPoint = lastPoint;
                    lastPoint = newPoint;
                } else {
                    // remove the last point from the hull
                    convexHullTemp.RemoveAt(convexHullTemp.Count - 1);

                    lastPoint = prevPoint;
                    prevPoint = convexHullTemp[convexHullTemp.Count - 2];
                }
            }

            // convert points back
            List<PointI> convexHull = new List<PointI>(convexHullTemp.Count);

            foreach (PointToProcess pt in convexHullTemp) {
                convexHull.Add(new PointI(pt.X,pt.Y));
            }

            return convexHull;
            //convexHullTemp.Select<PointToProcess,PointI>(pt=>pt.ToPoint());
        }

        // Internal comparer for sorting points
        private class PointToProcess : IComparable {
            public int X;
            public int Y;
            public double K;
            public double Distance;

            public PointToProcess(PointI point) {
                X = point.X;
                Y = point.Y;

                K = 0;
                Distance = 0;
            }

            public int CompareTo(object obj) {
                PointToProcess another = (PointToProcess)obj;

                return (K < another.K) ? -1 : (K > another.K) ? 1 :
                                                                      ((Distance > another.Distance) ? -1 : (Distance < another.Distance) ? 1 : 0);
            }
        }

    }
}