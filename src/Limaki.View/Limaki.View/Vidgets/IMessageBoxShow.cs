namespace Limaki.View.Vidgets {
    public interface IMessageBoxShow  {
        DialogResult Show(string title, string text, MessageBoxButtons buttons);
    }
}