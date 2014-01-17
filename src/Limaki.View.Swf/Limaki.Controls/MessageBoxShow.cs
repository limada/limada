using System.Windows.Forms;
using Limaki.View.Swf;
using Limaki.Viewers;
using DialogResult = Limaki.Viewers.DialogResult;
using MessageBoxButtons = Limaki.Viewers.MessageBoxButtons;

namespace Limaki.Swf.Backends {

    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }

    }
}