namespace Limaki.UseCases.Viewers.ToolStrips {
    public interface ILayoutTool {
        void AttachStyleSheet(string sheetName);
        void DetachStyleSheet(string oldSheetName);
    }
}