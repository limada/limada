using System.Diagnostics;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class MainWindow : Window {

       protected StatusIcon StatusIcon{get;set;}

        public MainWindow () {
            try {
                StatusIcon = Application.CreateStatusIcon();
                StatusIcon.Menu = new Menu();
                StatusIcon.Menu.Items.Add(new MenuItem("Test"));

            } catch {
                Trace.WriteLine("Status icon could not be shown");
            }
        }
    }
}