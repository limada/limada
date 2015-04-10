using System;
using System.Windows.Forms;
using Limaki.View.Vidgets;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using MessageBoxButtons = Limaki.View.Vidgets.MessageBoxButtons;

namespace Limaki.View.SwfBackend.Controls {

    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }

        public void Show (string title, string text, MessageBoxButtons buttons, Action<DialogResult> onResult) {
            var result = Show (title, text, buttons);
            onResult (result);
        }
    }
}