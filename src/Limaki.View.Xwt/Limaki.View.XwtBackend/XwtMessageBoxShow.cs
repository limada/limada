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

using System.Linq;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtMessageBoxShow : XwtMessager, IMessageBoxShow {

        public DialogResult Show (string title, string text, MessageBoxButtons buttons) {
            var result = DialogResult.None;
            if (buttons == MessageBoxButtons.None) {
                MessageDialog.ShowMessage(title, text);
                return DialogResult.None;
            }
            if ((buttons & (MessageBoxButtons.Yes | MessageBoxButtons.Ok)) != 0) {
                MessageDialog.ShowMessage(title, text);
                if (buttons == MessageBoxButtons.Yes)
                    return DialogResult.Yes;
                return DialogResult.Ok;
            }

            if ((buttons & (MessageBoxButtons.No | MessageBoxButtons.Cancel | MessageBoxButtons.Retry)) != 0) {
                var question = MessageDialog.AskQuestion(title, text, 0, ToXwt(buttons));
                return ToLim(question);
            }

            if (false) {
                var confirm = MessageDialog.Confirm(title, text, ToXwt(buttons).First());
                if (confirm)
                    return DialogResult.Ok;
                else
                    return DialogResult.Cancel;
            }

            return DialogResult.None;
        }
    }
}