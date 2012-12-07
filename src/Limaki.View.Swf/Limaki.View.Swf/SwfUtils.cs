/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
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

        public void InitializeToolstripPositions(ToolStripPanel toolStripPanel, ToolStrip MenuStrip, ToolStrip[] toolStrips) {

            toolStripPanel.SuspendLayout();

            var location = new System.Drawing.Point();

            toolStripPanel.Controls.Clear();
            toolStripPanel.ResumeLayout(true);
            Application.DoEvents();

            toolStripPanel.SuspendLayout();

            if (MenuStrip != null) {
                MenuStrip.Location = new System.Drawing.Point();
                toolStripPanel.Controls.Add(MenuStrip);
                location = MenuStrip.Location + new System.Drawing.Size(0, MenuStrip.Size.Height + 3);
            }

            toolStrips.ForEach(toolStrip => {
                toolStrip.SuspendLayout();
                var border = toolStrip.Items.Count;
                var size = new System.Drawing.Size(4, toolStrip.Size.Height);
                toolStrip.Items.Cast<ToolStripItem>().ForEach(s =>
                    size = new System.Drawing.Size(size.Width + s.Bounds.Width + 3, size.Height)
                    );
                toolStrip.ResumeLayout(true);
                toolStrip.Size = size;
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