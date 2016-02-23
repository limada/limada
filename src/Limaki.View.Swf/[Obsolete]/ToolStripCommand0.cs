
/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;

namespace Limaki.View.Vidgets {

    [Obsolete]
    public class ToolStripCommand0:ToolStripCommand {

        public virtual void DoAction(object sender, EventArgs e) {

            if (Action != null)
                Action(sender);

            var commandItem = sender as IToolStripCommandToggle0;
            if (commandItem != null && commandItem.ToggleOnClick != null)
                ToggleCommand(commandItem, commandItem.ToggleOnClick);
        }

        public virtual void Attach(object target) {
            var item = target as IToolStripItem0;
            if (item != null) {
                item.Image = this.Image;
                item.Label = this.Label;
                item.ToolTipText = this.ToolTipText;
                item.Size = this.Size;
                item.Click += this.DoAction;
            }
        }
        
        public virtual void DeAttach(object target) {
            var item = target as IToolStripItem0;
            if (item != null) {
                item.Image = null;
                item.Label = string.Empty;
                item.ToolTipText = string.Empty;
                item.Click -= this.DoAction;
            }
        }

        public virtual void ToggleCommand (IToolStripCommandToggle0 item1, IToolStripCommandToggle0 item2) {
            if (item1 == item2)
                return;

            var command1 = item1.Command;
            item1.Command = item2.Command;
            item2.Command = command1;

        }
    }
}