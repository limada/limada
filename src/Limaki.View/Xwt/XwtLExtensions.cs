using System;
using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.XwtAdapter {

    public static class XwtLExtensions {

        public static double Degree = 180 / Math.PI;
        public static double Angle (this Matrice m) {
            if ((m.Elements[2] != (-1 * m.Elements[1])) ||
                (m.Elements[3] != m.Elements[0]) ) {
                return double.NaN;
            } else {
                var scale_factor = Math.Sqrt ((m.Elements[0] * m.Elements[3] - m.Elements[1] * m.Elements[2]));
                return Math.Acos (m.Elements[0] / scale_factor) * Degree;
            }
        }

        public static void SetTransform (this Context context, Matrice transform) {
            context.ResetTransform();
            if (!transform.IsIdentity) {
                context.Scale (transform.Elements[0], transform.Elements[3]);
                context.Translate (transform.Elements[4], transform.Elements[5]);
                context.Rotate (transform.Angle());
            }
        }

        public static Matrice GetTransform (this Context context) {
            throw new NotImplementedException();
        }
    }
}