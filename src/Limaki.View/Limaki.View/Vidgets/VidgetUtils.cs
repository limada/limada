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

namespace Limaki.View.Vidgets {

    public class VidgetUtils {

        public static void SetCommand (IToolStripCommandToggle0 item, ref ToolStripCommand0 _command, ToolStripCommand0 value) {
            if (_command != value) {
                try {
                    if (_command != null)
                        _command.DeAttach (item);
                    _command = value;
                    _command.Attach (item);
                } finally {}
            }
        }

        public static void SetCommand (IToolStripCommand command, IToolStripCommand value) {
            if (command != null && value != null) {
                command.Image = value.Image;
                command.Label = value.Label;
                command.ToolTipText = value.ToolTipText;
                command.Size = value.Size;
                command.Action = value.Action;
            }
        }

        public static void ToggleCommand (IToolStripCommand a, IToolStripCommand b) {
            var c = new ToolStripCommand (a);
            SetCommand (a, b);
            SetCommand (b, c);
        }
    }
}