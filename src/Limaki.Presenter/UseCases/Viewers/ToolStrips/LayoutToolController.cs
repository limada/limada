/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.Visuals;
using Limaki.Presenter.Visuals.UI;
using Limaki.Visuals;
using Limaki.Context;

namespace Limaki.UseCases.Viewers.ToolStrips {
    public class LayoutToolController:ToolController<VisualsDisplay,ILayoutTool> {

        public void StyleSheetChange(string sheetName) {
            IStyleSheet styleSheet = null;
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            if (!styleSheets.TryGetValue(sheetName, out styleSheet)) {
                IStyle style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = StyleNames.DefaultStyle;
                styleSheet = new StyleSheet(sheetName, style);
                styleSheets.Add(styleSheet.Name, styleSheet);
            }
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                currentDisplay.StyleSheet = styleSheet;
                currentDisplay.DeviceRenderer.Render();
            }
        }

        public void ShapeChange(IShape shape) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                foreach (IVisual visual in currentDisplay.Data.Selected.Elements) {
                    SceneTools.ChangeShape (currentDisplay.Data, visual, shape);
                }
                currentDisplay.Execute ();
            }
        }

        public override void Attach(object sender) {
            var display = sender as VisualsDisplay;
            if (display != null) {
                this.CurrentDisplay = display;
                Tool.AttachStyleSheet(display.StyleSheet.Name);
            }
        }
    }
}