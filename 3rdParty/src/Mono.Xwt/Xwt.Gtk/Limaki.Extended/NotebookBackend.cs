namespace Xwt.GtkBackend {
    public partial class NotebookBackend  {
        void Compose() {
            Widget.BorderWidth = 0;
#if !XWT_GTKSHARP3            
            Widget.TabHborder = 0;
            Widget.TabVborder = 0;
#endif            
            Widget.ShowBorder = false;
        }
    }
}