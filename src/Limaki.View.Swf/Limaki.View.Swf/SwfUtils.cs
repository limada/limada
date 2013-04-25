/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using LinqKit;
using System.Linq;

namespace Limaki.View.Swf {

    public class SwfUtils {

        public void SetFont(System.Drawing.Font font, Control control) {
            control.Font = font;
            foreach (Control child in control.Controls) {
                SetFont(font, child);
            }
        }

        public bool IsSameApp(IWidgetBackend a, IWidgetBackend b) {
            var control = a as Control;
            var thisControl = b as Control;
            if (control != null) {
                Form form = control.FindForm();
                bool sameApp = false;
                foreach (Form iForm in Application.OpenForms) {
                    if (iForm == form) {
                        sameApp = true;
                        break;
                    }
                }
                return sameApp;
            }
            return false;
        }

        public System.Drawing.Color? ToolStripBackground = null;
        public System.Drawing.Color? ToolStripForeground = null;
        public System.Drawing.Color? ToolStripItemSelectedColor = null;
        
        public void InitializeToolstrips(ToolStripPanel toolStripPanel, ToolStrip menuStrip, ToolStrip[] toolStrips) {

            toolStripPanel.SuspendLayout();

            var location = new System.Drawing.Point();

            toolStripPanel.Controls.Clear();
            toolStripPanel.ResumeLayout(true);
            var renderer = toolStripPanel.Renderer;
            renderer = new Limaki.Swf.Backends.Viewers.ToolStrips.ToolStripRenderer {
                ToolStripButtonCheckedColor = this.ToolStripItemSelectedColor
            };
            toolStripPanel.Renderer = renderer;
            Application.DoEvents();

            toolStripPanel.SuspendLayout();

            if (ToolStripBackground != null) {
                toolStripPanel.BackColor = ToolStripBackground.Value;
                menuStrip.BackColor = ToolStripBackground.Value;
            }
            if (ToolStripForeground != null) {
                toolStripPanel.ForeColor = ToolStripForeground.Value;
                menuStrip.ForeColor = ToolStripForeground.Value;
            }
            if (menuStrip != null) {
                menuStrip.Location = new System.Drawing.Point();
                toolStripPanel.Controls.Add(menuStrip);
                location = menuStrip.Location + new System.Drawing.Size(0, menuStrip.Size.Height + 3);
            }

            toolStrips.ForEach(toolStrip => {
                toolStrip.SuspendLayout();
                toolStrip.Renderer = renderer;
                var border = toolStrip.Items.Count;
                var size = new System.Drawing.Size(4, toolStrip.Size.Height);
                toolStrip.Items.Cast<ToolStripItem>().ForEach(s =>
                    size = new System.Drawing.Size(size.Width + s.Bounds.Width + 3, size.Height)
                    );
                toolStrip.ResumeLayout(true);
                toolStrip.Size = size;
                if (ToolStripBackground != null)
                    toolStrip.BackColor = ToolStripBackground.Value;
                if (ToolStripForeground != null)
                    toolStrip.ForeColor = ToolStripForeground.Value;
            });

            toolStrips.ForEach(toolStrip => {
                toolStrip.Location = location;
                if(toolStrip.Bounds.Right>toolStripPanel.Bounds.Right)
                    toolStrip.Size = new System.Drawing.Size(1, 1);
                location = new System.Drawing.Point(toolStrip.Bounds.Right + 1, toolStrip.Bounds.Top);
            });

            toolStripPanel.Controls.AddRange(toolStrips);

            toolStripPanel.ResumeLayout(false);
            toolStripPanel.PerformLayout();


        }
    }
}