/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Drawing;
using System.Windows.Forms;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public class ToolStripRenderer : ToolStripProfessionalRenderer {

        public System.Drawing.Color? ToolStripButtonCheckedColor { get; set; }

        protected Brush _buttonBackgroundBrush = null;
        protected Brush ButtonBackgroundBrush { get { return _buttonBackgroundBrush ?? (_buttonBackgroundBrush = new SolidBrush(ToolStripButtonCheckedColor.Value)); } }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
            var btn = e.Item as ToolStripButton;
            if (btn != null && ToolStripButtonCheckedColor.HasValue && (btn.Checked || btn.Selected)) {
                var bounds = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(ButtonBackgroundBrush, bounds);
            } else
                base.OnRenderButtonBackground(e);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e) {
            var drawBackground = ToolStripButtonCheckedColor.HasValue && e.Item.Selected;
            if (drawBackground) {
                var bounds = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(ButtonBackgroundBrush, bounds);
            } else
                base.OnRenderItemBackground(e);
        }

        protected override void OnRenderDropDownButtonBackground (ToolStripItemRenderEventArgs e) {
            var drawBackground = ToolStripButtonCheckedColor.HasValue && e.Item.Selected;
            if (drawBackground) {
                var bounds = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(ButtonBackgroundBrush, bounds);
            } else
            base.OnRenderDropDownButtonBackground(e);
        }
    }

}