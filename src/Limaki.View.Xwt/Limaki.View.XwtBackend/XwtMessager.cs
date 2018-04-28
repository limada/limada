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

using System.Collections.Generic;
using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtMessager {

        public static Command Ok = new Command("Ok");
        public static Command Cancel = new Command("Cancel");
        public static Command Yes = new Command("Yes");
        public static Command No = new Command("No");
        public static Command Abort = new Command("Abort");
        public static Command Retry = new Command("Retry");
        public static Command Ignore = new Command("Ignore");

        protected virtual Command[] ToXwt (MessageBoxButtons buttons) {
            var result = new List<Command>();
            if (buttons.HasFlag(MessageBoxButtons.Ok)) {
                result.Add(Ok);
            }
            if (buttons.HasFlag(MessageBoxButtons.Cancel)) {
                result.Add(Cancel);
            }
            if (buttons.HasFlag(MessageBoxButtons.Yes)) {
                result.Add(Yes);
            }
            if (buttons.HasFlag(MessageBoxButtons.No)) {
                result.Add(No);
            }
            if (buttons.HasFlag(MessageBoxButtons.Abort)) {
                result.Add(Abort);
            }
            if (buttons.HasFlag(MessageBoxButtons.Retry)) {
                result.Add(Retry);
            }
            if (buttons.HasFlag(MessageBoxButtons.Ignore)) {
                result.Add(Ignore);
            }

            return result.ToArray();
        }

        protected virtual DialogResult ToLim (Command command) {
            if (command == Ok) {
                return DialogResult.Ok;
            }
            if (command == Cancel) {
                return DialogResult.Cancel;
            }
            if (command == Yes) {
                return DialogResult.Yes;
            }
            if (command == No) {
                return DialogResult.No;
            }
            if (command == Abort) {
                return DialogResult.Abort;
            }
            if (command == Retry) {
                return DialogResult.Retry;
            }
            if (command == Ignore) {
                return DialogResult.Ignore;
            }
            return DialogResult.None;
        }
    }
}