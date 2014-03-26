/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public class ToolStripCommand {

        public Action<object> Action { get; set; }
        public Image Image { get; set; }
        public string Text { get; set; }
        public string ToolTipText { get; set; }
        public Size Size { get; set; }

        public virtual void DoAction(object sender, EventArgs e) {

            if (Action != null)
                Action(sender);

            var commandItem = sender as IToolStripCommandItem;
            if (commandItem != null && commandItem.ToggleOnClick != null)
                ToggleCommand(commandItem, commandItem.ToggleOnClick);
        }

        public virtual void Attach(object target) {
            var item = target as IToolStripItem;
            if (item != null) {
                item.Image = this.Image;
                item.Text = this.Text;
                item.ToolTipText = this.ToolTipText;
                item.Size = this.Size;
                item.Click += this.DoAction;
            }
        }

        public virtual void DeAttach(object target) {
            var item = target as IToolStripItem;
            if (item != null) {
                item.Image = null;
                item.Text = string.Empty;
                item.ToolTipText = string.Empty;
                item.Click -= this.DoAction;
            }
        }

        public virtual void ToggleCommand(IToolStripCommandItem item1, IToolStripCommandItem item2) {
            if (item1 == item2)
                return;

            var command1 = item1.Command;
            item1.Command = item2.Command;
            item2.Command = command1;

        }
    }
}