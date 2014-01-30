using System;
using Xwt;

namespace Limaki.View.XwtBackend {
    class Program {
        [STAThread]
        public static void Main (string[] args) {
     
            var factory = new XwtAppFactory();
            factory.Run();
        }
    }
}
