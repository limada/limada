namespace Limaki.Viewers {
    public interface IMessageBoxShow  {
        DialogResult Show(string title, string text, MessageBoxButtons buttons);
    }
}