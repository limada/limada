using System;

namespace Limaki.View.Common {

    public interface IMessageBoxShow {

        DialogResult Show (string title, string text, MessageBoxButtons buttons);
        void ShowError (string title, string text);
        void Show (string title, string text, MessageBoxButtons buttons, Action<DialogResult> onResult);
    }

}