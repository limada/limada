using System.Windows.Forms;
using Limaki.Viewers;
using DialogResult = Limaki.Viewers.DialogResult;
using Limaki.View.Winform;
using MessageBoxButtons = Limaki.Viewers.MessageBoxButtons;

namespace Limaki.Winform.Controls {
    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }


    }
}