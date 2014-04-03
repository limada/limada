using System;
using Limaki.Common;
using Limaki.SwfBackend;

namespace Xwt.Html5.TestApp {

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            if (Registry.ConcreteContext == null) {
                var loader = new SwfContextResourceLoader ();
                Registry.ConcreteContext = new Limaki.Common.IOC.ApplicationContext ();
                loader.ApplyResources (Registry.ConcreteContext);
                
            }

            System.Windows.Forms.Application.Run(new XwtHtml5TestMainForm());
        }
    }
}
