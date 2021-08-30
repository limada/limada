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
using Limaki.Common;
using Xwt;
using Xwt.Drawing;

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

        public override void Transform(Matrix matrix) {
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

        public override Size DataSize {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public override Size Size {
            get { return DataSize; }
            set { DataSize = value; }
        }

        public override Point[] Hull(int delta, bool extend) {
            throw new NotImplementedException();
        }

        public override Point[] Hull(Matrix matrix, int delta, bool extend) {
            throw new NotImplementedException();
        }
    }
}