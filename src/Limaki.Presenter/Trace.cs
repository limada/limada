using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if SILVERLIGHT
namespace System.Diagnostics {
    public sealed class Trace {
        public static void WriteLine(string p) {
            System.Console.WriteLine (p);
        }
    }
}

#endif