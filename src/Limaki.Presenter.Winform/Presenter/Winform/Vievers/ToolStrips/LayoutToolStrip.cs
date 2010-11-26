/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.UseCases.Viewers.ToolStrips;

namespace Limaki.UseCases.Winform.Viewers.ToolStrips {
    public partial class LayoutToolStrip : ToolStrip, ILayoutTool {
        
        public LayoutToolController Controller = new LayoutToolController();

        public LayoutToolStrip() {
            InitializeComponent();
            InitDefaultStyles();
            InitLayoutTools();
            Controller.Tool = this;

        }

        void InitDefaultStyles() {
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            ShapeCombo.ShapeComboBoxControl.ShapeLayout.StyleSheet =
                styleSheets[styleSheets.StyleSheetNames[1]];

        }

        void InitLayoutTools() {
            StyleSheetCombo.Items.Clear();
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            foreach (IStyleSheet styleSheet in styleSheets.Values) {
                StyleSheetCombo.Items.Add(styleSheet.Name);
            }
            Application.DoEvents();
        }

        public void AttachStyleSheet(string sheetName) {
            StyleSheetCombo.SelectedIndexChanged -= StyleSheetSelectedIndexChanged;
            StyleSheetCombo.SelectedItem = sheetName;
            StyleSheetCombo.SelectedIndexChanged += StyleSheetSelectedIndexChanged;
        }

        

        private void StyleSheetSelectedIndexChanged(object sender, EventArgs e) {
            Controller.StyleSheetChange (StyleSheetCombo.SelectedItem.ToString ());
        }

        private void StyleSheetKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                int i = StyleSheetCombo.Items.Add(StyleSheetCombo.Text);
                StyleSheetCombo.SelectedIndex = i;
            }
        }

        private void ShapeSelectedIndexChanged(object sender, EventArgs e) {
            Controller.ShapeChange(ShapeCombo.ShapeComboBoxControl.SelectedItem as IShape);
        }

    }
}