using System.Windows.Forms;
using Limaki.View.Vidgets;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using MessageBoxButtons = Limaki.View.Vidgets.MessageBoxButtons;

namespace Limaki.SwfBackend.Controls {

    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }

    }
}