using System.Windows.Forms;
using Limaki.Presenter.Winform;
using Limaki.UseCases.Viewers;
using DialogResult = Limaki.UseCases.Viewers.DialogResult;

namespace Limaki.Winform.Controls {
    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, UseCases.Viewers.MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }


    }
}