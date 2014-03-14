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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visuals.Layout;
using Limaki.Visuals;
using Xwt;
using Xwt.Drawing;
using System.Linq;


namespace Limaki.View.XwtBackend {
    public class PrototypeData {
        /// <summary>
        /// produces a bmp-image with some text
        /// </summary>
        public Image Image (Action<Context> draw) {

            var size = new Size(1000, 3000);
            // Create image.
            var builder = new ImageBuilder(size.Width, size.Height);
            var ctx = builder.Context;

            ctx.SetColor(Colors.White);
            ctx.Rectangle(0, 0, size.Width, size.Height);
            ctx.Fill();

            draw(ctx);
            var image = builder.ToBitmap(ImageFormat.RGB24);

            return image;

        }

        public void SomeText (Context ctx) {
            var size = new Size(1000, 3000);
            var font = Font.FromName("Tahoma 10");
            var stringPos = new Point(0, 0);
            var xInc = 10;
            var yInc = 3;

            ctx.SetColor(Colors.LawnGreen);
            var rnd = new Random();

            while ((stringPos.Y < size.Height) && (stringPos.X < size.Width)) {
                string s = "[No " + rnd.Next(10000).ToString("#,##0") + " *-|-]";
                var textLayout = new TextLayout();
                textLayout.Font = font;
                textLayout.Width = size.Width - stringPos.X;
                textLayout.Trimming = TextTrimming.WordElipsis;
                textLayout.Text = s;

                ctx.DrawTextLayout(textLayout, stringPos);
                ctx.Stroke();
                ctx.Fill();

                stringPos.Y += textLayout.GetSize().Height + yInc;
                stringPos.X += xInc;
            }
        }



        public IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                IGraphScene<IVisual, IVisualEdge> scene = null;
                //var examples = new SceneExamples();
                //var testData = examples.Examples[1];
                //testData.Data.Count = new Random().Next(10, 30);
                ////testData.Data.AddDensity = true;
                //scene = examples.GetScene(testData.Data);

                //var view = scene.Graph as SubGraph<IVisual, IVisualEdge>;
                //var graph = view.Source;

                //foreach (var item in graph.FindRoots(null)) {
                //    //if (!graph.IsMarker (item))
                //    view.Add(item);
                //}
                //scene.Focused = scene.Elements.First();
                //var styleSheets = Registry.Pooled<StyleSheets>();
                //var styleSheet = styleSheets.DefaultStyleSheet;

                //var expander = new GraphSceneFolding<IVisual, IVisualEdge>();
                //expander.SceneHandler = () => scene;
                //var layout = new VisualsSceneLayout<IVisual, IVisualEdge>(expander.SceneHandler, styleSheet);
                //layout.Dimension = Dimension.Y;
                //expander.Layout = () => layout;
                //expander.Folder.Expand(true);
                return scene;
            }
        }

        public Rectangle FontTest (Context ctx) {
            var tl1 = new TextLayout();
            var col2 = new Rectangle(5,5,0,0);
            var font1 = Font.SystemFont;
            var families = Font.AvailableFontFamilies.ToArray();
            int fSize = 10;
            Random fIndex = new Random();
            // Text line
            for (fSize = 6; fSize < 50; fSize += 1) {
                ctx.Save();
                font1 = Font.FromName(families[fIndex.Next(families.Length)]);
                var lotPos = new Point(col2.Left, col2.Bottom);

                tl1 = new TextLayout();
                tl1.Font = font1.WithSize(fSize);
                tl1.Text = lotPos.ToString() + " \n" + font1.Family + " | " + fSize.ToString();
                var lotSize = tl1.GetSize();
                col2.Left += lotSize.Width + 20;

                // draw with textlayout.Height = -1
                ctx.SetColor(Colors.Red);
                ctx.DrawTextLayout(tl1, lotPos);

                // draw with textlayout.Height = lotSize.Height
                ctx.SetColor(Colors.Black);
                tl1.Height = lotSize.Height;
                ctx.DrawTextLayout(tl1, lotPos);

                ctx.SetColor(Colors.Black.WithAlpha(.5));
                ctx.SetLineWidth(1);
                ctx.MoveTo(lotPos);
                ctx.LineTo(lotPos.X + lotSize.Width, lotPos.Y);
                ctx.Stroke();

                ctx.Restore();
                if (col2.Left > 500) {
                    col2.Left = 5;
                    col2.Bottom += tl1.Height + 20;
                }
            }

            return col2;
        }
    }
}