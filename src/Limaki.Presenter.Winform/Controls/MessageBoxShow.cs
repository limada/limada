using System.Windows.Forms;
using DialogResult = Limaki.UseCases.Viewers.DialogResult;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Winform;

namespace Limaki.Winform.Controls {
    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, Limaki.UseCases.Viewers.MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }


    }
}