using System;

namespace Limaki.View.Common {
    
    public interface IMessageBoxShow  {
        DialogResult Show(string title, string text, MessageBoxButtons buttons);
        void Show (string title, string text, MessageBoxButtons buttons, Action<DialogResult> onResult);
    }

}