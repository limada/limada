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

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Styles;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using ComboBox = System.Windows.Controls.ComboBox;

namespace Limaki.View.WpfBackend {

    public class LayoutToolStripBackend : ToolStripBackend, ILayoutToolStripBackend0 {

        public new LayoutToolStrip0 Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LayoutToolStrip0)frontend;
            Compose();
        }

        public ComboBox StyleSheetCombo { get; set; }

        protected virtual void Compose () {

            var size = new Xwt.Size (60, 15);
            var margin = new Thickness (3, 2, 3, 2);

            var shapeCombo = new ComboBox {
                SnapsToDevicePixels = true,
                ToolTip = "change shape",
                Width = size.Width + margin.Left + margin.Right, 
                Height = size.Height + margin.Top + margin.Bottom
            };
          
            var shapes = ShapeFactory.Shapes ().ToArray ();

            shapes.ForEach (shape => {
                var img =
                    ToolStripUtils.WpfImage (
                        XwtDrawingExtensions.AsImage (
                            XwtDrawingExtensions.Render (shape, size, UiState.None, Frontend.CurrentStyleSheet),
                            size));
                var border = new Border { Child = img, Margin = margin };
                shapeCombo.Items.Add (border);
            });

            shapeCombo.SelectionChanged += (s, e) => 
                Frontend.ShapeChange(shapes[shapeCombo.SelectedIndex]);

            StyleSheetCombo = new ComboBox {
                Width = 100, 
                ToolTip = "Stylesheets",
            };

            StyleSheetCombo.SelectionChanged += (s, e) =>
                Frontend.StyleSheetChange (StyleSheetCombo.SelectedItem.ToString ());

            Registry.Pooled<StyleSheets> ().Keys.ForEach (s => StyleSheetCombo.Items.Add (s));

            this.AddItems (StyleSheetCombo, shapeCombo);
        }


        public void AttachStyleSheet (string sheetName) {
            StyleSheetCombo.SelectedItem = sheetName;
        }

        public void DetachStyleSheet (string sheetName) {
            StyleSheetCombo.SelectedItem = null;
        }

        
    }
}