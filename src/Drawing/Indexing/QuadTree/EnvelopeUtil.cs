using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Limaki.Drawing.Indexing.QuadTrees {
    public class Util {
        public static float MinX(RectangleF r) {
            //return Math.Min (r.X, r.Right);
            if (r.Width > 0)
                return r.X;
            else
                return r.X + r.Width;
        }
        public static float MinY(RectangleF r) {
            //return Math.Min(r.Y, r.Bottom);
            if (r.Height > 0)
                return r.Y;
            else
                return r.Y + r.Height;
        }
        public static float MaxX(RectangleF r) {
            //return Math.Max(r.X, r.Right);
            if (r.Width <= 0)
                return r.X;
            else
                return r.X + r.Width;
        }
        public static float MaxY(RectangleF r) {
            //return Math.Max(r.Y, r.Bottom);
            if (r.Height <= 0)
                return r.Y;
            else
                return r.Y + r.Height;
        }
    }
}
