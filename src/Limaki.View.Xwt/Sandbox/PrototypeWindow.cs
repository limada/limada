using System;
using System.Diagnostics;
using System.IO;
using Limaki.Drawing;
using Limaki.View.Visuals.Visualizers;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {
    public class PrototypeWindow : Window {
        private Image icon;

        private StatusIcon statusIcon;

        public void Compose() {
            Title = "Limada Xwt Prototype";
            var width = 500;
            Width = width;
            Height = 400;

            try {
                statusIcon = Application.CreateStatusIcon();
                statusIcon.Menu = new Menu();
                statusIcon.Menu.Items.Add(new MenuItem("Test"));

            } catch {
                Trace.WriteLine("Status icon could not be shown");
            }

            MainMenu = new Menu(
                new MenuItem("_File", null, null,
                    new MenuItem("_Open"),
                    new MenuItem("_New"),
                    new MenuItem("_Close", null, (s, e) => this.Close())
                    ),
                new MenuItem("_Edit", null, null,
                    new MenuItem("_Copy"),
                    new MenuItem("Cu_t"),
                    new MenuItem("_Paste")
                    )
                );

            
            var data = new PrototypeData();

            var imageDisplay = new ImageDisplay {
                Data = data.Image(ctx=>data.FontTest(ctx)),
                BackColor = Colors.White
            };
            
            var visualsDisplay = new VisualsDisplay {
               BackColor = Colors.White,
            };
            visualsDisplay.Layout.Dimension = Dimension.Y;
            visualsDisplay.Layout.Distance = new Xwt.Size(60, 60);
            visualsDisplay.StyleSheet.EdgeStyle.DefaultStyle.PaintData = true;
            visualsDisplay.Data = data.Scene;

            var box = new HPaned();
            box.Panel1.Content = visualsDisplay.ScrollView();
            box.Panel2.Content = imageDisplay.ScrollView();
            //box.Panel2.Content = new Label();
            box.Panel2.Resize = true;
            box.Position = width / 2;
            BoundsChanged += (s, e) => {
                box.Position = this.Width / 2;
            };

            Content = box;

            CloseRequested += (s, e) => {
                e.AllowClose = MessageDialog.Confirm("Samples will be closed", Command.Ok);
                if (e.AllowClose)
                    Application.Exit();
            };

            
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (statusIcon != null) {
                statusIcon.Dispose();
            }
        }
    }
}
