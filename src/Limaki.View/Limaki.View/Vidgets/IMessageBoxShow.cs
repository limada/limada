using System;
namespace Limaki.View.Vidgets {
    public interface IMessageBoxShow  {
        DialogResult Show(string title, string text, MessageBoxButtons buttons);
        void Show (string title, string text, MessageBoxButtons buttons, Action<DialogResult> onResult);
    }
}