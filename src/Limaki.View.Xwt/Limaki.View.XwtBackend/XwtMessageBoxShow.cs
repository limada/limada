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

using System;
using System.Diagnostics;
using System.Linq;
using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtMessageBoxShow : XwtMessager, IMessageBoxShow {

        public DialogResult Show (string title, string text, MessageBoxButtons buttons) => Show (title, text, buttons, default (Xwt.Drawing.Image));

        public void ShowError (string title, string test) => Show (title, test, MessageBoxButtons.Ok, Xwt.StockIcons.Error);

        public DialogResult Show (string title, string text, MessageBoxButtons buttons, Xwt.Drawing.Image icon) {

            try {

                Application.MainLoop.DispatchPendingEvents ();

                if (buttons == MessageBoxButtons.None) {
                    var result = Run (title, MessageFor (text, buttons, icon ?? Xwt.StockIcons.Information));
                    return DialogResult.None;
                }

                MessageDescription MessageFor (string txt, MessageBoxButtons bttns, Xwt.Drawing.Image icn = null) {
                    var msg = new MessageDescription {
                        Text = text,
                        Icon = icn,
                    };
                    foreach (var c in ToXwt (bttns))
                        msg.Buttons.Add (c);
                    if (msg.Buttons.Count == 1)
                        msg.DefaultButton = 1;
                    return msg;
                }

                if (buttons == MessageBoxButtons.Yes || buttons == MessageBoxButtons.Ok) {
                    var result = Run (title, MessageFor (text, buttons, icon ?? Xwt.StockIcons.Warning));
                    return ToLim (result);
                }

                if ((buttons & (MessageBoxButtons.No | MessageBoxButtons.Cancel | MessageBoxButtons.Retry)) != 0) {
                    var question = Run (title, MessageFor (text, buttons, icon ?? Xwt.StockIcons.Question));
                    return ToLim (question);
                }

                return DialogResult.None;
            } catch (Exception ex) {
                Application.NotifyException (ex);
                return DialogResult.None;
            }
            return DialogResult.None;
        }

        public void Show (string title, string text, MessageBoxButtons buttons, Action<DialogResult> onResult) {
            var result = Show (title, text, buttons);
            onResult (result);
        }

        public Command Run (string title, MessageDescription message) {

            Dialog dlg = new Dialog {
                Resizable = false,
                Padding = 0,
                Title = title,
                // TODO TransientFor = MessageDialog.RootWindow
            };

            var mainBox = new HBox { Margin = 25 };
            if (MessageDialog.RootWindow != null) {
                dlg.TransientFor = MessageDialog.RootWindow;
            }

            if (message.Icon != null) {
                var image = new ImageView (message.Icon.WithSize (32, 32));
                mainBox.PackStart (image, vpos: WidgetPlacement.Start);
            }
            var box = new VBox { Margin = 3, MarginLeft = 8, Spacing = 15 };
            mainBox.PackStart (box, true);
            var text = new Label {
                Text = message.Text ?? "",
                Markup = $"<b>{message.Text ?? ""}</b>",
            };
            Label stext = null;
            box.PackStart (text);
            if (!string.IsNullOrEmpty (message.SecondaryText)) {
                stext = new Label {
                    Text = message.SecondaryText
                };
                box.PackStart (stext);
            }
            foreach (var option in message.Options) {
                var check = new CheckBox (option.Text) {
                    Active = option.Value
                };
                box.PackStart (check);
                check.Toggled += (sender, e) => message.SetOptionValue (option.Id, check.Active);
            }
            dlg.Buttons.Add (message.Buttons.ToArray ());
            if (message.DefaultButton >= 0 && message.DefaultButton < message.Buttons.Count)
                dlg.DefaultCommand = message.Buttons[message.DefaultButton];
            if (mainBox.Surface.GetPreferredSize (true).Width > 300) {
                text.Wrap = WrapMode.Word;
                if (stext != null)
                    stext.Wrap = WrapMode.Word;
                mainBox.WidthRequest = 300;
            }
            var s = mainBox.Surface.GetPreferredSize (true);

            dlg.Content = mainBox;
            dlg.CommandActivated += (sender, e) => {
                Application.Invoke (() => dlg.Close ());
                Application.MainLoop.DispatchPendingEvents ();
            };
            if (!dlg.Buttons.Any ()) {
                dlg.Content.ButtonReleased += (sender, e) => dlg.Close ();
                box.ButtonReleased += (sender, e) => dlg.Close ();
                text.ButtonReleased += (sender, e) => dlg.Close ();
            }
            return dlg.Run ();
        }

    }
}