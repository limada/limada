#define NOT_MAKEBUNLDE

using System;
using System.Diagnostics;

namespace Limaki.View.XwtBackend {

    class Program {

        [STAThread]
        public static void Main (string[] args) {

			Preload();

            var factory = new XwtConceptUsecaseAppFactory();
            factory.Run();
        }

		[Conditional("MAKEBUNDLE")]
		static void Preload(){
			Trace.WriteLine (string.Format ("static {0}", typeof(Limaki.View.ViewContextResourceLoader)));
			Trace.WriteLine (string.Format ("static {0}", typeof(Limaki.View.XwtBackend.XwtContextResourceLoader)));
			#if MAKEBUNDLE
			Trace.WriteLine (string.Format ("static {0}", typeof(Limaki.View.GtkBackend.GtkContextResourceLoader)));
			#endif
			Trace.WriteLine (string.Format ("static {0}", typeof(Limaki.db4o.Db4oResourceLoader)));
		}
    }
}
