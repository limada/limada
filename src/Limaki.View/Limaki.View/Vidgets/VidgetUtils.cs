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
        
        public static void SetCommand (ICommandView command, ICommandView value) {
            if (command != null && value != null) {
                command.Image = value.Image;
                command.Label = value.Label;
                command.ToolTipText = value.ToolTipText;
                command.Size = value.Size;
                command.Action = value.Action;
            }
        }

        public static void ToggleCommand (ICommandView a, ICommandView b) {
            var c = new CommandView (a);
            SetCommand (a, b);
            SetCommand (b, c);
        }
    }
}