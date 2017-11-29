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
using System.Reflection;
using Limaki.Data;

namespace Limaki.Usecases {

    public class About {

        string _version = null;
        public string Version { get { return _version ?? (_version = Assembly.GetName ().Version.ToString (4)); } }

        string _company = null;
        public string Company { get { return _company ?? (_company = AssemblyAttibue<AssemblyCompanyAttribute> ().Company); } }

        string _copyright = null;
        public string Copyright { get { return _copyright ?? (_copyright = AssemblyAttibue<AssemblyCopyrightAttribute> ().Copyright); } }

        public string Credits {
            get {
                return "Storage: db4o object database http://www.db4o.com \r\n"
                      + "Graphics abstraction layer: http://github.com/mono/xwt \r\n"
              + "Icons: http://fortawesome.github.com/Font-Awesome \r\n";
            }
        }

        public string ToolKitType { get; set; }

        public Iori Iori { get; set; }

        public string ApplicationName { get; set; }

        public override string ToString () {
            return $@"
Version: {Version}

{Company} 
{Copyright}

ToolKitType: {ToolKitType ?? ""}

Database: {Iori?.ToString() ?? ""}

Credits: 

{Credits}
";
        }

        public string Link { get { return "www.limada.org"; } }


        static Assembly Assembly { get { return Assembly.GetAssembly (typeof (About)); } }
        static A AssemblyAttibue<A> () where A : Attribute {
            return Assembly.GetCustomAttributes (typeof (A), true).Cast<A> ().FirstOrDefault ();
        }
    }
}