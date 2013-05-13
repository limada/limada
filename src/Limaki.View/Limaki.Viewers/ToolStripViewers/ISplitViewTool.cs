namespace Limaki.Viewers.ToolStripViewers {

    public interface ISplitViewTool {
        SplitViewMode ViewMode { get; set; }
        void CheckBackForward(ISplitView splitView);
        void AttachSheets();
    }
}