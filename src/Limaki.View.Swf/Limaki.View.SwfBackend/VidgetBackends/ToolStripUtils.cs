/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.View;
using Limaki.View.Vidgets;
using System.Windows.Forms;
using System.Linq;
using Limaki.Common.Linqish;
using SWF = System.Windows.Forms;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolStripUtils {

        static IUISystemInformation _systemInformation = null;

        public static IUISystemInformation SystemInformation {
            get { return _systemInformation ?? (_systemInformation = Registry.Pooled<IUISystemInformation>()); }
        }

        public static int DropdownWidth = SystemInformation.VerticalScrollBarWidth / 3*2;

        public static void SetCommand(IToolStripCommandItem item, ref ToolStripCommand _command, ToolStripCommand value) {
            var toolStripItem = item as SWF.ToolStripItem;
            if (_command != value) {
                try {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.SuspendLayout();
                    if (_command != null)
                        _command.DeAttach(item);
                    _command = value;
                    _command.Attach(item);
                } finally {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.ResumeLayout(true);
                }
            }
        }

        public System.Drawing.Color? ToolStripBackground = null;
        public System.Drawing.Color? ToolStripForeground = null;
        public System.Drawing.Color? ToolStripItemSelectedColor = null;

        public void InitializeToolstrips (SWF.ToolStripPanel toolStripPanel, SWF.ToolStrip menuStrip, SWF.ToolStrip[] toolStrips) {

            toolStripPanel.SuspendLayout ();

            var location = new System.Drawing.Point ();

            toolStripPanel.Controls.Clear ();
            toolStripPanel.ResumeLayout (true);
            var renderer = toolStripPanel.Renderer;
            renderer = new ToolStripRenderer {
                ToolStripButtonCheckedColor = this.ToolStripItemSelectedColor
            };
            toolStripPanel.Renderer = renderer;
            Application.DoEvents ();

            toolStripPanel.SuspendLayout ();

            if (ToolStripBackground != null) {
                toolStripPanel.BackColor = ToolStripBackground.Value;
                menuStrip.BackColor = ToolStripBackground.Value;
            }
            if (ToolStripForeground != null) {
                toolStripPanel.ForeColor = ToolStripForeground.Value;
                menuStrip.ForeColor = ToolStripForeground.Value;
            }
            if (menuStrip != null) {
                menuStrip.Location = new System.Drawing.Point ();
                toolStripPanel.Controls.Add (menuStrip);
                location = menuStrip.Location + new System.Drawing.Size (0, menuStrip.Size.Height + 3);
            }

            toolStrips.ForEach (toolStrip => {
                toolStrip.SuspendLayout ();
                toolStrip.Renderer = renderer;
                var border = toolStrip.Items.Count;
                var size = new System.Drawing.Size (4, toolStrip.Size.Height);
                toolStrip.Items.Cast<SWF.ToolStripItem> ().ForEach (s =>
                    size = new System.Drawing.Size (size.Width + s.Bounds.Width + 3, size.Height)
                    );
                toolStrip.ResumeLayout (true);
                toolStrip.Size = size;
                if (ToolStripBackground != null)
                    toolStrip.BackColor = ToolStripBackground.Value;
                if (ToolStripForeground != null)
                    toolStrip.ForeColor = ToolStripForeground.Value;
            });

            toolStrips.ForEach (toolStrip => {
                toolStrip.Location = location;
                if (toolStrip.Bounds.Right > toolStripPanel.Bounds.Right)
                    toolStrip.Size = new System.Drawing.Size (1, 1);
                location = new System.Drawing.Point (toolStrip.Bounds.Right + 1, toolStrip.Bounds.Top);
            });

            toolStripPanel.Controls.AddRange (toolStrips);

            toolStripPanel.ResumeLayout (false);
            toolStripPanel.PerformLayout ();


        }

    }
}