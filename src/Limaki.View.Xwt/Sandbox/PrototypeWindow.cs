/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Diagnostics;
using System.IO;
using Limaki.Drawing;
using Limaki.View.Common;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {
    public class PrototypeWindow : Window {

        private Image icon;

        private StatusIcon statusIcon;

        public PrototypeWindow Composed () {
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

            var data = new PrototypeData();

            var imageDisplay = new ImageDisplay {
                Data = data.Image(ctx=>data.FontTest(ctx)),
                BackColor = Colors.White,
                ZoomState = ZoomState.FitToScreen,
            };
            
            var visualsDisplay = new VisualsDisplay {
               BackColor = Colors.White,
            };
            visualsDisplay.Layout.Dimension = Dimension.Y;
            visualsDisplay.Layout.Distance = new Xwt.Size(60, 60);
            visualsDisplay.StyleSheet.EdgeStyle.DefaultStyle.PaintData = true;
            visualsDisplay.Data = data.Scene;

            var box = new HPaned();
            box.Panel1.Content = visualsDisplay.WithScrollView();
            box.Panel2.Content = imageDisplay.WithScrollView();
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

            Action popover = () => {
                var popV = new Popover(new Label { Text = "hy" });
                popV.Show(Popover.Position.Bottom, box.Panel1.Content);
            };
            Action edit = () => {
                var edite = new TextEntry();
                (visualsDisplay.Backend as Canvas).AddChild(edite, new Rectangle(40, 40, 100, 20));
            };
            Action changeView = () => {
                var one = box.Panel1.Content;
                var two = box.Panel2.Content;
                box.Panel1.Content = null;
                box.Panel2.Content = null;
                box.Panel1.Content = two;
                box.Panel2.Content = one;
            };
            Action newImageDisplay = () => {
                imageDisplay = new ImageDisplay {
                    Data = data.Image(ctx => data.FontTest(ctx)),
                    BackColor = Colors.White
                };
                box.Panel2.Content = imageDisplay.Backend as Widget;
            };
            Action nextImage = () => {
                                   imageDisplay.Data = data.Image(ctx => data.FontTest(ctx));
                               };
            visualsDisplay.SceneFocusChanged += (s, e) => nextImage();
            //(visualsDisplay.Backend as Widget).ButtonPressed += (s, e) => nextImage();

            MainMenu = new Menu(
                new MenuItem("_File", null, null,
                    new MenuItem("_Open"),
                    new MenuItem("_New"),
                    new MenuItem("_Close", null, (s, e) => this.Close())
                    ),
                new MenuItem("_Edit", null, null,
                    new MenuItem("Change View",null,(s, e) => changeView()),
                    new MenuItem("Edit", null, (s, e) => edit())
                    )
                );

            return this;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (statusIcon != null) {
                statusIcon.Dispose();
            }
        }
    }
}
