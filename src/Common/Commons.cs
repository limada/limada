/*
 * Limaki 
 * Version 0.063
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

namespace Limaki {
    public class Commons {
        protected static Nullable<bool> _mono = null;
        public static bool Mono {
            get {
                if (_mono == null) {
                    _mono = Type.GetType("Mono.Runtime") != null; ;
                }
                return _mono.Value;
            }
        }
        protected static Nullable<bool> _unix = null;
        public static bool Unix {
            get {
                if (_unix == null) {
                    _unix = Environment.OSVersion.Platform.ToString ().Contains("Unix");
                }
                return _unix.Value;
            }
        }
    }
}
