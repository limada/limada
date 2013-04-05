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

using System;
using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.XwtAdapter {

    public static class XwtLExtensions {

        public static double Degree = 180 / Math.PI;

        public static double Angle (this Matrix m) {
            if ((m.M21 != (-1 * m.M12)) ||
                (m.M22 != m.M11) ) {
                return double.NaN;
            } else {
                var scale_factor = Math.Sqrt ((m.M11 * m.M22 - m.M12 * m.M21));
                return Math.Acos (m.M11 / scale_factor) * Degree;
            }
        }

        public static void SetTransform (this Context context, Matrix transform) {
            context.ResetTransform();
            if (!transform.IsIdentity) {
                context.Scale (transform.M11, transform.M22);
                context.Translate (transform.OffsetX, transform.OffsetY);
                context.Rotate (transform.Angle());
            }
        }

        public static Matrix GetTransform (this Context context) {
            throw new NotImplementedException();
        }

        public static void SetElements(this Matrix m, double[] value) {
                m.M11 = value[0];
                m.M12 = value[1];
                m.M21 = value[2];
                m.M22 = value[3];
                m.OffsetX = value[4];
                m.OffsetY= value[5];
        }

        public static double[] GetElements(this Matrix m) {
            return new double[] { m.M11, m.M12, m.M21, m.M22, m.OffsetX, m.OffsetY };
        }
    }
}