/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.View;
using System.Linq;

namespace Limaki.Usecases {

    public class About {
        string _version = null;
        public string Version {
            get {
                if (_version == null) {
                    _version = System.Reflection.Assembly.GetAssembly (GetType ()).GetName ().Version.ToString (4);
                }
                return _version;
            }
        }

        public string Credits {
            get {
                return "Storage: db4o object database http://www.db4o.com \r\n"
                      + "Graphics abstraction layer: http://github.com/mono/xwt \r\n"
              + "Icons: http://fortawesome.github.com/Font-Awesome \r\n";
            }
        }

        public string ToolKitType { get; set; }

        public override string ToString () {
            return string.Format ($"Version {Version}\nCredits {Credits}\nToolKitType {ToolKitType??""}");
        }

        public string Link { get { return "www.limada.org";}}
    }
}