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
using System;
using Limaki.Common;
using Xwt;

namespace Limaki.Drawing.Shapes {
#if !SILVERLIGHT
    [Serializable]
#endif

    /// <summary>
    /// this will be a class for feeding the painter and painterfactories
    /// IDataPainter should become obsolete with this
    /// </summary>
   [TODO]
    public class StringShape : Shape<string> {

        public override Point this[Anchor i] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override void Transform(Matrice matrice) {
            throw new NotImplementedException();
        }

        public override object Clone() {
            throw new NotImplementedException();
        }

        public override Point Location {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override Size Size {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override Point[] Hull(int delta, bool extend) {
            throw new NotImplementedException();
        }

        public override Point[] Hull(Matrice matrix, int delta, bool extend) {
            throw new NotImplementedException();
        }
    }
}