namespace Limaki.Viewers.ToolStripViewers {

    public interface ISplitViewToolStripBackend:IToolStripViewerBackend {
        SplitViewMode ViewMode { get; set; }
        void CheckBackForward(ISplitView splitView);
        void AttachSheets();
    }
}