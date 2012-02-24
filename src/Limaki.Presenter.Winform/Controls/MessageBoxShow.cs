using System.Windows.Forms;
using Limaki.UseCases.Viewers;
using DialogResult = Limaki.UseCases.Viewers.DialogResult;
using Limaki.Presenter.Winform;
using MessageBoxButtons = Limaki.UseCases.Viewers.MessageBoxButtons;

namespace Limaki.Winform.Controls {
    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }


    }
}