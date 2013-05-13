namespace Limaki.Viewers.ToolStripViewers {

    public interface ILayoutToolStripViewerBackend:IToolStripViewerBackend {
        void AttachStyleSheet(string sheetName);
        void DetachStyleSheet(string oldSheetName);
    }
}